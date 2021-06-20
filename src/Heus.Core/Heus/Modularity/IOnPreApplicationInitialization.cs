using JetBrains.Annotations;

namespace Heus.Modularity
{
    public interface IOnPreApplicationInitialization
    {
        void OnPreApplicationInitialization([NotNull] ApplicationInitializationContext context);
    }
}