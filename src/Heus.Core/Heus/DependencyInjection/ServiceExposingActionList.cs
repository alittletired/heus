using System;
using System.Collections.Generic;

namespace Heus.DependencyInjection
{
    public class ServiceExposingActionList : List<Action<IOnServiceExposingContext>>
    {

    }
}