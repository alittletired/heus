using System;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DependencyAttribute : Attribute
    {
        public   ServiceLifetime Lifetime { get;  }
      
        public DependencyAttribute(ServiceLifetime lifetime)
        {
            Lifetime = lifetime;
        }
    }
}