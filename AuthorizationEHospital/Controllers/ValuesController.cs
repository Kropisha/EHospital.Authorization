using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eHospital.Authorization.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        [Authorize]
        [Route("getlogin")]
        [HttpGet]
        public IActionResult GetLogin()
        {
            return Ok($"Ваш логин: {User.Identity.Name}");
        }

        [Authorize(Roles = "admin")]
        [Route("getrole")]
        [HttpGet]
        public IActionResult GetRole()
        {
            return Ok("Ваша роль: администратор");
        }

        [HttpGet]
        public string Create()
        {
            return "Finally work";// _roleManager.Roles.ToList());
        }
    }
}