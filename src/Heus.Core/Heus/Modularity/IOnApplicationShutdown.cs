using System;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Modularity
{
    public interface IOnApplicationShutdown
    {
        void OnApplicationShutdown([NotNull] ApplicationShutdownContext context);
    }
}