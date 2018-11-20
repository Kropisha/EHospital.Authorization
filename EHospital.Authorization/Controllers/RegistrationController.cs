namespace EHospital.Authorization.WebApi
{
    using AutoMapper;
    using EHospital.Authorization.Models;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    public class RegistrationController : Controller
    {
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
        public IActionResult Post([FromBody] UsersDataContext model) // UsersData userDatas, Logins userLogins, Secrets userSecrets, Roles roles) 
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var userDatas = _mapper.Map<UsersData>(model);
            var userLogins = _mapper.Map<Logins>(model);
            var userRoles = _mapper.Map<Roles>(model);
            var userSecrets = _mapper.Map<Secrets>(model);

            userRoles.RoleId = (int)UsersRoles.NoRole;

            _appDbContext.AddRoles(new Roles { RoleId = userRoles.RoleId, Title = UsersRoles.NoRole.ToString() });

            _appDbContext.AddLogin(new Logins { Login = userLogins.Login, RoleId = userLogins.RoleId });

            _appDbContext.AddUserData(new UsersData
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

            _appDbContext.AddSecrets(new Secrets { UserId = userSecrets.UserId, Password = userSecrets.Password });

             _appDbContext.Save(); // await

            return new OkObjectResult("Account created");
        }
    }
}