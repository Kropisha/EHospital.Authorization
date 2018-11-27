namespace EHospital.Authorization.WebAPI
{
    using System.Threading.Tasks;
    using AutoMapper;
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

            if (role == UsersRoles.NoRole)
            {
                definition = "No role";
            }

            if (role == UsersRoles.Admin)
            {
                definition = "Administrator";
            }

            if (role == UsersRoles.Doctor)
            {
                definition = "Doctor";
            }

            if (role == UsersRoles.Nurse)
            {
                definition = "Nurse";
            }

            return definition;
        }

       // [Authorize(Roles = _roleManager.Roles[])]
        // POST api/accounts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UsersData userDatas, Secrets userSecrets)
        {
            Log.Info("Get datas for registration.");
            if (!this.ModelState.IsValid)
            {
                Log.Error("Incorrect input.");
                return this.BadRequest(this.ModelState);
            }

            Log.Info("Chek is password safe.");
            if (PasswordManager.ValidatePassword(userSecrets.Password))
            {
                Log.Info("Safety of password is good.");

                Log.Info("Chek is it a new user.");
                if (! await _appDbContext.IsUserExist(userDatas.Email))
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
                    await _appDbContext.AddSecrets(new Secrets { Password = userSecrets.Password});

                    Log.Info("Save changes.");
                    await _appDbContext.Save();
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
    }
}