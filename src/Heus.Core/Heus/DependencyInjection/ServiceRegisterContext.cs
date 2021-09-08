using Microsoft.Extensions.DependencyInjection;

namespace Heus.DependencyInjection
{
    public class ServiceRegisterContext
    {
        public IServiceCollection Services { get; }
        public IServiceRegistrar Next { get; }

        public ServiceRegisterContext(IServiceCollection services)
        {
            Services = services;
        }
    }
}