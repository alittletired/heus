using System.Collections.Generic;
namespace Heus.Modularity
{
    public interface IModuleContainer
    {
        IReadOnlyList<ServiceModuleDescriptor> Modules { get; }
    }
}