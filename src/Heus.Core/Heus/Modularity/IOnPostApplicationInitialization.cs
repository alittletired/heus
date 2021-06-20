using JetBrains.Annotations;

namespace Heus.Modularity
{
    public interface IOnPostApplicationInitialization
    {
        void OnPostApplicationInitialization([NotNull] ApplicationInitializationContext context);
    }
}