using Microsoft.Extensions.DependencyInjection;

namespace Heus.Modularity
{
    public interface IPreConfigureServices
    {
        void PreConfigureServices(ServiceConfigurationContext context);
    }
}
