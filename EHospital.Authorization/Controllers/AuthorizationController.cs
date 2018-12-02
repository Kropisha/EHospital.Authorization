namespace EHospital.Authorization.WebAPI
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Threading.Tasks;
    using EHospital.Authorization.BusinessLogic;
    using EHospital.Authorization.Data;
    using EHospital.Authorization.Model;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;
    using Newtonsoft.Json;

    /// <summary>
    /// Controller for authorization
    /// </summary>
    [Route("api/[controller]")]
    public class AuthorizationController : Controller
    {
        private readonly ILogging Log;

        private readonly IDataProvider _appDbContext;

        /// <summary>
        /// an instance of authorization manager
        /// </summary>
        private AuthorizationManager authorizationManager;

        public AuthorizationController(IDataProvider data, ILogging logger)
        {
            _appDbContext = data;
            Log = logger;
        }

        /// <summary>
        /// Log in to application
        /// </summary>
        /// <param name="credentials">user's login and password</param>
        /// <returns>token</returns>
        // POST api/auth/login
        [HttpPost("login")]
        public ActionResult LogIn([FromBody]CredentialsViewModel credentials)
        {
            Log.LogInfo("Set credentials for authorization.");
            if (!this.ModelState.IsValid)
            {
                Log.LogError("Incorrect format of input.");
                return this.BadRequest(this.ModelState);
            }

            authorizationManager = new AuthorizationManager(_appDbContext);
            Log.LogInfo("Check the user.");
            var identity = this.authorizationManager.GetClaimsIdentity(credentials.UserLogin, credentials.Password);
            if (identity.Result == null)
            {
                Log.LogError("Invalid username or password.");
                return this.BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.", this.ModelState));
            }
            else
            {
                Log.LogInfo("Set an access token.");
                var jwt = this.GetToken(credentials.UserLogin);
                if (jwt == null)
                {
                    return this.BadRequest("Invalid username or password.");
                }

                Log.LogInfo("Successful authorize.");
                return new OkObjectResult(jwt.Result);
            }
        }

        /// <summary>
        /// Log out from application
        /// </summary>
        /// <param name="userId">user's id</param>
        /// <returns>Ok</returns>
        [HttpPost("logout")]
        public async Task<IActionResult> LogOut(int userId)
        {
            await _appDbContext.LogOut(userId);
            return new OkObjectResult("Log out succes.");
        }

        /// <summary>
        /// Get new token
        /// </summary>
        /// <param name="username">login</param>
        /// <returns>token</returns>
        private async Task<string> GetToken(string username)
        {
            int userId = await _appDbContext.FindByLogin(username);

            var identity = await authorizationManager.GetIdentity(username, userId);
            if (identity == null)
            {
                return null;
            }

            Log.LogInfo("Set token options.");
            var now = DateTime.Now;

            var jwt = new JwtSecurityToken(
                    issuer: AuthorizationOptions.ISSUER,
                    audience: AuthorizationOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthorizationOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthorizationOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            Log.LogInfo("Set session options.");
            Sessions start = new Sessions()
            {
                Token = encodedJwt,
                UserId = userId,
                ExpiredDate = now.Add(TimeSpan.FromMinutes(AuthorizationOptions.LIFETIME))
            };

            Log.LogInfo("Check for previous session.");
            if (await _appDbContext.IsExistPreviousSession(userId))
            {
                Log.LogInfo("The session was founded. I`ll delete it.");
                await _appDbContext.DeleteSessions(userId);
                Log.LogInfo("Successfull delete.");
            }

            Log.LogInfo("Add session");
            await _appDbContext.AddSession(start);
            Log.LogInfo("Session was add.");
            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            Log.LogInfo("Return session's token");
            this.Response.ContentType = "application/json";
            await this.Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
            return encodedJwt;
        }
    }
}