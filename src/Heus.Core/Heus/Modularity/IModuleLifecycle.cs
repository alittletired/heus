using JetBrains.Annotations;

namespace Heus.Modularity
{
    public interface IModuleLifecycle
    {
        void Initialize([NotNull] ApplicationInitializationContext context);

        void Shutdown([NotNull] ApplicationShutdownContext context);
    }
}