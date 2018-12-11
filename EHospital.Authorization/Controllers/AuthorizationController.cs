using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using EHospital.Authorization.BusinessLogic.Credentials;
using EHospital.Authorization.Data.Data;
using EHospital.Authorization.Model.Models;
using EHospital.Authorization.WebAPI.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace EHospital.Authorization.WebAPI.Controllers
{
    /// <summary>
    /// Controller for authorization
    /// </summary>
    [Route("api/[controller]")]
    public class AuthorizationController : Controller
    {
        private readonly ILogging _log;
     Htt

        private readonly IDataProvider _appDbContext;

        /// <summary>
        /// an instance of authorization manager
        /// </summary>
        private AuthorizationManager _authorizationManager;

        public AuthorizationController(IDataProvider data, ILogging logger)
        {
            _appDbContext = data;
            _log = logger;
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
            _log.LogInfo("Set credentials for authorization.");
            if (!ModelState.IsValid)
            {
                _log.LogError("Incorrect format of input.");
                return BadRequest(ModelState);
            }

            _authorizationManager = new AuthorizationManager(_appDbContext);
            _log.LogInfo("Check the user.");
            Task<System.Security.Claims.ClaimsIdentity> identity =
                _authorizationManager.GetClaimsIdentity(credentials.UserLogin, credentials.Password);
            if (identity.Result == null)
            {
                _log.LogError("Invalid username or password.");
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.", ModelState));
            }
            else
            {
                _log.LogInfo("Set an access token.");
                var jwt = GetToken(credentials.UserLogin);

                _log.LogInfo("Successful authorize.");
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
            return new OkObjectResult("Log out success.");
        }

        /// <summary>
        /// Get new token
        /// </summary>
        /// <param name="username">login</param>
        /// <returns>token</returns>
        private async Task<string> GetToken(string username)
        {
            int userId = await _appDbContext.FindByLogin(username);

            var identity = await _authorizationManager.GetIdentity(username, userId);
            if (identity == null)
            {
                return null;
            }

            _log.LogInfo("Set token options.");
            var now = DateTime.Now;

            var jwt = new JwtSecurityToken(
                    issuer: AuthorizationOptions.Issuer,
                    audience: AuthorizationOptions.Audience,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthorizationOptions.Lifetime)),
                    signingCredentials: new SigningCredentials(AuthorizationOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            _log.LogInfo("Set session options.");
            Sessions start = new Sessions()
            {
                Token = encodedJwt,
                UserId = userId,
                ExpiredDate = now.Add(TimeSpan.FromMinutes(AuthorizationOptions.Lifetime))
            };

            _log.LogInfo("Check for previous session.");
            if (await _appDbContext.IsExistPreviousSession(userId))
            {
                _log.LogInfo("The session was founded. I`ll delete it.");
                await _appDbContext.DeleteSessions(userId);
                _log.LogInfo("Success delete.");
            }

            _log.LogInfo("Add session");
            await _appDbContext.AddSession(start);
            _log.LogInfo("Session was add.");
            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            _log.LogInfo("Return session's token");
            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
            return encodedJwt;
        }
    }
}