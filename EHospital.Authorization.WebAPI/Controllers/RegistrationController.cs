using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using EHospital.Authorization.BusinessLogic.Credentials;
using EHospital.Authorization.BusinessLogic.Enums;
using EHospital.Authorization.Data.Data;
using EHospital.Authorization.Model.Models;
using Microsoft.AspNetCore.Mvc;

namespace EHospital.Authorization.WebAPI.Controllers
{
    /// <summary>
    /// Controller for registration
    /// </summary>
    [Route("api/[controller]")]
    public class RegistrationController : Controller
    {
        private readonly ILogging _log;

        private readonly IDataProvider _appDbContext;

        public RegistrationController(IDataProvider appDbContext, ILogging logger)
        {
            _appDbContext = appDbContext;
            _log = logger;
        }

        /// <summary>
        /// Set roles definition by id
        /// </summary>
        /// <param name="role">role</param>
        /// <returns>definition</returns>
        public static string RolesDefinition(UsersRoles role)
        {
            string definition = null;
            switch (role)
            {
                case UsersRoles.NoRole:
                    definition = "No role";
                    break;
                case UsersRoles.Admin:
                    definition = "Administrator";
                    break;
                case UsersRoles.Doctor:
                    definition = "Doctor";
                    break;
                case UsersRoles.Nurse:
                    definition = "Nurse";
                    break;
            }

            return definition;
        }

        /// <summary>
        /// Registers new user
        /// </summary>
        /// <param name="userData">new user</param>
        /// <param name="userSecrets">new password</param>
        /// <returns>success</returns>
        [HttpPost]
        public async Task<IActionResult> Registration(UsersData userData, Secrets userSecrets)
        {
            _log.LogInfo("Get data for registration.");
            if (!ModelState.IsValid)
            {
                _log.LogError("Incorrect input.");
                return BadRequest(ModelState);
            }

            _log.LogInfo("Check is password safe.");
            try
            {
                if (PasswordManager.ValidatePassword(userSecrets.Password))
                {
                    _log.LogInfo("Safety of password is good.");

                    _log.LogInfo("Check is it a new user.");
                    if (!await _appDbContext.IsUserExist(userData.Email))
                    {
                        using (SqlConnection connection = new SqlConnection("Data Source=JULIKROP;Initial Catalog=Schema;Integrated Security=True"))
                        {
                            connection.Open();
                            using (var transaction = connection.BeginTransaction())
                            {
                                try
                                {
                                    _log.LogInfo("Add default role.");
                                    await _appDbContext.AddRoles(new Roles { Id = (int)UsersRoles.NoRole, Title = UsersRoles.NoRole.ToString() });

                                    _log.LogInfo("Add login.");
                                    await _appDbContext.AddLogin(new Logins {  Login = userData.Email});

                                    _log.LogInfo("Add user's data");
                                    await _appDbContext.AddUserData(new UsersData
                                    {
                                        FirstName = userData.FirstName,
                                        LastName = userData.LastName,
                                        BirthDate = userData.BirthDate,
                                        PhoneNumber = userData.PhoneNumber,
                                        Country = userData.Country,
                                        City = userData.City,
                                        Adress = userData.Adress,
                                        Gender = userData.Gender,
                                        Email = userData.Email
                                    });

                                    _log.LogInfo("Add password.");
                                    await _appDbContext.AddSecrets(new Secrets { Password = userSecrets.Password });

                                    transaction.Commit();
                                    connection.Close();
                                }
                                catch (Exception ex)
                                {
                                    _log.LogError("Account is not created." + ex.Message);
                                    return new BadRequestObjectResult("Creation of account was failed." + ex.Message);
                                }
                    }
                }
            }
                    else
                    {
                        _log.LogError("Account is not created.");
                        return new BadRequestObjectResult("Creation of account was failed.");
                    }

                    _log.LogInfo("Account created");
                    return new OkObjectResult("Account created");
                }
                else
                {
                    _log.LogError("Account is not created.");
                    return new BadRequestObjectResult("Creation of account was failed.");
                }
            }
            catch (ArgumentException ex)
            {
                _log.LogError("Account is not created." + ex.Message);
                return new BadRequestObjectResult("Creation of account was failed." + ex.Message);
            }
        }
    }
}