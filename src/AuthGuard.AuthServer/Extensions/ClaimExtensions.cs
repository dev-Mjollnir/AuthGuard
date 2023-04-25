using OpenIddict.Abstractions;
using System.Collections.Immutable;
using System.Security.Claims;

namespace AuthGuard.AuthServer.Extensions
{
    public static class ClaimExtensions
    {
        public static ClaimsIdentity AddClaim(this ClaimsIdentity identity, string type, string value, params string[]? destinations)
        {
            identity.AddClaim(new Claim(type, value).SetDestinations(destinations));
            return identity;
        }
    }
}
