using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Heus.Heus.MultiTenancy
{
    internal class CurrentTenant : ICurrentTenant
    {
        private readonly static AsyncLocal<BasicTenantInfo> _currentScope = new AsyncLocal<BasicTenantInfo>();
        public virtual bool IsAvailable => Id.HasValue;

        public virtual Guid? Id => _currentScope.Value?.TenantId;

        public string Name => _currentScope.Value?.Name;
        public IDisposable Change(Guid? tenantId, string name = null)
        {
            var parentScope = _currentScope.Value;
            _currentScope.Value = new BasicTenantInfo(tenantId, name);
            return new DisposeAction(() =>
            {
                _currentScope.Value = parentScope;
            });
        }
    }
}
