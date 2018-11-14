using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eHospital.Authorization.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eHospital.Authorization.Controllers
{
    [Route("api/[controller]")]
    public class ConfirmationController : Controller
    {
        private readonly IDataProvider _appDbContext;

        public ConfirmationController(IDataProvider data)
        {
            _appDbContext = data;
        }

        [HttpPut("role")]
        public ActionResult<Roles> ChangeRole([FromBody]Roles roles)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_appDbContext.ChangeRole(roles) != null)
            {
                return BadRequest(Errors.AddErrorToModelState("roles_failure", "Invalid role.", ModelState));
            }
            else
            {
                _appDbContext.ChangeRole(roles);
                return Ok();
            }
        }


        [HttpPut("user")]
        public ActionResult<UsersData> ChangeUserData([FromBody]UsersData usersData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_appDbContext.ChangeUserData(usersData) != null)
            {
                return BadRequest(Errors.AddErrorToModelState("change_failure", "Invalid input.", ModelState));
            }
            else
            {
                return Ok(_appDbContext.ChangeUserData(usersData));
            }
        }

        [HttpPut("password")]
        public ActionResult<Secrets> ChangePassword([FromBody]Secrets secrets)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_appDbContext.ChangePassword(secrets) == null)
            {
                return BadRequest(Errors.AddErrorToModelState("change_failure", "Invalid input.", ModelState));
            }
            else
            {
                return Ok(_appDbContext.ChangePassword(secrets));
            }
        }

        [HttpDelete]
        public IActionResult DeleteUser(UsersData user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                _appDbContext.DeleteUser(user.UserId);
                return Ok();
            }
        }
    }
}