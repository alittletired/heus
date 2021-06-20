using System;
using JetBrains.Annotations;

namespace Heus.Modularity
{
    public interface IDependedTypesProvider
    {
        [NotNull]
        Type[] GetDependedTypes();
    }
}