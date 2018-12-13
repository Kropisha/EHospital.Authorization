    using System.Text;
    using Microsoft.IdentityModel.Tokens;

namespace EHospital.Authorization.WebAPI.Helpers
{
    /// <summary>
    /// For setting information regards to token
    /// </summary>
    public class AuthorizationOptions
    {
        public const string Issuer = "EHospital.AuthorizationServer";
        public const string Audience = "http://localhost:44386/";
        public const int Lifetime = 30;
        const string Key = "bhiyrbgi893nixmbh47yhfbuj";

        /// <summary>
        /// Encrypt token's key
        /// </summary>
        /// <returns>secret key</returns>
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
