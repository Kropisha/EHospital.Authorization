namespace EHospital.Authorization.WebApi
{
    using Microsoft.IdentityModel.Tokens;
    using System.Text;

    public class AuthorizationOptions
    {
        public const string ISSUER = "EHospital.AuthorizationServer";
        public const string AUDIENCE = "http://localhost:44386/";
        const string KEY = "bhiyrbgi893nixmbh47yhfbuj";
        public const int LIFETIME = 30;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
