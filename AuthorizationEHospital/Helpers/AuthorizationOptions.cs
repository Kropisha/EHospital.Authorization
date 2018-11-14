using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHospital.Authorization.Models
{
    public class AuthorizationOptions
    {
        public const string ISSUER = "eHospital.AuthorizationServer"; 
        public const string AUDIENCE = "http://localhost:44386/"; 
        const string KEY = "bhiyrbgi893nixmbh47yhfbuj";
        public const int LIFETIME = 30; 

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
