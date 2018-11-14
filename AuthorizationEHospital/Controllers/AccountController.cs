using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eHospital.Authorization.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using eHospital.Authorization.Service;
using System.Security.Claims;
using Newtonsoft.Json;

namespace eHospital.Authorization.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        
        private readonly IDataProvider _appDbContext;
       // private readonly UserManager<UsersData> _userManager;
        private readonly IMapper _mapper;
      //  RoleManager<Roles> _roleManager;

        public AccountController( IMapper mapper, IDataProvider appDbContext)
        {
          //  _userManager = userManager;
            _mapper = mapper;
            _appDbContext = appDbContext;
         //   _roleManager = manager;
        }

        public static string RolesDefenition(UsersRoles role)
        {
            string definition = null;

            if (role == UsersRoles.noRole)
            {
                definition = "No role";
            }
            if (role == UsersRoles.admin)
            {
                definition = "Administrator";
            }
            if (role == UsersRoles.doctor)
            {
                definition = "Doctor";
            }
            if (role == UsersRoles.nurse)
            {
                definition = "Nurse";
            }
            return definition;
        }

        //public static string currentRole = RolesDefenition(UsersRoles.admin);

       // [Authorize(Roles = _roleManager.Roles[])]
        // POST api/accounts
        [HttpPost]
        public IActionResult Post([FromBody]UsersDataContext model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdentity = _mapper.Map<UsersData>(model);
            var userLogins = _mapper.Map<Logins>(model);
            var userRoles = _mapper.Map<Roles>(model);
            var userSecrets = _mapper.Map<Secrets>(model);


          //  var result = await _userManager.CreateAsync(userIdentity, userSecrets.Password); //Password

          //  if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

             _appDbContext.AddRoles(new Roles { RoleId = (int)UsersRoles.noRole, Title = UsersRoles.noRole.ToString() });
             _appDbContext.AddLogin(new Logins { LoginId = userLogins.LoginId, Login = userLogins.Login, RoleId = userLogins.RoleId });
             _appDbContext.AddSecrets(new Secrets { UserId = userSecrets.UserId, Password = userSecrets.Password });
             _appDbContext.AddUserData(new UsersData { UserId = userIdentity.UserId, FirstName = userIdentity.FirstName,
                LastName = userIdentity.LastName, BirthDate = userIdentity.BirthDate, PhoneNumber = userIdentity.PhoneNumber,
                Country = userIdentity.Country, City = userIdentity.City, Address = userIdentity.Address, Gender = userIdentity.Gender,
                Email = userIdentity.Email });
             _appDbContext.Save(); //await

            return new OkObjectResult("Account created");
        }

        
        //public List<UsersData> Users()
        //{
        //    List<UsersData> users = new List<UsersData>();
        //    foreach (var login in _appDbContext.Logins)
        //    {
        //        foreach (var user in _appDbContext.UsersData)
        //        {
        //            if (login.Login == user.Email)
        //            {
        //                user.Id = login.Login;
        //                users.Add(user);
        //            }
        //        }

        //    }
        //    return users;
        //}

        //public List<UsersData> Create()
        //{
        //    return Users();// _roleManager.Roles.ToList());
        //}
    }
}