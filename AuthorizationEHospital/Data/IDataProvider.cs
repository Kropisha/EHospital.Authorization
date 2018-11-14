using eHospital.Authorization.Models;
using Microsoft.EntityFrameworkCore;

namespace eHospital.Authorization
{
    public interface IDataProvider
    {
        //DbSet<Logins> Logins { get; set; }
        //DbSet<Roles> Roles { get; set; }
        //DbSet<Secrets> Secrets { get; set; }
        //DbSet<Sessions> Sessions { get; set; }
        //DbSet<UsersData> UsersData { get; set; }

        void AddLogin(Logins login);
        void AddRoles(Roles roles);
        void AddSecrets(Secrets secrets);
        void AddSession(Sessions sessions);
        void AddUserData(UsersData usersData);

        //TODO: add filter [only for admin]
        Roles ChangeRole(Roles roles);
        UsersData ChangeUserData(UsersData usersData);
        Secrets ChangePassword(Secrets secrets);

        int FindByLogin(string login);
        void DeleteUser(int id);
        bool CheckPassword(string password, int userId);
        string GetRole(int userId);
        int GetUserPassword(string password);

        void Save();
    }
}