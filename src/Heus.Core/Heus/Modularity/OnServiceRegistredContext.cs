using System;
using System.Collections.Generic;
using Heus.DynamicProxy;
using JetBrains.Annotations;

namespace Heus.Modularity
{
    public class OnServiceRegistredContext 
    {
        public List<IInterceptor> Interceptors { get; }

        public virtual Type ServiceType { get; }

        public virtual Type ImplementationType { get; }

        public OnServiceRegistredContext(Type serviceType, [NotNull] Type implementationType)
        {
            ServiceType = Check.NotNull(serviceType, nameof(serviceType));
            ImplementationType = Check.NotNull(implementationType, nameof(implementationType));

            Interceptors = new List<IInterceptor>();
        }
    }
}