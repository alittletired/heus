using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.DependencyInjection
{
    public interface IServiceRegistrar
    {
        
        void Handle(ServiceRegisterContext context, Type type);
    }
}