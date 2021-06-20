using System;

namespace Heus.DependencyInjection
{
    public interface IClientScopeServiceProviderAccessor
    {
        IServiceProvider ServiceProvider { get; }
    }
}
