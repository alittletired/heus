using Microsoft.Extensions.DependencyInjection;

namespace Heus.DependencyInjection
{
    public class SingletonDependencyAttribute : DependencyAttribute
    {
        public SingletonDependencyAttribute() : base(ServiceLifetime.Singleton)
        {
        }
    }
}