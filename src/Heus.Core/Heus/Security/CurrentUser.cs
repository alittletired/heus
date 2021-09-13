using Heus.DependencyInjection;
using Heus.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Heus.Security
{
    [Service]
    internal class CurrentUser : ICurrentUser
    {
        private static readonly AsyncLocal<ClaimsPrincipal?> CurrentPrincipal = new();
        public bool IsAuthenticated => Principal != null;

        public ClaimsPrincipal? Principal => CurrentPrincipal.Value;

        public long? UserId => this.FindClaimValue<long>(ClaimTypes.Name);

        public string? UserName => this.FindClaimValue(ClaimTypes.NameIdentifier);

        public long? TenantId => this.FindClaimValue<long>(TenantConst.TenantIdKey);

        public IDisposable SetCurrent(ClaimsPrincipal principal)
        {
            var parent = Principal;
            CurrentPrincipal.Value = principal;
            return new DisposeAction(() =>
            {
                CurrentPrincipal.Value = parent;
            });
        }
    }
}
