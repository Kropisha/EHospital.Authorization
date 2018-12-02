namespace EHospital.Authorization.WebAPI
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Controller for connection with other services
    /// </summary>
    [Route("api/[controller]")]
    public class AuthentificationController : Controller
    {
        IDataProvider dataProvider;

        public AuthentificationController(IDataProvider data)
        {
            dataProvider = data;
        }

        /// <summary>
        /// Get role by token
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>role</returns>
        [HttpGet("Token")]
        public Task<string> GetRoleByToken([FromHeader]string token)
        {
            return this.dataProvider.GetRoleByToken(token);
        }

       // public string GetToken() => dataProvider.Token;
    }
}