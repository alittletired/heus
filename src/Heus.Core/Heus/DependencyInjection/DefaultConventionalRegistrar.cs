using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Heus.DependencyInjection
{
    public class DefaultConventionalRegistrar : ConventionalRegistrarBase
    {
        public override void AddType(IServiceCollection services, Type type)
        {
            if (IsConventionalRegistrationDisabled(type))
            {
                return;
            }

            var dependencyAttribute = GetDependencyAttributeOrNull(type);
            var lifeTime = GetLifeTimeOrNull(type, dependencyAttribute);

            if (lifeTime == null)
            {
                return;
            }

            var exposedServiceTypes = GetExposedServiceTypes(type);
            foreach (var exposedServiceType in exposedServiceTypes)
            {
                var serviceDescriptor = CreateServiceDescriptor(
                    type,
                    exposedServiceType,
                    exposedServiceTypes,
                    lifeTime.Value
                );

                //允许外层重新注入
                services.TryAdd(serviceDescriptor);
            }
        }

        protected virtual List<Type> GetExposedServiceTypes(Type type)
        {
            return ExposedServiceExplorer.GetExposedServices(type);
        }

        protected virtual ServiceDescriptor CreateServiceDescriptor(
            Type implementationType,
            Type exposingServiceType,
            List<Type> allExposingServiceTypes,
            ServiceLifetime lifeTime)
        {
       

            return ServiceDescriptor.Describe(
                exposingServiceType,
                implementationType,
                lifeTime
            );
        }

    

        protected virtual DependencyAttribute? GetDependencyAttribute(Type type)
        {
            return type.GetCustomAttributes().OfType<DependencyAttribute>().FirstOrDefault();
        }

        protected virtual ServiceLifetime? GetLifeTimeOrNull(Type type, [CanBeNull] DependencyAttribute dependencyAttribute)
        {
            return dependencyAttribute?.Lifetime ?? GetServiceLifetimeFromClassHierarchy(type);
        }

        protected virtual ServiceLifetime? GetServiceLifetimeFromClassHierarchy(Type type)
        {
            if (typeof(TransientDependencyAttribute).GetTypeInfo().IsAssignableFrom(type))
            {
                return ServiceLifetime.Transient;
            }

            if (typeof(SingletonDependencyAttribute).GetTypeInfo().IsAssignableFrom(type))
            {
                return ServiceLifetime.Singleton;
            }

            if (typeof(ScopedDependencyAttribute).GetTypeInfo().IsAssignableFrom(type))
            {
                return ServiceLifetime.Scoped;
            }

            return null;
        }
    }
}