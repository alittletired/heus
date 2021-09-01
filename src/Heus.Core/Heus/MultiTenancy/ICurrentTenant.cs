using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heus.MultiTenancy
{
    public interface ICurrentTenant
    {
        bool IsAvailable { get; }

        [CanBeNull]
        Guid? Id { get; }

        [CanBeNull]
        string Name { get; }

        IDisposable Change(Guid? id, string name = null);
    }
}
