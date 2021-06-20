using System;
using JetBrains.Annotations;

namespace Heus.Modularity.PlugIns
{
    public interface IPlugInSource
    {
        [NotNull]
        Type[] GetModules();
    }
}