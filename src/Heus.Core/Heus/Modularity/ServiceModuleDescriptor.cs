using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Heus.Modularity
{
    internal class ServiceModuleDescriptor
    {
        public Type Type { get; }

        public Assembly Assembly { get; }

        public ServiceModule Instance { get; }

        public bool IsLoadedAsPlugIn { get; }

        public IReadOnlyList<ServiceModuleDescriptor> Dependencies => _dependencies.ToImmutableList();
        private readonly List<ServiceModuleDescriptor> _dependencies;

        public ServiceModuleDescriptor(
            [NotNull] Type type,
            [NotNull] ServiceModule instance,
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

            _dependencies = new List<ServiceModuleDescriptor>();
        }

        public void AddDependency(ServiceModuleDescriptor descriptor)
        {
            _dependencies.AddIfNotContains(descriptor);
        }

        public override string ToString()
        {
            return $"[AbpModuleDescriptor {Type.FullName}]";
        }
    }
}
