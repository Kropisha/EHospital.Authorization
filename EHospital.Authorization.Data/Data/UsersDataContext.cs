namespace EHospital.Authorization.Data
{
    using System.Linq;
    using EHospital.Authorization.Model;
    using Microsoft.EntityFrameworkCore;

    public class UsersDataContext : DbContext, IDataProvider
    {
        public UsersDataContext(DbContextOptions<UsersDataContext> options) : base(options)
        {
        }

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

        public void AddRoles(Roles roles)
        {
            Roles existed = this.Roles.FirstOrDefault(x => x.RoleId == roles.RoleId);
            if (existed == null)
            {
                this.Roles.Add(roles);
                this.SaveChanges();
            }
        }

        public void AddLogin(Logins login)
        {
            Logins existed = this.Logins.FirstOrDefault(x => x.Login == login.Login);
            if (existed == null)
            {
                this.Logins.Add(login);
                this.SaveChanges();
            }
        }

        public void AddSecrets(Secrets secrets)
        {
            Secrets existed = this.Secrets.FirstOrDefault(x => x.UserId == secrets.UserId);
            if (existed == null)
            {
                this.Secrets.Add(secrets);
                this.SaveChanges();
            }
        }

        public void AddSession(Sessions sessions)
        {
            Sessions existed = this.Sessions.FirstOrDefault(x => x.Token == sessions.Token);
            if (existed == null)
            {
                this.Sessions.Add(sessions);
                this.SaveChanges();
            }
        }

        public void AddUserData(UsersData usersData)
        {
            UsersData existed = this.UsersData.FirstOrDefault(x => x.UserId == usersData.UserId);
            if (existed == null)
            {
                this.UsersData.Add(usersData);
                this.SaveChanges();
            }
        }

        public Logins ChangeRole(Roles roles)
        {
            Logins existed = this.Logins.FirstOrDefault(x => x.RoleId == roles.RoleId);
            if (existed != null)
            {
                existed.RoleId = roles.RoleId;
                this.SaveChanges();
            }

            return existed;
        }

        public Secrets ChangePassword(Secrets secrets)
        {
            Secrets existed = this.Secrets.FirstOrDefault(s => s.UserId == secrets.UserId);
            if (existed != null)
            {
                existed.Password = secrets.Password;
                this.SaveChanges();
            }

            return existed;
        }

        public UsersData ChangeUserData(UsersData usersData)
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
                this.SaveChanges();
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

        public void DeleteUser(int id)
        {
            UsersData existed = this.UsersData.FirstOrDefault(x => x.UserId == id);
            if (existed != null)
            {
                existed.IsDeleted = true;
                this.SaveChanges();
            }
        }

        public void Save()
        {
            this.SaveChanges();
        }
    }
}
