using System;
using System.Collections.Generic;

namespace Heus.DependencyInjection
{
    public class ServiceRegistrationActionList : List<Action<IOnServiceRegistredContext>>
    {
        public bool IsClassInterceptorsDisabled { get; set; }
    }
}