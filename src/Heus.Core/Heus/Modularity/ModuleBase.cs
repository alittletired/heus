using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Heus.Modularity
{
    public abstract class ModuleBase
    {
        public virtual bool SkipAutoServiceRegistration { get; protected set; }
      
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
