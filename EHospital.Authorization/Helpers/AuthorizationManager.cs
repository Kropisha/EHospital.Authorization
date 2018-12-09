using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EHospital.Authorization.Data.Data;

namespace EHospital.Authorization.WebAPI.Helpers
{
    /// <summary>
    /// For some authorization logic
    /// </summary>
    public sealed class AuthorizationManager
    {
        private readonly IDataProvider _appDbContext;

        public AuthorizationManager(IDataProvider data)
        {
            _appDbContext = data;
        }

        /// <summary>
        /// Get identity by credentials
        /// </summary>
        /// <param name="userLogin">user's login</param>
        /// <param name="password">user's password</param>
        /// <returns>user</returns>
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
                return await GetIdentity(userLogin, userToVerify);
            }

            return null;
        }

        /// <summary>
        /// Verify user
        /// </summary>
        /// <param name="userLogin">user's login</param>
        /// <param name="userToVerify">user's id</param>
        /// <returns>identity</returns>
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
