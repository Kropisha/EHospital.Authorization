namespace eHospital.Authorization.Controllers
{
    using eHospital.Authorization.Models;
    using Microsoft.AspNetCore.Mvc;

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
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (_appDbContext.ChangeRole(roles) != null)
            {
                return this.BadRequest(Errors.AddErrorToModelState("roles_failure", "Invalid role.", this.ModelState));
            }
            else
            {
                _appDbContext.ChangeRole(roles);
                return this.Ok();
            }
        }

        [HttpPut("user")]
        public ActionResult<UsersData> ChangeUserData([FromBody]UsersData usersData)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            if (_appDbContext.ChangeUserData(usersData) != null)
            {
                return this.BadRequest(Errors.AddErrorToModelState("change_failure", "Invalid input.", ModelState));
            }
            else
            {
                return this.Ok(_appDbContext.ChangeUserData(usersData));
            }
        }

        [HttpPut("password")]
        public ActionResult<Secrets> ChangePassword([FromBody]Secrets secrets)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (_appDbContext.ChangePassword(secrets) == null)
            {
                return this.BadRequest(Errors.AddErrorToModelState("change_failure", "Invalid input.", ModelState));
            }
            else
            {
                return this.Ok(_appDbContext.ChangePassword(secrets));
            }
        }

        [HttpDelete]
        public IActionResult DeleteUser(UsersData user)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            else
            {
                _appDbContext.DeleteUser(user.UserId);
                return this.Ok();
            }
        }
    }
}