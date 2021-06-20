using Microsoft.Extensions.DependencyInjection;

namespace Heus.Modularity
{
    public interface IPostConfigureServices
    {
        void PostConfigureServices(ServiceConfigurationContext context);
    }
}