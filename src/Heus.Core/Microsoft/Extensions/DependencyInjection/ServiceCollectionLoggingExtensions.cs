﻿using Microsoft.Extensions.Logging;
using Heus.Logging;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionLoggingExtensions
    {
        public static ILogger<T> GetInitLogger<T>(this IServiceCollection services)
        {
            return services.GetSingletonInstance<IInitLoggerFactory>().Create<T>();
        }
    }
}
