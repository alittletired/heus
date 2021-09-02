using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Heus.DependencyInjection;
using Heus.DynamicProxy;
using Heus.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Heus.Auditing
{
    [TransientDependency]
   public class AuditingInterceptor : IInterceptor
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AuditingInterceptor(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public  async Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            using (var serviceScope = _serviceScopeFactory.CreateScope())
            {
                var auditingHelper = serviceScope.ServiceProvider.GetRequiredService<IAuditingHelper>();
                var auditingOptions = serviceScope.ServiceProvider.GetRequiredService<IOptions<AuditingOptions>>().Value;

                if (!ShouldIntercept(invocation, auditingOptions, auditingHelper))
                {
                    await invocation.ProceedAsync();
                    return;
                }
                
                var auditingManager = serviceScope.ServiceProvider.GetRequiredService<IAuditingManager>();
                if (auditingManager.Current != null)
                {
                    await ProceedByLoggingAsync(invocation, auditingHelper, auditingManager.Current);
                }
                else
                {
                    var currentUser = serviceScope.ServiceProvider.GetRequiredService<ICurrentUser>();
                    await ProcessWithNewAuditingScopeAsync(invocation, auditingOptions, currentUser, auditingManager, auditingHelper);
                }
            }
        }

        protected virtual bool ShouldIntercept(IAbpMethodInvocation invocation,
            AbpAuditingOptions options,
            IAuditingHelper auditingHelper)
        {
            if (!options.IsEnabled)
            {
                return false;
            }
            
            if (AbpCrossCuttingConcerns.IsApplied(invocation.TargetObject, AbpCrossCuttingConcerns.Auditing))
            {
                return false;
            }

            if (!auditingHelper.ShouldSaveAudit(invocation.Method))
            {
                return false;
            }

            return true;
        }
        
        private static async Task ProceedByLoggingAsync(
            IAbpMethodInvocation invocation,
            IAuditingHelper auditingHelper,
            IAuditLogScope auditLogScope)
        { 
            var auditLog = auditLogScope.Log;
            var auditLogAction = auditingHelper.CreateAuditLogAction(
                auditLog,
                invocation.TargetObject.GetType(),
                invocation.Method,
                invocation.Arguments
            );
            
            var stopwatch = Stopwatch.StartNew();

            try
            {
                await invocation.ProceedAsync();
            }
            catch (Exception ex)
            {
                auditLog.Exceptions.Add(ex);
                throw;
            }
            finally
            {
                stopwatch.Stop();
                auditLogAction.ExecutionDuration = Convert.ToInt32(stopwatch.Elapsed.TotalMilliseconds);
                auditLog.Actions.Add(auditLogAction);
            }
        }
        
        private async Task ProcessWithNewAuditingScopeAsync(
            IAbpMethodInvocation invocation, 
            AbpAuditingOptions options,
            ICurrentUser currentUser,
            IAuditingManager auditingManager,
            IAuditingHelper auditingHelper)
        {
            var hasError = false;
            using (var saveHandle = auditingManager.BeginScope())
            {
                try
                {
                    await ProceedByLoggingAsync(invocation, auditingHelper, auditingManager.Current);

                    Debug.Assert(auditingManager.Current != null);
                    if (auditingManager.Current.Log.Exceptions.Any())
                    {
                        hasError = true;
                    }
                }
                catch (Exception)
                {
                    hasError = true;
                    throw;
                }
                finally
                {
                    if (ShouldWriteAuditLog(invocation, options, currentUser, hasError))
                    {
                        await saveHandle.SaveAsync();
                    }
                }
            }
        }
        
        private bool ShouldWriteAuditLog(
            IAbpMethodInvocation invocation,
            AbpAuditingOptions options,
            ICurrentUser currentUser,
            bool hasError)
        {
            if (options.AlwaysLogOnException && hasError)
            {
                return true;
            }

            if (!options.IsEnabledForAnonymousUsers && !currentUser.IsAuthenticated)
            {
                return false;
            }

            if (!options.IsEnabledForGetRequests &&
                invocation.Method.Name.StartsWith("Get",StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }
    }
}