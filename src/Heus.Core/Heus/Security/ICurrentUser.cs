using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Heus.Security
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }
        long? UserId { get; }
        string? UserName { get; }
        long? TenantId { get; }
        public ClaimsPrincipal? Principal { get; }
        IDisposable SetCurrent(ClaimsPrincipal principal);
    }
}
