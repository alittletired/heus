using Heus.DependencyInjection;
using JetBrains.Annotations;

namespace Heus.Modularity
{
    public interface IModuleLifecycleContributor : ITransientDependency
    {
        void Initialize([NotNull] ApplicationInitializationContext context, [NotNull] IHeusModule module);

        void Shutdown([NotNull] ApplicationShutdownContext context, [NotNull] IHeusModule module);
    }
}
