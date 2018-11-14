using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using eHospital.Authorization.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace eHospital.Authorization.Controllers
{
    [Route("api/[controller]")]
    public class AuthorizationController : Controller
    {
        private readonly IDataProvider _appDbContext;

        public AuthorizationController(IDataProvider data)
        {
            _appDbContext = data;
        }

        private async Task<string> GetToken(string username, string password)
        {
            // var username = Request.Form["username"];
            // var password = Request.Form["password"];

            int userId = _appDbContext.GetUserPassword(password);

            var identity = GetIdentity(username, userId);
            if (identity == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Invalid username or password.");
            }

            var now = DateTime.Now;

            var jwt = new JwtSecurityToken(
                    issuer: AuthorizationOptions.ISSUER,
                    audience: AuthorizationOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthorizationOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthorizationOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            Sessions start = new Sessions()
            {
                Token = encodedJwt,
                UserId = userId,
                ExpiredDate = now.Add(TimeSpan.FromMinutes(AuthorizationOptions.LIFETIME))
            };

            _appDbContext.AddSession(start);
            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };
 
            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
            return encodedJwt;
        }

        // POST api/auth/login
        [HttpPost("login")]
        public IActionResult Post([FromBody]CredentialsViewModel credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = GetClaimsIdentity(credentials.UserLogin, credentials.Password);
            if (identity.Result == null)
            {
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.", ModelState));
            }
            else
            {
                var jwt = GetToken(credentials.UserLogin, credentials.Password);
             
                return new OkObjectResult(jwt.Result);
            }
        }

        private Task<ClaimsIdentity> GetClaimsIdentity(string userLogin, string password)
        {
            if (string.IsNullOrEmpty(userLogin) || string.IsNullOrEmpty(password))
            {
                return Task.FromResult<ClaimsIdentity>(null);
            }

            // get the user to verifty
            var userToVerify = _appDbContext.FindByLogin(userLogin);

            if (userToVerify == 0)
            {
                return Task.FromResult<ClaimsIdentity>(null);
            }

            // check the credentials
            if (_appDbContext.CheckPassword(password, userToVerify))
            {
                return Task.FromResult(GetIdentity(userLogin, userToVerify));
            }

            // Credentials are invalid, or account doesn't exist
            return Task.FromResult<ClaimsIdentity>(null);
        }

        private ClaimsIdentity GetIdentity(string userLogin, int userToVerify)
        {
            var claims = new List<Claim>
                {
                   new Claim(ClaimsIdentity.DefaultNameClaimType, userLogin),
                   new Claim(ClaimsIdentity.DefaultRoleClaimType, _appDbContext.GetRole(userToVerify))
                };

            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }
    }
}