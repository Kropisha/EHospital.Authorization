using System.Threading.Tasks;
using EHospital.Authorization.Data.Data;
using EHospital.Authorization.Model.Models;
using EHospital.Authorization.WebAPI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace EHospital.Authorization.WebAPI.Controllers
{
    /// <summary>
    /// Controller for confirmation and managing
    /// </summary>
    [Route("api/[controller]")]
    public class ConfirmationController : Controller
    {
        private readonly ILogging _log;

        private readonly IDataProvider _appDbContext;

        public ConfirmationController(IDataProvider data, ILogging logger)
        {
            _appDbContext = data;
            _log = logger;
        }

        /// <summary>
        /// Change role for user
        /// </summary>
        /// <param name="roles">new role</param>
        /// <returns>ok</returns>
        [HttpPut("Role")]
        public async Task<IActionResult> ChangeRole([FromBody]Roles roles)
        {
            _log.LogInfo("Get new role.");
            if (!ModelState.IsValid)
            {
                _log.LogError("Incorrect input.");
                return BadRequest(ModelState);
            }

            if (await _appDbContext.ChangeRole(roles) == null)
            {
                _log.LogError("Incorrect role.");
                return BadRequest(Errors.AddErrorToModelState("rolesFailure", "Invalid role.", ModelState)); 
            }
            else
            {
                _log.LogInfo("Changing role.");
                await _appDbContext.ChangeRole(roles);
                return Ok();
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
            _log.LogInfo("Get user's data.");
            if (!ModelState.IsValid)
            {
                _log.LogError("Incorrect input.");
                return BadRequest(ModelState);
            }

            if (await _appDbContext.ChangeUserData(usersData) == null)
            {
                _log.LogError("Invalid input, null user.");
                return BadRequest(Errors.AddErrorToModelState("changesFailure", "Invalid input.", ModelState));
            }
            else
            {
                _log.LogInfo("Change data.");
                return Ok();
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
            _log.LogInfo("Get new password.");
            if (!ModelState.IsValid)
            {
                _log.LogError("Incorrect input.");
                return BadRequest(ModelState);
            }

            if (await _appDbContext.ChangePassword(secrets) == null)
            {
                _log.LogError("Invalid input, null password.");
                return BadRequest(Errors.AddErrorToModelState("changesFailure", "Invalid input.", ModelState));
            }
            else
            {
                _log.LogInfo("Change password.");
                return Ok();
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
            _log.LogInfo("Get user for delete.");
            if (!ModelState.IsValid)
            {
                _log.LogError("Incorrect input");
                return BadRequest(ModelState);
            }
            else
            {
                _log.LogInfo("Deleting user.");
                await _appDbContext.DeleteUser(userId);
                _log.LogInfo("Success.");
                return Ok();
            }
        }
    }
}