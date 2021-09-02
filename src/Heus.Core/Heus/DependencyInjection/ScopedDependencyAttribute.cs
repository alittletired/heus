using Microsoft.Extensions.DependencyInjection;

namespace Heus.DependencyInjection
{
    public class ScopedDependencyAttribute : DependencyAttribute
    {
        public ScopedDependencyAttribute() : base(ServiceLifetime.Scoped)
        {
        }
    }
}