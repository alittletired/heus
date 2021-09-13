using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heus.Modularity
{
    public class ServiceConfigurationContext
    {
        public IServiceCollection Services { get; }

        public IDictionary<string, object> Items { get; }

        public ServiceConfigurationContext(IServiceCollection services)
        {
            Services = services;
            Items = new Dictionary<string, object>();
        }
    }
}
