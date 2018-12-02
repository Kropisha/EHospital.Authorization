namespace EHospital.Authorization.WebAPI
{
    using System.Threading.Tasks;
    using EHospital.Authorization.Data;
    using EHospital.Authorization.Model;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Controller for confirmation and managing
    /// </summary>
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

        /// <summary>
        /// Change role for user
        /// </summary>
        /// <param name="roles">new role</param>
        /// <returns>ok</returns>
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

        /// <summary>
        /// Change users data
        /// </summary>
        /// <param name="usersData">new data</param>
        /// <returns>ok</returns>
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

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="secrets">new password</param>
        /// <returns>ok</returns>
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

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="userId">user's id</param>
        /// <returns>ok</returns>
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