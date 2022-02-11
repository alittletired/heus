using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Heus.Core;
using Heus.MultiTenancy;

namespace Heus.MultiTenancy
{
    internal class CurrentTenant : ICurrentTenant
    {
        private readonly static AsyncLocal<BasicTenantInfo?> CurrentScope = new();
        public  bool IsAvailable => Id.HasValue;

        public  Guid? Id => CurrentScope.Value?.TenantId;

        public string? Name => CurrentScope.Value?.Name;
        public IDisposable Change(Guid? tenantId, string? name = null)
        {
            var parentScope = CurrentScope.Value;
            CurrentScope.Value = new BasicTenantInfo(tenantId, name);
            return  DisposeAction.Create(() =>
            {
                CurrentScope.Value = parentScope;
            });
        }
    }
}
