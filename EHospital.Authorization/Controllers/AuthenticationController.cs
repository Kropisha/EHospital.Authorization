using System.Threading.Tasks;
using EHospital.Authorization.Data.Data;
using Microsoft.AspNetCore.Mvc;

namespace EHospital.Authorization.WebAPI.Controllers
{
    /// <summary>
    /// Controller for connection with other services
    /// </summary>
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        readonly IDataProvider _dataProvider;

        public AuthenticationController(IDataProvider data)
        {
            _dataProvider = data;
        }

        /// <summary>
        /// Get role by token
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>role</returns>
        [HttpGet("Token")]
        public Task<string> GetRoleByToken([FromHeader]string token)
        {
            return _dataProvider.GetRoleByToken(token);
        }

       // public string GetToken() => dataProvider.Token;
    }
}