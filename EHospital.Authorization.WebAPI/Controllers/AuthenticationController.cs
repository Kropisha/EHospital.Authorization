using EHospital.Authorization.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EHospital.Authorization.WebAPI.Controllers
{
    /// <summary>
    /// Controller for connection with other services
    /// </summary>
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly IUserDataProvider _userDataProvider;

        public AuthenticationController(IUserDataProvider userData)
        {
            _userDataProvider = userData;
        }

        /// <summary>
        /// Get role by token
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>role</returns>
        [HttpGet("Token")]
        public Task<string> GetRoleByToken()
        {
            var headers = Request.Headers;
            var token = "";
            foreach (var head in headers)
            {
                if (head.Key == "Authorization")
                {
                    token = head.Value;
                    break;
                }
            }

            return _userDataProvider.GetRoleByToken(token);
        }

        /// <summary>
        /// Get user id
        /// </summary>
        /// <returns>id</returns>
        [HttpGet("Id")]
        public Task<int> GetId()
        {
            var headers = Request.Headers;
            var token = "";
            foreach (var head in headers)
            {
                if (head.Key == "Authorization")
                {
                    token = head.Value;
                    break;
                }
            }

            return _userDataProvider.GetId(token);
        }

        /// <summary>
        /// Get information about authorized user by token
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>model with information</returns>
        [HttpGet]
        public async Task<IActionResult> GetAuthInfo(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest("Token cannot be empty.");
                }

                var tokeAuthDataModel = await _userDataProvider.GetUserAuthInfo(token);
                if (tokeAuthDataModel == null)
                {
                    return null;
                }

                var result = new Shared.Authorization.Models.UserAuthModel
                {
                    Id = tokeAuthDataModel.Id,
                    Role = tokeAuthDataModel.Role,
                    RoleId = tokeAuthDataModel.RoleId,
                    UserTokenExpirationDate = tokeAuthDataModel.UserTokenExpirationDate
                };

                return Ok(result);
            }
            catch
            {
                return null;
            }
        }
    }
}