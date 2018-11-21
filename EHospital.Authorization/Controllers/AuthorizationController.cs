﻿namespace EHospital.Authorization.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
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

        public AuthorizationController(IDataProvider data)
        {
            _appDbContext = data;
        }

        // POST api/auth/login
        [HttpPost("login")]
        public IActionResult Post([FromBody]CredentialsViewModel credentials)
        {
            Log.Info("Set credentials for authorization.");
            if (!this.ModelState.IsValid)
            {
                Log.Error("Incorrect format of input.");
                return this.BadRequest(this.ModelState);
            }

            Log.Info("Check the user.");
            var identity = this.GetClaimsIdentity(credentials.UserLogin, credentials.Password);
            if (identity.Result == null)
            {
                Log.Error("Invalid username or password.");
                return this.BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.", this.ModelState));
            }
            else
            {
                Log.Info("Set an access token.");
                var jwt = this.GetToken(credentials.UserLogin, credentials.Password);

                Log.Info("Successful authorize.");
                return new OkObjectResult(jwt.Result);
            }
        }

        private async Task<string> GetToken(string username, string password)
        {
            // var username = Request.Form["username"];
            // var password = Request.Form["password"];

            int userId = _appDbContext.GetUserPassword(password);

            var identity = this.GetIdentity(username, userId);
            if (identity == null)
            {
                this.Response.StatusCode = 400;
                await this.Response.WriteAsync("Invalid username or password.");
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

        private Task<ClaimsIdentity> GetClaimsIdentity(string userLogin, string password)
        {
            if (string.IsNullOrEmpty(userLogin) || string.IsNullOrEmpty(password))
            {
                return Task.FromResult<ClaimsIdentity>(null);
            }

            Log.Info("Get the user to verifty.");
            var userToVerify = _appDbContext.FindByLogin(userLogin);

            if (userToVerify == 0)
            {
                return Task.FromResult<ClaimsIdentity>(null);
            }

            Log.Info("Check the credentials.");
            if (_appDbContext.CheckPassword(password, userToVerify))
            {
                return Task.FromResult(this.GetIdentity(userLogin, userToVerify));
            }

            Log.Error("Credentials are invalid, or account doesn't exist.");
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
            new ClaimsIdentity(
                claims,
                "Token",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }
    }
}