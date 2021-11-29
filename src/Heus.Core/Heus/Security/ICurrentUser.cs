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
        ClaimsPrincipal? Principal { get; }
        IDisposable SetCurrent(ClaimsPrincipal principal);
        Claim? FindClaim(string claimType)
        {
            return Principal?.Claims.FirstOrDefault(c => c.Type == claimType);
        }
    }
}
