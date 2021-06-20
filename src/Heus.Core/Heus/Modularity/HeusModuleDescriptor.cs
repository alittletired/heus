using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using JetBrains.Annotations;

namespace Heus.Modularity
{
    public class HeusModuleDescriptor : IHeusModuleDescriptor
    {
        public Type Type { get; }

        public Assembly Assembly { get; }

        public IHeusModule Instance { get; }

        public bool IsLoadedAsPlugIn { get; }

        public IReadOnlyList<IHeusModuleDescriptor> Dependencies => _dependencies.ToImmutableList();
        private readonly List<IHeusModuleDescriptor> _dependencies;

        public HeusModuleDescriptor(
            [NotNull] Type type, 
            [NotNull] IHeusModule instance, 
            bool isLoadedAsPlugIn)
        {
            Check.NotNull(type, nameof(type));
            Check.NotNull(instance, nameof(instance));

            if (!type.GetTypeInfo().IsAssignableFrom(instance.GetType()))
            {
                throw new ArgumentException($"Given module instance ({instance.GetType().AssemblyQualifiedName}) is not an instance of given module type: {type.AssemblyQualifiedName}");
            }

            Type = type;
            Assembly = type.Assembly;
            Instance = instance;
            IsLoadedAsPlugIn = isLoadedAsPlugIn;

            _dependencies = new List<IHeusModuleDescriptor>();
        }

        public void AddDependency(IHeusModuleDescriptor descriptor)
        {
            _dependencies.AddIfNotContains(descriptor);
        }

        public override string ToString()
        {
            return $"[AbpModuleDescriptor {Type.FullName}]";
        }
    }
}
