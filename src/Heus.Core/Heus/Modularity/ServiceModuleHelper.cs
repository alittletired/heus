using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Heus.Modularity
{
    internal static class ServiceModuleHelper
    {
        public static List<Type> FindAllModuleTypes(Type startupModuleType, ILogger logger)
        {
            var moduleTypes = new List<Type>();
            logger.LogInformation( "Loaded Service modules:");
            AddModuleAndDependenciesRecursively(moduleTypes, startupModuleType, logger);
            return moduleTypes;
        }

        public static List<Type> FindDependedModuleTypes(Type moduleType)
        {
            CheckSeriveModuleType(moduleType);

            var dependencies = new List<Type>();

            var dependsOnAttributes = moduleType
                .GetCustomAttributes()
                .OfType<DependsOnAttribute>();

            foreach (var dependsOn in dependsOnAttributes)
            {
                foreach (var dependedModuleType in dependsOn.GetDependedTypes())
                {
                    dependencies.AddIfNotContains(dependedModuleType);
                }
            }

            return dependencies;
        }
        internal static void CheckSeriveModuleType(Type moduleType)
        {
            if (!IsSeriveModule(moduleType))
            {
                throw new ArgumentException("Given type is not an ABP module: " + moduleType.AssemblyQualifiedName);
            }
        }

        public static bool IsSeriveModule(Type type)
        {
            var typeInfo = type.GetTypeInfo();

            return
                typeInfo.IsClass &&
                !typeInfo.IsAbstract &&
                !typeInfo.IsGenericType &&
                typeof(ServiceModule).GetTypeInfo().IsAssignableFrom(type);
        }
        private static void AddModuleAndDependenciesRecursively(
            List<Type> moduleTypes,
            Type moduleType,
            ILogger logger,
            int depth = 0)
        {
            CheckSeriveModuleType(moduleType);

            if (moduleTypes.Contains(moduleType))
            {
                return;
            }

            moduleTypes.Add(moduleType);
            logger.Log(LogLevel.Information, $"{new string(' ', depth * 2)}- {moduleType.FullName}");

            foreach (var dependedModuleType in FindDependedModuleTypes(moduleType))
            {
                AddModuleAndDependenciesRecursively(moduleTypes, dependedModuleType, logger, depth + 1);
            }
        }
    }
}
