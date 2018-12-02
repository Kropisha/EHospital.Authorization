namespace EHospital.Authorization.WebAPI
{
    using System.Threading.Tasks;
    using EHospital.Authorization.Data;
    using EHospital.Authorization.Model;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    public class ConfirmationController : Controller
    {
        private readonly ILogging Log;

        private readonly IDataProvider _appDbContext;

        public ConfirmationController(IDataProvider data, ILogging logger)
        {
            _appDbContext = data;
            Log = logger;
        }

        [HttpPut("Role")]
        public async Task<IActionResult> ChangeRole([FromBody]Roles roles)
        {
            Log.LogInfo("Get new role.");
            if (!this.ModelState.IsValid)
            {
                Log.LogError("Incorrect input.");
                return this.BadRequest(this.ModelState);
            }

            if (await _appDbContext.ChangeRole(roles) != null)
            {
                Log.LogError("Incorrect role.");
                return this.BadRequest(Errors.AddErrorToModelState("roles_failure", "Invalid role.", this.ModelState));
            }
            else
            {
                Log.LogInfo("Changing role.");
                await _appDbContext.ChangeRole(roles);
                return this.Ok();
            }
        }

        [HttpPut("User")]
        public async Task<IActionResult> ChangeUserData([FromBody]UsersData usersData)
        {
            Log.LogInfo("Get user's data.");
            if (!this.ModelState.IsValid)
            {
                Log.LogError("Incorrect input.");
                return this.BadRequest(this.ModelState);
            }

            if (await _appDbContext.ChangeUserData(usersData) != null)
            {
                Log.LogError("Invalid input, null user.");
                return this.BadRequest(Errors.AddErrorToModelState("change_failure", "Invalid input.", ModelState));
            }
            else
            {
                Log.LogInfo("Change datas.");
                return this.Ok();
            }
        }

        [HttpPut("Password")]
        public async Task<IActionResult> ChangePassword([FromBody]Secrets secrets)
        {
            Log.LogInfo("Get new password.");
            if (!this.ModelState.IsValid)
            {
                Log.LogError("Incorrect input.");
                return this.BadRequest(this.ModelState);
            }

            if (await _appDbContext.ChangePassword(secrets) == null)
            {
                Log.LogError("Invalid input, null password.");
                return this.BadRequest(Errors.AddErrorToModelState("change_failure", "Invalid input.", this.ModelState));
            }
            else
            {
                Log.LogInfo("Change password.");
                return this.Ok();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            Log.LogInfo("Get user for delete.");
            if (!this.ModelState.IsValid)
            {
                Log.LogError("Incorrect input");
                return this.BadRequest(this.ModelState);
            }
            else
            {
                Log.LogInfo("Deleting user.");
                await _appDbContext.DeleteUser(userId);
                Log.LogInfo("Success.");
                return this.Ok();
            }
        }
    }
}