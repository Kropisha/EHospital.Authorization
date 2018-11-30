namespace EHospital.Authorization.WebAPI
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using EHospital.Authorization.Data;
    using EHospital.Authorization.Model;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    public class RegistrationController : Controller
    {
        private static readonly log4net.ILog Log = log4net.LogManager
                                                          .GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDataProvider _appDbContext;

        public RegistrationController(IDataProvider appDbContext)
        {
            _appDbContext = appDbContext;
        }

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

        [HttpPost]
        public async Task<IActionResult> Registration([FromBody] UsersData userDatas, [FromBody] Secrets userSecrets)
        {
            Log.Info("Get datas for registration.");
            if (!this.ModelState.IsValid)
            {
                Log.Error("Incorrect input.");
                return this.BadRequest(this.ModelState);
            }

            Log.Info("Chek is password safe.");
            try
            {
                if (PasswordManager.ValidatePassword(userSecrets.Password))
                {
                    Log.Info("Safety of password is good.");

                    Log.Info("Chek is it a new user.");
                    if (!await _appDbContext.IsUserExist(userDatas.Email))
                    {
                        using (var context = new UsersDataContext())
                        {
                            using (var transaction = context.Database.BeginTransaction())
                            {
                                try
                                {
                                    Log.Info("Add default role.");
                                    await _appDbContext.AddRoles(new Roles { Id = (int)UsersRoles.NoRole, Title = UsersRoles.NoRole.ToString() });

                                    Log.Info("Add login.");
                                    await _appDbContext.AddLogin(new Logins { Id = 0, Login = userDatas.Email });

                                    Log.Info("Add user's data");
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

                                    Log.Info("Add password.");
                                    await _appDbContext.AddSecrets(new Secrets { Password = userSecrets.Password });

                                    Log.Info("Save changes.");
                                    await _appDbContext.Save();

                                    transaction.Commit();
                                }
                                catch (Exception ex)
                                {
                                    Log.Error("Account is not created." + ex.Message);
                                    return new BadRequestObjectResult("Creation of account was failed." + ex.Message);
                                }
                            }
                        }
                    }
                    else
                    {
                        Log.Error("Account is not created.");
                        return new BadRequestObjectResult("Creation of account was failed.");
                    }

                    Log.Info("Account created");
                    return new OkObjectResult("Account created");
                }
                else
                {
                    Log.Error("Account is not created.");
                    return new BadRequestObjectResult("Creation of account was failed.");
                }
            }
            catch (ArgumentException ex)
            {
                Log.Error("Account is not created." + ex.Message);
                return new BadRequestObjectResult("Creation of account was failed." + ex.Message);
            }
        }
    }
}