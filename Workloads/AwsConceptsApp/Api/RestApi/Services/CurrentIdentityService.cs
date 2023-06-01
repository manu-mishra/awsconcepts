using Application.Identity;
using System.Security.Claims;

namespace RestApi.Services
{
    internal class CurrentIdentityService : IIdentity
    {
        public CurrentIdentityService(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor != null && httpContextAccessor.HttpContext != null && httpContextAccessor.HttpContext.User.Claims != null)
            {
                Claims = httpContextAccessor.HttpContext.User.Claims;
                this.FillId().FillGivenName().FillSurName().FillNickName().FillEmail().FillVerified();
            }

            if (string.IsNullOrEmpty(Id))
            {
                Id = "Anonomous";
            }
        }

        public bool IsAdmin(string? email = null)
        {
            if (!string.IsNullOrEmpty(email))
            {
                return IdentityHelper.IsAdminUser(email);
            }

            if (!string.IsNullOrEmpty(this.Email))
            {
                return IdentityHelper.IsAdminUser(this.Email);
            }

            return false;
        }

        public IEnumerable<Claim> Claims;

        public string Id { get; set; }

        public string GivenName { get; set; }

        public string Surname { get; set; }

        public string NickName { get; set; }

        public string Email { get; set; }

        public bool Verified { get; set; }

    }

    internal static class IdentityHelper
    {
        internal const string IdClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        internal const string GivenNameClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname";
        internal const string SurnameClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname";
        internal const string NickNameClaim = "nickname";
        internal const string VerifiedClaim = "email_verified";
        internal const string EmailClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";

        internal static CurrentIdentityService FillId(this CurrentIdentityService identity)
        {
            var claim = identity.Claims.FirstOrDefault(predicate: x => x.Type == IdClaim);
            if (claim != null)
                identity.Id = claim.Value;
            return identity;
        }
        internal static CurrentIdentityService FillGivenName(this CurrentIdentityService identity)
        {
            var claim = identity.Claims.FirstOrDefault(predicate: x => x.Type == GivenNameClaim);
            if (claim != null)
                identity.GivenName = claim.Value;
            return identity;
        }
        internal static CurrentIdentityService FillSurName(this CurrentIdentityService identity)
        {
            var claim = identity.Claims.FirstOrDefault(predicate: x => x.Type == SurnameClaim);
            if (claim != null)
                identity.Surname = claim.Value;
            return identity;
        }
        internal static CurrentIdentityService FillNickName(this CurrentIdentityService identity)
        {
            var claim = identity.Claims.FirstOrDefault(predicate: x => x.Type == NickNameClaim);
            if (claim != null)
                identity.NickName = claim.Value;
            return identity;
        }
        internal static CurrentIdentityService FillEmail(this CurrentIdentityService identity)
        {
            var claim = identity.Claims.FirstOrDefault(predicate: x => x.Type == EmailClaim);
            if (claim != null)
                identity.Email = claim.Value;
            return identity;
        }
        internal static CurrentIdentityService FillVerified(this CurrentIdentityService identity)
        {
            var claim = identity.Claims.FirstOrDefault(predicate: x => x.Type == VerifiedClaim);
            if (claim != null)
                identity.Verified = bool.Parse(claim.Value);
            return identity;
        }
        internal static string[] AdminList()
        {
            // locally setup in lambda api launch settings
            var adminEmailListVariable = Environment.GetEnvironmentVariable("globalAdmins");
            if (!string.IsNullOrWhiteSpace(adminEmailListVariable))
            {
                return adminEmailListVariable.ToLower().Split(',');
            }
            return new string[0];
        }
        internal static bool IsAdminUser(string Email)
        {
            return AdminList().Contains(Email.ToLower());
        }
    }

}
