using System;
using System.Linq;
using System.Security.Claims;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Mvc
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetSubject(this ClaimsPrincipal claimsPrincipal)
        {
            var subjectValue = claimsPrincipal.GetFirstClaimValue("sub");
            return string.IsNullOrWhiteSpace(subjectValue) ? Guid.Empty : Guid.Parse(subjectValue);
        }

        public static string GetGivenName(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.GetFirstClaimValue("given_name");
        }

        public static string GetSurname(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.GetFirstClaimValue("family_name");
        }

        public static string GetFirstClaimValue(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            return claimsPrincipal.Claims.FirstOrDefault(x => x.Type == claimType)?.Value;
        }
    }
}