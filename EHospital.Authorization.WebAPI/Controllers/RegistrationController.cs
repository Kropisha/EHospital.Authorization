using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using EHospital.Authorization.BusinessLogic.Credentials;
using EHospital.Authorization.BusinessLogic.EmailAction;
using EHospital.Authorization.BusinessLogic.Enums;
using EHospital.Authorization.Data;
using EHospital.Authorization.Models;
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

        private readonly IUserDataProvider _appDbContext;

        private EmailSender emailSender = new EmailSender();

        public RegistrationController(IUserDataProvider appDbContext, ILogging logger)
        {
            _appDbContext = appDbContext;
            _log = logger;
        }

        /// <summary>
        /// Registers new user
        /// </summary>
        /// <param name="userData">new user</param>
        /// <param name="userSecrets">new password</param>
        /// <returns>success</returns>
        [HttpPost("Registration")]
        public async Task<IActionResult> Registration(UsersData userData, Secrets userSecrets)
        {
            _log.LogInfo("Get userData for registration.");
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
                        using (SqlConnection connection =
                            new SqlConnection("Data Source=JULIKROP\\SQLEXPRESS;Initial Catalog=EHospital;Integrated Security=True"))
                        {
                            connection.Open();
                            using (var transaction = connection.BeginTransaction())
                            {
                                try
                                {
                                    _log.LogInfo("Add default role.");
                                    await _appDbContext.AddRoles(new Roles
                                        {Id = (int) UsersRoles.NoRole, Title = UsersRoles.NoRole.ToString()});

                                    _log.LogInfo("Add login.");
                                    await _appDbContext.AddLogin(new Logins {Login = userData.Email,
                                        RegisterKey = emailSender.GenerateKey(), Status = "New" });

                                    _log.LogInfo("Add user's userData");
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
                                    await _appDbContext.AddSecrets(new Secrets {Password = userSecrets.Password});

                                    transaction.Commit();
                                }
                                catch (Exception ex)
                                {
                                    _log.LogError("Account is not created." + ex.Message);
                                    transaction.Rollback();
                                    return new BadRequestObjectResult("Creation of account was failed." + ex.Message);
                                }
                                finally
                                {
                                    transaction.Dispose();
                                }
                            }
                        }
                    }
                    else
                    {
                        _log.LogError("Account is not created.");
                        return new BadRequestObjectResult("Creation of account was failed.");
                    }

                    string greetingText;

                    using (StreamReader streamreader = new StreamReader(@"..\EHospital.Authorization.WebAPI\Letters\greetings.txt"))
                    {
                        greetingText = streamreader.ReadToEnd();
                    }

                    _log.LogInfo("Send greetings.");
                    await emailSender.SendEmail(userData.Email, "Welcome to EHospital", greetingText);

                    int id = await _appDbContext.FindByLogin(userData.Email);
                    string key = await _appDbContext.GetRegisterKey(userData.Email);
                    var callbackUrl = $"{Request.Scheme}://{Request.Host}/authorization/api/Registration/Confirmation?userId={id}&token={key}";

                    _log.LogInfo("Send confirmation");
                    await emailSender.SendEmail(userData.Email, "Confirm the registration",
                        $"Confirm the registration by clicking the following link: <a href='{callbackUrl}'>confirm</a>");

                    _log.LogInfo("Account created");
                    Task.WaitAll();
                    return new OkObjectResult("Account created. We sent letter on your email.Confirm it. If you don`t see the letter, please, check the spam.");
                }

                _log.LogError("Account is not created.");
                return new BadRequestObjectResult("Creation of account was failed.");
            }
            catch (ArgumentException ex)
            {
                _log.LogError("Account is not created." + ex.Message);
                return new BadRequestObjectResult("Creation of account was failed." + ex.Message);
            }
        }

        /// <summary>
        /// Endpoint for confirming emails
        /// </summary>
        /// <param name="userId">user's id</param>
        /// <param name="token">registration key</param>
        /// <returns>ok</returns>
        [HttpGet("Confirmation")]
        public async Task<IActionResult> ConfirmEmail(int userId, string token)
        {
            _log.LogInfo("User try to confirm email.");
            if (!ModelState.IsValid)
            {
                _log.LogError("Incorrect input.");
                return BadRequest(ModelState);
            }

            if (await _appDbContext.Confirm(userId, token))
            {
                _log.LogInfo("Success confirmed.");
                return Ok();
            }
            else
            {
                _log.LogError("Wrong confirm details");
                return BadRequest("Not confirmed");
            }
        }
    }
}