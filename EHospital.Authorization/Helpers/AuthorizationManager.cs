namespace EHospital.Authorization.WebAPI
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public sealed class AuthorizationManager
    {
        private readonly IDataProvider _appDbContext;

        public async Task<ClaimsIdentity> GetClaimsIdentity(string userLogin, string password)
        {
            if (string.IsNullOrEmpty(userLogin) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var userToVerify = await _appDbContext.FindByLogin(userLogin);

            if (userToVerify == 0)
            {
                return null;
            }

            if (await _appDbContext.CheckPassword(password, userToVerify))
            {
                return await this.GetIdentity(userLogin, userToVerify);
            }

            return null;
        }

        public async Task<ClaimsIdentity> GetIdentity(string userLogin, int userToVerify)
        {
            var claims = new List<Claim>
                {
                   new Claim(ClaimsIdentity.DefaultNameClaimType, userLogin),
                   new Claim(ClaimsIdentity.DefaultRoleClaimType, await _appDbContext.GetRole(userToVerify))
                };

            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(
                claims,
                "Token",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }
    }
}
