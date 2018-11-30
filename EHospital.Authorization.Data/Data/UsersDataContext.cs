namespace EHospital.Authorization.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using EHospital.Authorization.Model;
    using EHospital.Authorization.WebAPI;
    using Microsoft.EntityFrameworkCore;

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

        public string Token { get; set; }

        public async Task AddRoles(Roles roles)
        {
            {
                await this.Roles.AddAsync(roles);
            }
        }

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

        public async Task AddSession(Sessions sessions)
        {
            Sessions existed = this.Sessions.FirstOrDefault(x => x.Token == sessions.Token);
            if (existed == null)
            {
                await this.Sessions.AddAsync(sessions);
            }
        }

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

        public async Task<int> FindByLogin(string login)
        {
            Logins existed = await this.Logins.FirstOrDefaultAsync(x => x.Login == login);

            return existed.Id;
        }

        public async Task LogOut(int userId)
        {
            Sessions existed = this.Sessions.LastOrDefault(x => x.UserId == userId);
            if (existed != null)
            {
                existed.ExpiredDate = DateTime.Now;
                await this.SaveChangesAsync();
            }
        }

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

        public async Task DeleteUser(int id)
        {
            UsersData existed = this.UsersData.FirstOrDefault(x => x.Id == id);
            if (existed != null)
            {
                existed.IsDeleted = true;
                await this.SaveChangesAsync();
            }
        }

        public async Task DeleteSessions(int userId)
        {
            Sessions existed = this.Sessions.FirstOrDefault(x => x.UserId == userId);
            if (existed != null)
            {
                this.Sessions.Remove(existed);
                await this.SaveChangesAsync();
            }
        }

        public async Task Save()
        {
            await this.SaveChangesAsync();
        }
    }
}
