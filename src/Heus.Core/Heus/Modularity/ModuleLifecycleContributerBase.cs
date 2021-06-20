namespace Heus.Modularity
{
    public abstract class ModuleLifecycleContributorBase : IModuleLifecycleContributor
    {
        public virtual void Initialize(ApplicationInitializationContext context, IHeusModule module)
        {
        }

        public virtual void Shutdown(ApplicationShutdownContext context, IHeusModule module)
        {
        }
    }
}