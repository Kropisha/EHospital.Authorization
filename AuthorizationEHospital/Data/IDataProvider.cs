namespace eHospital.Authorization
{
    using eHospital.Authorization.Models;

    public interface IDataProvider
    {
        void AddLogin(Logins login);

        void AddRoles(Roles roles);

        void AddSecrets(Secrets secrets);

        void AddSession(Sessions sessions);

        void AddUserData(UsersData usersData);

        // TODO: add filter [only for admin]
        Logins ChangeRole(Roles roles);

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