using Microsoft.Extensions.DependencyInjection;

namespace Heus.DependencyInjection
{
    public class TransientDependencyAttribute: DependencyAttribute
    {
        public TransientDependencyAttribute() : base(ServiceLifetime.Transient)
        {
        }
    }
}