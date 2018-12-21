using System;
using System.Linq;
using System.Threading.Tasks;
using EHospital.Authorization.BusinessLogic.Credentials;
using EHospital.Authorization.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace EHospital.Authorization.Data.Data
{
    /// <summary>
    /// For interface implementation
    /// </summary>
    public class UsersDataContext : DbContext, IDataProvider
    {
        public UsersDataContext(DbContextOptions<UsersDataContext> options) : base(options) { }

        public UsersDataContext()
        {
        }

        public DbSet<UsersData> UsersData { get; set; }

        public DbSet<Logins> Logins { get; set; }

        public DbSet<Roles> Roles { get; set; }

        public DbSet<Secrets> Secrets { get; set; }

        public DbSet<Sessions> Sessions { get; set; }

        /// <summary>
        /// Add new role
        /// </summary>
        /// <param name="roles">user's role</param>
        /// <returns>ok</returns>
        public async Task AddRoles(Roles roles)
        {
            {
                await Roles.AddAsync(roles);
                await SaveChangesAsync();
            }
        }

        /// <summary>
        /// Add new login
        /// </summary>
        /// <param name="login">user's login</param>
        /// <returns>ok</returns>
        public async Task AddLogin(Logins login)
        {
            Logins existed = Logins.FirstOrDefault(x => x.Login == login.Login);
            if (existed == null)
            {
                var role = Roles.LastOrDefault(x => x.Id > 0);
                if (role != null)
                {
                    login.RoleId = role.Id;
                }

                await Logins.AddAsync(login);
                await SaveChangesAsync();
            }
        }

        /// <summary>
        /// Add new password
        /// </summary>
        /// <param name="secrets">user's password</param>
        /// <returns>ok</returns>
        public async Task AddSecrets(Secrets secrets)
        {
            Secrets existed = Secrets.FirstOrDefault(x => x.Id == secrets.Id);
            if (existed == null)
            {
                var login = Logins.LastOrDefault(x => x.Id > 0);
                if (login != null)
                {
                    secrets.Id = login.Id;
                }

                secrets.Password = PasswordManager.HashPassword(secrets.Password);
                await Secrets.AddAsync(secrets);
                await SaveChangesAsync();
            }
        }

        /// <summary>
        /// Add new session
        /// </summary>
        /// <param name="sessions">user's session</param>
        /// <returns>ok</returns>
        public async Task AddSession(Sessions sessions)
        {
            Sessions existed = Sessions.FirstOrDefault(x => x.Token == sessions.Token);
            if (existed == null)
            {
                await Sessions.AddAsync(sessions);
                await SaveChangesAsync();
            }
        }

        /// <summary>
        /// Add new user's data
        /// </summary>
        /// <param name="usersData">user's data</param>
        /// <returns>ok</returns>
        public async Task AddUserData(UsersData usersData)
        {
            UsersData existed = UsersData.FirstOrDefault(x => x.Email == usersData.Email);
            if (existed == null)
            {
                var login = Logins.FirstOrDefault(x => x.Login == usersData.Email);
                if (login != null)
                {
                    usersData.Id = login.Id;
                }

                await UsersData.AddAsync(usersData);
                await SaveChangesAsync();
            }
        }

        /// <summary>
        /// Change role for user
        /// </summary>
        /// <param name="roles">new role</param>
        /// <returns>login with new role</returns>
        public async Task<Logins> ChangeRole(Roles roles)
        {
            Logins existed = Logins.FirstOrDefault(x => x.RoleId == roles.Id);
            if (existed != null)
            {
                var current = Roles.FirstOrDefault(x => x.Id == existed.RoleId);
                if (current != null)
                {
                    current.Title = roles.Title;
                }

                await SaveChangesAsync();
            }

            return existed;
        }

        /// <summary>
        /// Change password for user
        /// </summary>
        /// <param name="secrets">password</param>
        /// <returns>new password</returns>
        public async Task<Secrets> ChangePassword(Secrets secrets)
        {
            Secrets existed = Secrets.FirstOrDefault(s => s.Id == secrets.Id);
            if (existed != null)
            {
                existed.Password = PasswordManager.HashPassword(secrets.Password);
                await SaveChangesAsync();
            }

            return existed;
        }

        /// <summary>
        /// Change user's data
        /// </summary>
        /// <param name="usersData">user's data</param>
        /// <returns>new user's data</returns>
        public async Task<UsersData> ChangeUserData(UsersData usersData)
        {
            UsersData existed = UsersData.FirstOrDefault(x => x.Id == usersData.Id);
            if (existed != null)
            {
                existed.FirstName = usersData.FirstName;
                existed.LastName = usersData.LastName;
                existed.FirstName = usersData.FirstName;
                existed.BirthDate = usersData.BirthDate;
                existed.PhoneNumber = usersData.PhoneNumber;
                existed.Country = usersData.Country;
                existed.City = usersData.City;
                existed.Adress = usersData.Adress;
                existed.Gender = usersData.Gender;
                existed.Email = usersData.Email;
                await SaveChangesAsync();
            }

            return existed;
        }

        /// <summary>
        /// Find user by login
        /// </summary>
        /// <param name="login">credential's login</param>
        /// <returns>user's id</returns>
        public async Task<int> FindByLogin(string login)
        {
            Logins existed = await Logins.FirstOrDefaultAsync(x => x.Login == login);

            if (existed == null)
            {
                return 0;
            }
            else
            {
                return existed.Id;
            }
        }

        /// <summary>
        /// Log out from application
        /// </summary>
        /// <param name="userId">user's id</param>
        /// <returns>ok</returns>
        public async Task LogOut(int userId)
        {
            Sessions existed = Sessions.LastOrDefault(x => x.UserId == userId);
            if (existed != null)
            {
                existed.ExpiredDate = DateTime.Now;
                await SaveChangesAsync();
            }
        }

        /// <summary>
        /// Get role by user's id
        /// </summary>
        /// <param name="userId">user's id</param>
        /// <returns>role</returns>
        public async Task<string> GetRole(int userId)
        {
            Roles existed = await Roles.FirstOrDefaultAsync(r => r.Id == userId);
            if (existed != null)
            {
                return existed.Title;
            }
            else
            {
                return "noRole";
            }
        }

        /// <summary>
        /// Get user's role by token
        /// </summary>
        /// <param name="token">current token</param>
        /// <returns>role</returns>
        public async Task<string> GetRoleByToken(string token)
        {
            Sessions existed = await Sessions.LastOrDefaultAsync(s => s.Token == token);
            if (existed != null)
            {
                if ((existed.ExpiredDate.Hour + existed.ExpiredDate.Minute) > (DateTime.Now.Hour + DateTime.Now.Minute))
                {
                    var current = Roles.FirstOrDefault(r => r.Id == existed.UserId);
                    if (current != null)
                    {
                        return current.Title;
                    }
                }

                return "sessionIsEnd";
            }

            return "sessionIsEnd";
        }

        /// <summary>
        /// Check if exist user with this login
        /// </summary>
        /// <param name="email">user's login</param>
        /// <returns>is exist</returns>
        public async Task<bool> IsUserExist(string email)
        {
            Logins existed = await Logins.FirstOrDefaultAsync(e => e.Login == email);
            if (existed == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Check if this user has previous sessions
        /// </summary>
        /// <param name="userId">user's id</param>
        /// <returns>is exist</returns>
        public async Task<bool> IsExistPreviousSession(int userId)
        {
            Sessions existed = await Sessions.FirstOrDefaultAsync(x => x.UserId == userId);
            if (existed != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check user's password
        /// </summary>
        /// <param name="password">user's password</param>
        /// <param name="userId">user's id</param>
        /// <returns>if password correct</returns>
        public async Task<bool> CheckPassword(string password, int userId)
        {
            Secrets existed = await Secrets.FirstOrDefaultAsync(s => s.Id == userId);
            if (existed != null)
            {
                if (PasswordManager.VerifyHashedPassword(existed.Password, password))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="id">user's id</param>
        /// <returns>ok</returns>
        public async Task DeleteUser(int id)
        {
            UsersData existed = UsersData.FirstOrDefault(x => x.Id == id);
            if (existed != null)
            {
                existed.IsDeleted = true;
                await SaveChangesAsync();
            }
        }

        /// <summary>
        /// Delete previous sessions
        /// </summary>
        /// <param name="userId">user's id</param>
        /// <returns>ok</returns>
        public async Task DeleteSessions(int userId)
        {
            Sessions existed = Sessions.FirstOrDefault(x => x.UserId == userId);
            if (existed != null)
            {
                Sessions.Remove(existed);
                await SaveChangesAsync();
            }
        }

        /// <summary>
        /// For saving changes
        /// </summary>
        /// <returns>ok</returns>
        public async Task Save()
        {
            await SaveChangesAsync();
        }
    }
}
