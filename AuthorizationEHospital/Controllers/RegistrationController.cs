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
    public class RegistrationController : Controller
    {
        
        private readonly IDataProvider _appDbContext;
       // private readonly UserManager<UsersData> _userManager;
        private readonly IMapper _mapper;
      //  RoleManager<Roles> _roleManager;

        public RegistrationController( IMapper mapper, IDataProvider appDbContext)
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
        public IActionResult Post([FromBody] UsersDataContext model)//UsersData userDatas, Logins userLogins, Secrets userSecrets, Roles roles) //UsersDataContext model
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userDatas = _mapper.Map<UsersData>(model);
            var userLogins = _mapper.Map<Logins>(model);
            var userRoles = _mapper.Map<Roles>(model);
            var userSecrets = _mapper.Map<Secrets>(model);

            userRoles.RoleId = (int)UsersRoles.noRole;
            
            _appDbContext.AddRoles(new Roles { RoleId = userRoles.RoleId, Title = UsersRoles.noRole.ToString() });

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
             
             _appDbContext.Save(); //await

            return new OkObjectResult("Account created");
        }
    }
}