using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Heus
{
    public class ApplicationCreationOptions
    {
        [NotNull]
        public IServiceCollection Services { get; }

        public ApplicationCreationOptions(IServiceCollection services)
        {
            Services = services;
        }
    }
}