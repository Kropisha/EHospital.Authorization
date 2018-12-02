namespace EHospital.Authorization.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using EHospital.Authorization.Model;
    using EHospital.Authorization.WebAPI;
    using Microsoft.EntityFrameworkCore;

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
                await this.Roles.AddAsync(roles);
            }
        }

        /// <summary>
        /// Add new login
        /// </summary>
        /// <param name="login">user's login</param>
        /// <returns>ok</returns>
        public async Task AddLogin(Logins login)
        {
            Logins existed = this.Logins.FirstOrDefault(x => x.Login == login.Login);
            if (existed == null)
            {
                var role = this.Roles.LastOrDefault(x => x.Id > 0);
                login.RoleId = role.Id;
                await this.Logins.AddAsync(login);
            }
        }

        /// <summary>
        /// Add new password
        /// </summary>
        /// <param name="secrets">user's password</param>
        /// <returns>ok</returns>
        public async Task AddSecrets(Secrets secrets)
        {
            Secrets existed = this.Secrets.FirstOrDefault(x => x.Id == secrets.Id);
            if (existed == null)
            {
                var login = this.Logins.LastOrDefault(x => x.Id > 0);
                secrets.Id = login.Id;
                secrets.Password = PasswordManager.HashPassword(secrets.Password);
                await this.Secrets.AddAsync(secrets);
            }
        }

        /// <summary>
        /// Add new session
        /// </summary>
        /// <param name="sessions">user's session</param>
        /// <returns>ok</returns>
        public async Task AddSession(Sessions sessions)
        {
            Sessions existed = this.Sessions.FirstOrDefault(x => x.Token == sessions.Token);
            if (existed == null)
            {
                await this.Sessions.AddAsync(sessions);
                await this.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Add new user's data
        /// </summary>
        /// <param name="usersData">user's data</param>
        /// <returns>ok</returns>
        public async Task AddUserData(UsersData usersData)
        {
            UsersData existed = this.UsersData.FirstOrDefault(x => x.Email == usersData.Email);
            if (existed == null)
            {
                var login = this.Logins.FirstOrDefault(x => x.Login == usersData.Email);
                usersData.Id = login.Id;
                await this.UsersData.AddAsync(usersData);
            }
        }

        /// <summary>
        /// Change role for user
        /// </summary>
        /// <param name="roles">new role</param>
        /// <returns>login with new role</returns>
        public async Task<Logins> ChangeRole(Roles roles)
        {
            Logins existed = this.Logins.FirstOrDefault(x => x.RoleId == roles.Id);
            if (existed != null)
            {
                var current = this.Roles.FirstOrDefault(x => x.Id == existed.RoleId);
                current.Title = roles.Title;
                await this.SaveChangesAsync();
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
            Secrets existed = this.Secrets.FirstOrDefault(s => s.Id == secrets.Id);
            if (existed != null)
            {
                existed.Password = PasswordManager.HashPassword(secrets.Password);
                await this.SaveChangesAsync();
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
            UsersData existed = this.UsersData.FirstOrDefault(x => x.Id == usersData.Id);
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
                await this.SaveChangesAsync();
            }

            return existed;
        }

        /// <summary>
        /// Find user by login
        /// </summary>
        /// <param name="login">credentional's login</param>
        /// <returns>user's id</returns>
        public async Task<int> FindByLogin(string login)
        {
            Logins existed = await this.Logins.FirstOrDefaultAsync(x => x.Login == login);

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
            Sessions existed = this.Sessions.LastOrDefault(x => x.UserId == userId);
            if (existed != null)
            {
                existed.ExpiredDate = DateTime.Now;
                await this.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Get role by user's id
        /// </summary>
        /// <param name="userId">user's id</param>
        /// <returns>role</returns>
        public async Task<string> GetRole(int userId)
        {
            Roles existed = await this.Roles.FirstOrDefaultAsync(r => r.Id == userId);
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
            Sessions existed = await this.Sessions.LastOrDefaultAsync(s => s.Token == token);
            if ((existed.ExpiredDate.Hour + existed.ExpiredDate.Minute) < (DateTime.Now.Hour + DateTime.Now.Minute))
            {
                if (existed != null)
                {
                    var current = this.Roles.FirstOrDefault(r => r.Id == existed.UserId);
                    return current.Title;
                }
                else
                {
                    return "sessionIsEnd";
                }
            }
            else
            {
                return "sessionIsEnd";
            }
        }

        /// <summary>
        /// Check if exist user with this login
        /// </summary>
        /// <param name="email">user's login</param>
        /// <returns>is exist</returns>
        public async Task<bool> IsUserExist(string email)
        {
            Logins existed = await this.Logins.FirstOrDefaultAsync(e => e.Login == email);
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
            Sessions existed = await this.Sessions.FirstOrDefaultAsync(x => x.UserId == userId);
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
            Secrets existed = await this.Secrets.FirstOrDefaultAsync(s => s.Id == userId);
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
            UsersData existed = this.UsersData.FirstOrDefault(x => x.Id == id);
            if (existed != null)
            {
                existed.IsDeleted = true;
                await this.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Delete previous sessions
        /// </summary>
        /// <param name="userId">user's id</param>
        /// <returns>ok</returns>
        public async Task DeleteSessions(int userId)
        {
            Sessions existed = this.Sessions.FirstOrDefault(x => x.UserId == userId);
            if (existed != null)
            {
                this.Sessions.Remove(existed);
                await this.SaveChangesAsync();
            }
        }

        /// <summary>
        /// For saving changes
        /// </summary>
        /// <returns>ok</returns>
        public async Task Save()
        {
            await this.SaveChangesAsync();
        }
    }
}
