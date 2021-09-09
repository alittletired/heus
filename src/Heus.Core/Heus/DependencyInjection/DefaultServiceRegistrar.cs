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

        }
    }
}