using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heus.Heus.Data
{
    public interface IConnectionStringResolver
    {
        [NotNull]
        Task<string> ResolveAsync(string connectionStringName = null);
    }
}
