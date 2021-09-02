using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Heus.Auditing
{
    public class AuditingOptions
    {
        //TODO: Consider to add an option to disable auditing for application service methods?

        /// <summary>
        /// If this value is true, auditing will not throw an exceptions and it will log it when an error occurred while saving AuditLog.
        /// Default: true.
        /// </summary>
        public bool ThrowErrors { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Default: true.
        /// </summary>
        public bool DiabledForAnonymousUsers { get; set; }

        /// <summary>
        /// Audit log on exceptions.
        /// Default: true.
        /// </summary>
        public bool DiabledLogOnException { get; set; }

        public List<AuditLogContributor> Contributors { get; }

        public List<Type> IgnoredTypes { get; }

        public IEntityHistorySelectorList EntityHistorySelectors { get; }

        //TODO: Move this to asp.net core layer or convert it to a more dynamic strategy?
        /// <summary>
        /// Default: false.
        /// </summary>
        public bool IsEnabledForGetRequests { get; set; }

        public AuditingOptions()
        {
           

            Contributors = new List<AuditLogContributor>();

            IgnoredTypes = new List<Type>
            {
                typeof(Stream),
                typeof(Expression)
            };

            EntityHistorySelectors = new EntityHistorySelectorList();
        }
    }
}
