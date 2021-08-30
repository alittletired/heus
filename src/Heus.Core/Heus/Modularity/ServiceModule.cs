using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Heus.Modularity
{
    public abstract class ServiceModule
    {
        protected internal bool SkipAutoServiceRegistration { get; protected set; }
        private ServiceConfigurationContext _serviceConfigurationContext;
        /// <summary>
        /// 服务配置上下文
        /// </summary>
        /// <exception cref="HeusException"></exception>
        protected internal ServiceConfigurationContext ServiceConfigurationContext
        {
            get
            {
                if (_serviceConfigurationContext == null)
                {
                    throw new HeusException(
                        $"{nameof(ServiceConfigurationContext)} is only available in the {nameof(ConfigureServices)}, {nameof(PreConfigureServices)} and {nameof(PostConfigureServices)} methods.");
                }

                return _serviceConfigurationContext;
            }
            internal set => _serviceConfigurationContext = value;
        }
        /// <summary>
        /// 配置服务（前）
        /// </summary>
        /// <param name="context"></param>
        public virtual void PreConfigureServices(ServiceConfigurationContext context)
        {
        }

        /// <summary>
        /// 配置服务（中）
        /// </summary>
        /// <param name="context"></param>
        public virtual void ConfigureServices(ServiceConfigurationContext context)
        {
        }

        /// <summary>
        /// 配置服务（后）
        /// </summary>
        /// <param name="context"></param>
        public virtual void PostConfigureServices(ServiceConfigurationContext context)
        {
        }

       
    }
}
