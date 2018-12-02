namespace EHospital.Authorization.WebAPI
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    public class AuthentificationController : Controller
    {
        IDataProvider dataProvider;

        public AuthentificationController(IDataProvider data)
        {
            dataProvider = data;
        }

        [HttpGet("Token")]
        public Task<string> GetRoleByToken([FromHeader]string token)
        {
            return this.dataProvider.GetRoleByToken(token);
        }

       // public string GetToken() => dataProvider.Token;
    }
}