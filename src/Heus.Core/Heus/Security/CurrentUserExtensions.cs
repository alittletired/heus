using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Heus.Security
{
    public static class CurrentUserExtensions
    {
        [CanBeNull]
        public static string FindClaimValue(this ICurrentUser currentUser, string claimType)
        {
            return currentUser.FindClaim(claimType)?.Value;
        }

        public static T FindClaimValue<T>(this ICurrentUser currentUser, string claimType)
            where T : struct
        {
            var value = currentUser.FindClaimValue(claimType);
            if (value == null)
            {
                return default;
            }

            return value.To<T>();
        }
        public static Guid? FindUserId(this ClaimsPrincipal principal)
        {

            var userIdOrNull = principal.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdOrNull == null || userIdOrNull.Value.IsNullOrWhiteSpace())
            {
                return null;
            }

            if (Guid.TryParse(userIdOrNull.Value, out Guid guid))
            {
                return guid;
            }

            return null;
        }
    }
}
