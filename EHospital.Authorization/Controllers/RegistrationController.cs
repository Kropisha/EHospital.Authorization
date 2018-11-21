namespace EHospital.Authorization.WebApi
{
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
        private readonly IMapper _mapper;

        public RegistrationController( IMapper mapper, IDataProvider appDbContext)
        {
            _mapper = mapper;
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
        public async Task<IActionResult> Post([FromBody] UsersDataContext model) // UsersData userDatas, Logins userLogins, Secrets userSecrets, Roles roles) 
        {
            Log.Info("Get datas for registration.");
            if (!this.ModelState.IsValid)
            {
                Log.Error("Incorrect input.");
                return this.BadRequest(this.ModelState);
            }

            var userDatas = _mapper.Map<UsersData>(model);
            var userLogins = _mapper.Map<Logins>(model);
            var userRoles = _mapper.Map<Roles>(model);
            var userSecrets = _mapper.Map<Secrets>(model);

            Log.Info("Set default users role.");
            userRoles.RoleId = (int)UsersRoles.NoRole;

            Log.Info("Add role.");
            await _appDbContext.AddRoles(new Roles { RoleId = userRoles.RoleId, Title = UsersRoles.NoRole.ToString() });

            Log.Info("Add login.");
            await _appDbContext.AddLogin(new Logins { Login = userLogins.Login, RoleId = userLogins.RoleId });

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
            await _appDbContext.AddSecrets(new Secrets { UserId = userSecrets.UserId, Password = userSecrets.Password });

            Log.Info("Save changes.");
            await _appDbContext.Save();

            Log.Info("Account created");
            return new OkObjectResult("Account created");
        }
    }
}