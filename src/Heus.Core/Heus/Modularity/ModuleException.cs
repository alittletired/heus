using System;
using System.Collections.Generic;
using System.Text;

namespace Heus.Modularity
{
    internal class ModuleException:Exception
    {

        public ModuleException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

    }
}
