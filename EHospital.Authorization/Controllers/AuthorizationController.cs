namespace EHospital.Authorization.WebAPI
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Threading.Tasks;
    using EHospital.Authorization.BusinessLogic;
    using EHospital.Authorization.Model;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;
    using Newtonsoft.Json;

    [Route("api/[controller]")]
    public class AuthorizationController : Controller
    {
        private static readonly log4net.ILog Log = log4net.LogManager
                                                          .GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDataProvider _appDbContext;

        AuthorizationManager authorizationManager = new AuthorizationManager();

        public AuthorizationController(IDataProvider data)
        {
            _appDbContext = data;
        }

        // POST api/auth/login
        [HttpPost("login")]
        public IActionResult LogIn([FromBody]CredentialsViewModel credentials)
        {
            Log.Info("Set credentials for authorization.");
            if (!this.ModelState.IsValid)
            {
                Log.Error("Incorrect format of input.");
                return this.BadRequest(this.ModelState);
            }

            Log.Info("Check the user.");
            var identity = authorizationManager.GetClaimsIdentity(credentials.UserLogin, credentials.Password);
            if (identity.Result == null)
            {
                Log.Error("Invalid username or password.");
                return this.BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.", this.ModelState));
            }
            else
            {
                Log.Info("Set an access token.");
                var jwt = this.GetToken(credentials.UserLogin);
                if (jwt == null)
                {
                    return this.BadRequest("Invalid username or password.");
                }

                _appDbContext.Token = jwt.Result;
                Log.Info("Successful authorize.");
                return new OkObjectResult(jwt.Result);
            }
        }

        [HttpPost("logout")]
        public IActionResult LogOut(int userId)
        {
            _appDbContext.LogOut(userId);
            return new OkObjectResult("Log out succes.");
        }

        private async Task<string> GetToken(string username)
        {
            int userId = await _appDbContext.FindByLogin(username);

            var identity = await authorizationManager.GetIdentity(username, userId);
            if (identity == null)
            {
                return null;
            }

            Log.Info("Set token options.");
            var now = DateTime.Now;

            var jwt = new JwtSecurityToken(
                    issuer: AuthorizationOptions.ISSUER,
                    audience: AuthorizationOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthorizationOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthorizationOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            Log.Info("Set session options.");
            Sessions start = new Sessions()
            {
                Token = encodedJwt,
                UserId = userId,
                ExpiredDate = now.Add(TimeSpan.FromMinutes(AuthorizationOptions.LIFETIME))
            };

            Log.Info("Check for previous session.");
            if (await _appDbContext.IsExistPreviousSession(userId))
            {
                Log.Info("The session was founded. I`ll delete it.");
                await _appDbContext.DeleteSessions(userId);
                Log.Info("Successfull delete.");
            }

            Log.Info("Add session");
            await _appDbContext.AddSession(start);
            Log.Info("Session was add.");
            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            Log.Info("Return session's token");
            this.Response.ContentType = "application/json";
            await this.Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
            return encodedJwt;
        }
    }
}