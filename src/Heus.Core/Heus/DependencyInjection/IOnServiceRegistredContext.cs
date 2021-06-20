using Heus.Collections;
using Heus.DynamicProxy;
using System;


namespace Heus.DependencyInjection
{
    public interface IOnServiceRegistredContext
    {
        ITypeList<IInterceptor> Interceptors { get; }

        Type ImplementationType { get; }
    }
}