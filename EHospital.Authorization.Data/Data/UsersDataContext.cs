namespace EHospital.Authorization.Data
{
    using System.Linq;
    using System.Threading.Tasks;
    using EHospital.Authorization.Model;
    using Microsoft.EntityFrameworkCore;

    public class UsersDataContext : DbContext, IDataProvider
    {
        public UsersDataContext(DbContextOptions<UsersDataContext> options) : base(options) { }

        public DbSet<UsersData> UsersData { get; set; }

        public DbSet<Logins> Logins { get; set; }

        public DbSet<Roles> Roles { get; set; }

        public DbSet<Secrets> Secrets { get; set; }

        public DbSet<Sessions> Sessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsersData>().ToTable("UsersData");
            modelBuilder.Entity<Logins>().ToTable("Logins");
            modelBuilder.Entity<Roles>().ToTable("Roles");
            modelBuilder.Entity<Secrets>().ToTable("Secrets");
            modelBuilder.Entity<Sessions>().ToTable("Sessions");
            modelBuilder.Entity<Roles>()
               .HasKey(c => new { c.RoleId });
            modelBuilder.Entity<Logins>()
                .HasKey(c => new { c.LoginId, c.RoleId });
            modelBuilder.Entity<UsersData>()
                .HasKey(c => new { c.UserId });
            modelBuilder.Entity<Sessions>()
                .HasKey(c => new { c.SessionId });
        }

        public async Task AddRoles(Roles roles)
        {
            Roles existed = this.Roles.FirstOrDefault(x => x.RoleId == roles.RoleId);
            if (existed == null)
            {
                this.Roles.Add(roles);
               await this.SaveChangesAsync();
            }
        }

        public async Task AddLogin(Logins login)
        {
            Logins existed = this.Logins.FirstOrDefault(x => x.Login == login.Login);
            if (existed == null)
            {
                this.Logins.Add(login);
               await this.Save(); // проверить как корректней
            }
        }

        public async Task AddSecrets(Secrets secrets)
        {
            Secrets existed = this.Secrets.FirstOrDefault(x => x.UserId == secrets.UserId);
            if (existed == null)
            {
                this.Secrets.Add(secrets);
                await this.SaveChangesAsync();
            }
        }

        public async Task AddSession(Sessions sessions)
        {
            Sessions existed = this.Sessions.FirstOrDefault(x => x.Token == sessions.Token);
            if (existed == null)
            {
                this.Sessions.Add(sessions);
                await this.SaveChangesAsync();
            }
        }

        public async Task AddUserData(UsersData usersData)
        {
            UsersData existed = this.UsersData.FirstOrDefault(x => x.UserId == usersData.UserId);
            if (existed == null)
            {
                this.UsersData.Add(usersData);
                await this.SaveChangesAsync();
            }
        }

        public async Task<Logins> ChangeRole(Roles roles)
        {
            Logins existed = this.Logins.FirstOrDefault(x => x.RoleId == roles.RoleId);
            if (existed != null)
            {
                existed.RoleId = roles.RoleId;
                await this.SaveChangesAsync();
            }

            return existed;
        }

        public async Task<Secrets> ChangePassword(Secrets secrets)
        {
            Secrets existed = this.Secrets.FirstOrDefault(s => s.UserId == secrets.UserId);
            if (existed != null)
            {
                existed.Password = secrets.Password;
                await this.SaveChangesAsync();
            }

            return existed;
        }

        public async Task<UsersData> ChangeUserData(UsersData usersData)
        {
            UsersData existed = this.UsersData.FirstOrDefault(x => x.UserId == usersData.UserId);
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

        public int FindByLogin(string login)
        {
            Logins existed = this.Logins.FirstOrDefault(x => x.Login == login);
            if (existed != null)
            {
                return existed.LoginId;
            }
            else
            {
                return 0;
            }
        }

        public string GetRole(int userId)
        {
            Roles existed = this.Roles.FirstOrDefault(r => r.RoleId == userId);
            if (existed != null)
            {
                return existed.Title;
            }
            else
            {
                return "noRole";
            }
        }

        public int GetUserPassword(string password)
        {
            Secrets existed = this.Secrets.FirstOrDefault(s => s.Password == password);
            if (existed != null)
            {
                return existed.UserId;
            }
            else
            {
                return 0;
            }
        }

        public bool CheckPassword(string password, int userId)
        {
            Secrets existed = this.Secrets.FirstOrDefault(s => s.Password == password);
            if (existed != null && existed.UserId == userId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task DeleteUser(int id)
        {
            UsersData existed = this.UsersData.FirstOrDefault(x => x.UserId == id);
            if (existed != null)
            {
                existed.IsDeleted = true;
                await this.SaveChangesAsync();
            }
        }

        public async Task Save()
        {
            await this.SaveChangesAsync();
        }
    }
}
