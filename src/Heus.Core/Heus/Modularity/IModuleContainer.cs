using System.Collections.Generic;
using JetBrains.Annotations;

namespace Heus.Modularity
{
    public interface IModuleContainer
    {
        [NotNull]
        IReadOnlyList<ServiceModuleDescriptor> Modules { get; }
    }
}