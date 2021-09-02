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
    internal class CurrentUser : ICurrentUser, TransientDependencyAttribute
    {
        private static readonly AsyncLocal<ClaimsPrincipal?> _currentPrincipal = new AsyncLocal<ClaimsPrincipal?>();
        public bool IsAuthenticated => Principal != null;

        public ClaimsPrincipal? Principal => _currentPrincipal.Value;

        public long? UserId => this.FindClaimValue<long>(ClaimTypes.Name);

        public string? UserName => this.FindClaimValue(ClaimTypes.NameIdentifier);

        public long? TenantId => this.FindClaimValue<long>(TenantConst.TenantIdKey);

        public IDisposable SetCurrent(ClaimsPrincipal principal)
        {
            var parent = Principal;
            _currentPrincipal.Value = principal;
            return new DisposeAction(() =>
            {
                _currentPrincipal.Value = parent;
            });
        }
    }
}
