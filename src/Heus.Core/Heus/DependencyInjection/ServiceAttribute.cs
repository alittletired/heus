using System;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute : Attribute
    {
        public bool IncludeDefaults { get; set; } = true;

        public bool IncludeSelf { get; set; } = true;
        public ServiceLifetime? Lifetime { get; }

        public ServiceAttribute()
        {
        }
        public ServiceAttribute(ServiceLifetime lifetime)
        {
            Lifetime = lifetime;
        }
    }
}