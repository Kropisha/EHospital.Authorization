namespace EHospital.Authorization.WebAPI
{
    using System.Threading.Tasks;
    using EHospital.Authorization.Model;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    public class ConfirmationController : Controller
    {
        private static readonly log4net.ILog Log = log4net.LogManager
                                                          .GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDataProvider _appDbContext;

        public ConfirmationController(IDataProvider data)
        {
            _appDbContext = data;
        }

        [HttpPut("Role")]
        public async Task<IActionResult> ChangeRole([FromBody]Roles roles)
        {
            Log.Info("Get new role.");
            if (!this.ModelState.IsValid)
            {
                Log.Error("Incorrect input.");
                return this.BadRequest(this.ModelState);
            }

            if (await _appDbContext.ChangeRole(roles) != null)
            {
                Log.Error("Incorrect role.");
                return this.BadRequest(Errors.AddErrorToModelState("roles_failure", "Invalid role.", this.ModelState));
            }
            else
            {
                Log.Info("Changing role.");
                await _appDbContext.ChangeRole(roles);
                return this.Ok();
            }
        }

        [HttpPut("User")]
        public async Task<IActionResult> ChangeUserData([FromBody]UsersData usersData)
        {
            Log.Info("Get user's data.");
            if (!this.ModelState.IsValid)
            {
                Log.Error("Incorrect input.");
                return this.BadRequest(this.ModelState);
            }

            if (await _appDbContext.ChangeUserData(usersData) != null)
            {
                Log.Error("Invalid input, null user.");
                return this.BadRequest(Errors.AddErrorToModelState("change_failure", "Invalid input.", ModelState));
            }
            else
            {
                Log.Info("Change datas.");
                return this.Ok();
            }
        }

        [HttpPut("Password")]
        public async Task<IActionResult> ChangePassword([FromBody]Secrets secrets)
        {
            Log.Info("Get new password.");
            if (!this.ModelState.IsValid)
            {
                Log.Error("Incorrect input.");
                return this.BadRequest(this.ModelState);
            }

            if (await _appDbContext.ChangePassword(secrets) == null)
            {
                Log.Error("Invalid input, null password.");
                return this.BadRequest(Errors.AddErrorToModelState("change_failure", "Invalid input.", this.ModelState));
            }
            else
            {
                Log.Info("Change password.");
                return this.Ok();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            Log.Info("Get user for delete.");
            if (!this.ModelState.IsValid)
            {
                Log.Error("Incorrect input");
                return this.BadRequest(this.ModelState);
            }
            else
            {
                Log.Info("Deleting user.");
                await _appDbContext.DeleteUser(userId);
                Log.Info("Success.");
                return this.Ok();
            }
        }
    }
}