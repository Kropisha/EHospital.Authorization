    namespace EHospital.Authorization.WebAPI
{
    using System.Text;
    using Microsoft.IdentityModel.Tokens;

    /// <summary>
    /// For setting information regards to token
    /// </summary>
    public class AuthorizationOptions
    {
        public const string ISSUER = "EHospital.AuthorizationServer";
        public const string AUDIENCE = "http://localhost:44386/";
        public const int LIFETIME = 30;
        const string KEY = "bhiyrbgi893nixmbh47yhfbuj";

        /// <summary>
        /// Encrypt token's key
        /// </summary>
        /// <returns>secret key</returns>
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
