namespace EHospital.Authorization
{
    using System.Threading.Tasks;
    using EHospital.Authorization.Model;

    public interface IDataProvider
    {
        string Token { get; set; }

        Task AddLogin(Logins login);

        Task AddRoles(Roles roles);

        Task AddSecrets(Secrets secrets);

        Task AddSession(Sessions sessions);

        Task AddUserData(UsersData usersData);

        // TODO: add filter [only for admin]
        Task<Logins> ChangeRole(Roles roles);

        Task<UsersData> ChangeUserData(UsersData usersData);

        Task<Secrets> ChangePassword(Secrets secrets);

        Task LogOut(int userId);

        Task<int> FindByLogin(string login);

        Task DeleteUser(int id);

        Task DeleteSessions(int userId);

        Task<bool> IsUserExist(string email);

        Task<bool> IsExistPreviousSession(int userId);

        Task<bool> CheckPassword(string password, int userId);

        Task<string> GetRole(int userId);

        Task<string> GetRoleByToken(string token);

        Task Save();
    }
}