using EHospital.Authorization.BusinessLogic.EmailAction;
using EHospital.Authorization.Data;
using EHospital.Authorization.Models;
using EHospital.Authorization.WebAPI.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace EHospital.Authorization.WebAPI.Controllers
{
    /// <summary>
    /// Controller for confirmation and managing
    /// </summary>
    [Route("api/[controller]")]
    public class ConfirmationController : Controller
    {
        private readonly ILogging _log;
        private readonly IUserDataProvider _appDbContext;
        private EmailSender emailSender = new EmailSender();

        public ConfirmationController(IUserDataProvider userData, ILogging logger)
        {
            _appDbContext = userData;
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
            string role = await _appDbContext.GetRoleByToken(Request.Headers["Authorization"]);

            if (role.ToLower() == "admin")
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

                _log.LogInfo("Changing role.");
                await _appDbContext.ChangeRole(roles);

                string roleChangeText;
                using (StreamReader streamreader = new StreamReader(@"..\EHospital.Authorization.WebAPI\Letters\roleChange.txt"))
                {
                    roleChangeText = streamreader.ReadToEnd();
                }

                string email = await _appDbContext.GetEmail(roles);
                await emailSender.SendEmail(email, "Free access", roleChangeText);
                return Ok();
            }

            return BadRequest(role);
        }

        /// <summary>
        /// Change users userData
        /// </summary>
        /// <param name="usersData">new userData</param>
        /// <returns>ok</returns>
        [HttpPut("User")]
        public async Task<IActionResult> ChangeUserData([FromBody]UsersData usersData)
        {
            _log.LogInfo("Get user's userData.");
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

            _log.LogInfo("Change userData.");
            return Ok();
        }

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="secrets">new password</param>
        /// <returns>ok</returns>
        [HttpPut("Password")]
        public async Task<IActionResult> ChangePassword([FromBody]Secrets secrets)
        {
            //TODO: Here is a bug that any user even without token can change password for any one.
            //We should check that if user change password for himself its ok, or admin can change password.

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

            _log.LogInfo("Change password.");
            return Ok();
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="userId">user's id</param>
        /// <returns>ok</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            string role = await _appDbContext.GetRoleByToken(Request.Headers["Authorization"]);

            if (role.ToLower() == "admin")
            {
                _log.LogInfo("Get user for delete.");
                if (!ModelState.IsValid)
                {
                    _log.LogError("Incorrect input");
                    return BadRequest(ModelState);
                }

                _log.LogInfo("Deleting user.");
                await _appDbContext.DeleteUser(userId);
                _log.LogInfo("Success.");
                return Ok();
            }

            return BadRequest(role);
        }
    }
}