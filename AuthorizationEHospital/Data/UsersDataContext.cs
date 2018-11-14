using eHospital.Authorization.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eHospital.Authorization
{
    public class UsersDataContext : DbContext, IDataProvider
    {
       // IDataProvider _data;
        public UsersDataContext(DbContextOptions<UsersDataContext> options) : base (options)
        {
           // _data = data;
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
            modelBuilder.Entity<Logins>()
                .HasKey(c => new { c.LoginId});
            modelBuilder.Entity<Sessions>()
                .HasKey(c => new { c.SessionId });
            modelBuilder.Entity<Roles>()
                .HasKey(c => new { c.RoleId });
        }

        public void AddLogin(Logins login)
        {
            Logins existed = Logins.FirstOrDefault(x => x.LoginId == login.LoginId);
            if (existed == null)
            {
                Logins.Add(login);
                SaveChanges();
            }         
        }

        public void AddRoles(Roles roles)
        {
            Roles existed = Roles.FirstOrDefault(x => x.RoleId == roles.RoleId);
            if (existed == null)
            {
                Roles.Add(roles);
                SaveChanges();
            }
        }

        public void AddSecrets(Secrets secrets)
        {
            Secrets existed = Secrets.FirstOrDefault(x => x.UserId == secrets.UserId);
            if (existed == null)
            {
                Secrets.Add(secrets);
                SaveChanges();
            }
        }

        public void AddSession(Sessions sessions)
        {
            Sessions existed = Sessions.FirstOrDefault(x => x.Token == sessions.Token);
            if (existed == null)
            {
                //var current = Sessions.LastOrDefault(s => s.SessionId != sessions.SessionId);
                //sessions.SessionId = current.SessionId + 1;
                Sessions.Add(sessions);
                SaveChanges();
            }
        }

        public void AddUserData(UsersData usersData)
        {
            UsersData existed = UsersData.FirstOrDefault(x => x.UserId == usersData.UserId);
            if (existed == null)
            {
                UsersData.Add(usersData);
                SaveChanges();
            }
        }

        public Roles ChangeRole(Roles roles)
        {
            Roles existed = Roles.FirstOrDefault(x => x.RoleId == roles.RoleId);
            if (existed != null)
            {
                existed.RoleId = roles.RoleId;
                existed.Title = roles.Title;
                SaveChanges();
            }
            return existed;
        }

        public Secrets ChangePassword(Secrets secrets)
        {
            Secrets existed = Secrets.FirstOrDefault(s => s.UserId == secrets.UserId);
            if (existed != null)
            {
                existed.Password = secrets.Password;
                SaveChanges();
            }
            return existed;
        }

        public UsersData ChangeUserData(UsersData usersData)
        {
            UsersData existed = UsersData.FirstOrDefault(x => x.UserId == usersData.UserId);
            if (existed != null)
            {
                existed.FirstName = usersData.FirstName;
                existed.LastName = usersData.LastName;
                existed.FirstName = usersData.FirstName;
                existed.BirthDate = usersData.BirthDate;
                existed.PhoneNumber = usersData.PhoneNumber;
                existed.Country = usersData.Country;
                existed.City = usersData.City;
                existed.Address = usersData.Address;
                existed.Gender = usersData.Gender;
                existed.Email = usersData.Email;
                SaveChanges();
            }
            return existed;
        }

        public int FindByLogin(string login)
        {
            Logins existed = Logins.FirstOrDefault(x => x.Login == login);
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
            Roles existed = Roles.FirstOrDefault(r => r.RoleId == userId);
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
            Secrets existed = Secrets.FirstOrDefault(s => s.Password == password);
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
            Secrets existed = Secrets.FirstOrDefault(s => s.Password == password);
            if (existed != null && existed.UserId == userId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public void DeleteUser(int id)
        {
            UsersData existed = UsersData.FirstOrDefault(x => x.UserId == id);
            if (existed != null)
            {
                existed.IsDeleted = true;
                SaveChanges();
            }
        }

        public void Save()
        {
            this.SaveChanges();
        }
    }
}
