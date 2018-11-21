namespace EHospital.Authorization
{
    using EHospital.Authorization.Model;
    using System.Threading.Tasks;

    public interface IDataProvider
    {
        Task AddLogin(Logins login);

        Task AddRoles(Roles roles);

        Task AddSecrets(Secrets secrets);

        Task AddSession(Sessions sessions);

        Task AddUserData(UsersData usersData);

        // TODO: add filter [only for admin]
        Task<Logins> ChangeRole(Roles roles);

        Task<UsersData> ChangeUserData(UsersData usersData);

        Task<Secrets> ChangePassword(Secrets secrets);

        int FindByLogin(string login);

        Task DeleteUser(int id);

        bool CheckPassword(string password, int userId);

        string GetRole(int userId);

        int GetUserPassword(string password);

        Task Save();
    }
}