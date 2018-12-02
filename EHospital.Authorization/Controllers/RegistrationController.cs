namespace EHospital.Authorization.WebAPI
{
    using System;
    using System.Threading.Tasks;
    using EHospital.Authorization.Data;
    using EHospital.Authorization.Model;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Controller for registration
    /// </summary>
    [Route("api/[controller]")]
    public class RegistrationController : Controller
    {
        private readonly ILogging Log;

        private readonly IDataProvider _appDbContext;

        public RegistrationController(IDataProvider appDbContext, ILogging logger)
        {
            _appDbContext = appDbContext;
            Log = logger;
        }

        /// <summary>
        /// Set roles defenition by id
        /// </summary>
        /// <param name="role">role</param>
        /// <returns>defenition</returns>
        public static string RolesDefenition(UsersRoles role)
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
                default:
                    break;
            }

            return definition;
        }

        /// <summary>
        /// Registers new user
        /// </summary>
        /// <param name="userDatas">new user</param>
        /// <param name="userSecrets">new password</param>
        /// <returns>succes</returns>
        [HttpPost]
        public async Task<IActionResult> Registration([FromBody] UsersData userDatas, [FromBody] Secrets userSecrets)
        {
            Log.LogInfo("Get datas for registration.");
            if (!this.ModelState.IsValid)
            {
                Log.LogError("Incorrect input.");
                return this.BadRequest(this.ModelState);
            }

            Log.LogInfo("Chek is password safe.");
            try
            {
                if (PasswordManager.ValidatePassword(userSecrets.Password))
                {
                    Log.LogInfo("Safety of password is good.");

                    Log.LogInfo("Chek is it a new user.");
                    if (!await _appDbContext.IsUserExist(userDatas.Email))
                    {
                        using (var context = new UsersDataContext())
                        {
                            using (var transaction = context.Database.BeginTransaction())
                            {
                                try
                                {
                                    Log.LogInfo("Add default role.");
                                    await _appDbContext.AddRoles(new Roles { Id = (int)UsersRoles.NoRole, Title = UsersRoles.NoRole.ToString() });

                                    Log.LogInfo("Add login.");
                                    await _appDbContext.AddLogin(new Logins { Id = 0, Login = userDatas.Email });

                                    Log.LogInfo("Add user's data");
                                    await _appDbContext.AddUserData(new UsersData
                                    {
                                        FirstName = userDatas.FirstName,
                                        LastName = userDatas.LastName,
                                        BirthDate = userDatas.BirthDate,
                                        PhoneNumber = userDatas.PhoneNumber,
                                        Country = userDatas.Country,
                                        City = userDatas.City,
                                        Adress = userDatas.Adress,
                                        Gender = userDatas.Gender,
                                        Email = userDatas.Email
                                    });

                                    Log.LogInfo("Add password.");
                                    await _appDbContext.AddSecrets(new Secrets { Password = userSecrets.Password });

                                    Log.LogInfo("Save changes.");
                                    await _appDbContext.Save();

                                    transaction.Commit();
                                }
                                catch (Exception ex)
                                {
                                    Log.LogError("Account is not created." + ex.Message);
                                    return new BadRequestObjectResult("Creation of account was failed." + ex.Message);
                                }
                            }
                        }
                    }
                    else
                    {
                        Log.LogError("Account is not created.");
                        return new BadRequestObjectResult("Creation of account was failed.");
                    }

                    Log.LogInfo("Account created");
                    return new OkObjectResult("Account created");
                }
                else
                {
                    Log.LogError("Account is not created.");
                    return new BadRequestObjectResult("Creation of account was failed.");
                }
            }
            catch (ArgumentException ex)
            {
                Log.LogError("Account is not created." + ex.Message);
                return new BadRequestObjectResult("Creation of account was failed." + ex.Message);
            }
        }
    }
}