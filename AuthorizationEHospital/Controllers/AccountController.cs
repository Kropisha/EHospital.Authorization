using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eHospital.Authorization.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace eHospital.Authorization.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UsersDataContext _appDbContext;
        private readonly UserManager<UsersData> _userManager;
        private readonly IMapper _mapper;

        public AccountController(UserManager<UsersData> userManager, IMapper mapper, UsersDataContext appDbContext)
        {
            _userManager = userManager;
            _mapper = mapper;
            _appDbContext = appDbContext;
        }

        // POST api/accounts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]UsersDataContext model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdentity = _mapper.Map<UsersData>(model);
            var userLogins = _mapper.Map<Logins>(model);
            var userRoles = _mapper.Map<Roles>(model);
            var userSecrets = _mapper.Map<Secrets>(model);


            var result = await _userManager.CreateAsync(userIdentity, userSecrets.Password); //Password

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            await _appDbContext.roles.AddAsync(new Roles { RoleId = (int)UsersRoles.noRole, Title = UsersRoles.noRole.ToString() });
            await _appDbContext.logins.AddAsync(new Logins { LoginId = userLogins.LoginId, Login = userLogins.Login, RoleId = userLogins.RoleId });
            await _appDbContext.secrets.AddAsync(new Secrets { UserId = userSecrets.UserId, Password = userSecrets.Password });
            await _appDbContext.usersDatas.AddAsync(new UsersData { UserId = userIdentity.UserId, FirstName = userIdentity.FirstName,
                LastName = userIdentity.LastName, BirthDate = userIdentity.BirthDate, PhoneNumber = userIdentity.PhoneNumber,
                Country = userIdentity.Country, City = userIdentity.City, Address = userIdentity.Address, Gender = userIdentity.Gender,
                Email = userIdentity.Email });
            await _appDbContext.SaveChangesAsync();

            return new OkObjectResult("Account created");
        }
    }
}