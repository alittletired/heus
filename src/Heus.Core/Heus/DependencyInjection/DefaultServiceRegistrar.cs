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
    public class DefaultServiceRegistrar : IServiceRegistrar
    {
        public void Handle(ServiceRegisterContext context, Type type)
        {
            if (IsRegistrationDisabled(type))
                return;
            var serviceAttr = type.GetCustomAttribute<ServiceAttribute>();
            if (serviceAttr == null)
                return;
            foreach (var serviceType in GetExposedServiceTypes(type, serviceAttr))
            {
                var descriptor = ServiceDescriptor.Describe(
                    type,
                    serviceType,
                    serviceAttr.Lifetime ?? ServiceLifetime.Transient
                );
                context.Services.Add(descriptor);
            }
        }

        protected bool IsRegistrationDisabled(Type type)
        {
            return type.IsDefined(typeof(DisableRegistrationAttribute));
        }

        protected List<Type> GetExposedServiceTypes(Type type, ServiceAttribute attribute)
        {
            var serviceTypes = new List<Type>();
            if (attribute.IncludeSelf)
            {
                serviceTypes.Add(type);
            }

            if (attribute.IncludeDefaults)
            {
                serviceTypes.AddRange(GetDefaultServices(type));
            }

            return serviceTypes;
        }

        private  List<Type> GetDefaultServices(Type type)
        {
            var serviceTypes = new List<Type>();

            foreach (var interfaceType in type.GetTypeInfo().GetInterfaces())
            {
                var interfaceName = interfaceType.Name;

                if (interfaceName.StartsWith("I"))
                {
                    interfaceName = interfaceName.Right(interfaceName.Length - 1);
                }

                if (type.Name.EndsWith(interfaceName))
                {
                    serviceTypes.Add(interfaceType);
                }
            }

            return serviceTypes;
        }
    }
}