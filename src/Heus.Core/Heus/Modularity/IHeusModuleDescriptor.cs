using System;
using System.Collections.Generic;
using System.Reflection;

namespace Heus.Modularity
{
    public interface IHeusModuleDescriptor
    {
        Type Type { get; }

        Assembly Assembly { get; }

        IHeusModule Instance { get; }

        bool IsLoadedAsPlugIn { get; }

        IReadOnlyList<IHeusModuleDescriptor> Dependencies { get; }
    }
}