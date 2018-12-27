using System;
using System.Threading.Tasks;
using EHospital.Authorization.Models;

namespace EHospital.Authorization.Data
{
    public interface IUserDataProvider : IDisposable
    {
        /// <summary>
        /// Add new login
        /// </summary>
        /// <param name="login">user's login</param>
        /// <returns>ok</returns>
        Task AddLogin(Logins login);

        /// <summary>
        /// Add new role
        /// </summary>
        /// <param name="roles">user's role</param>
        /// <returns>ok</returns>
        Task AddRoles(Roles roles);

        /// <summary>
        /// Add new password
        /// </summary>
        /// <param name="secrets">user's password</param>
        /// <returns>ok</returns>
        Task AddSecrets(Secrets secrets);

        /// <summary>
        /// Add new session
        /// </summary>
        /// <param name="sessions">user's session</param>
        /// <returns>ok</returns>
        Task AddSession(Sessions sessions);

        /// <summary>
        /// Add new user's userData
        /// </summary>
        /// <param name="usersData">user's userData</param>
        /// <returns>ok</returns>
        Task AddUserData(UsersData usersData);

        // TODO: add filter [only for admin]
        /// <summary>
        /// Change role for user
        /// </summary>
        /// <param name="roles">new role</param>
        /// <returns>login with new role</returns>
        Task<Logins> ChangeRole(Roles roles);

        /// <summary>
        /// Change user's userData
        /// </summary>
        /// <param name="usersData">user's userData</param>
        /// <returns>new user's userData</returns>
        Task<UsersData> ChangeUserData(UsersData usersData);

        /// <summary>
        /// Change password for user
        /// </summary>
        /// <param name="secrets">password</param>
        /// <returns>new password</returns>
        Task<Secrets> ChangePassword(Secrets secrets);

        /// <summary>
        /// Log out from application
        /// </summary>
        /// <param name="userId">user's id</param>
        /// <returns>ok</returns>
        Task LogOut(int userId);

        /// <summary>
        /// Find user by login
        /// </summary>
        /// <param name="login">credential's login</param>
        /// <returns>user's id</returns>
        Task<int> FindByLogin(string login);

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="id">user's id</param>
        /// <returns>ok</returns>
        Task DeleteUser(int id);

        /// <summary>
        /// Delete previous sessions
        /// </summary>
        /// <param name="userId">user's id</param>
        /// <returns>ok</returns>
        Task DeleteSessions(int userId);

        /// <summary>
        /// Check if exist user with this login
        /// </summary>
        /// <param name="email">user's login</param>
        /// <returns>is exist</returns>
        Task<bool> IsUserExist(string email);

        /// <summary>
        /// Check is usuer already in system
        /// </summary>
        /// <param name="email">users email</param>
        /// <returns>token if authorized</returns>
        Task<string> IsAuthorized(string email);

        /// <summary>
        /// Confirm email if link was clicked
        /// </summary>
        /// <param name="userId"> user id</param>
        /// <param name="token">register key</param>
        /// <returns>true/false</returns>
        Task<bool> Confirm(int userId, string token);

        /// <summary>
        /// Check is email confirmed
        /// </summary>
        /// <param name="email">users email</param>
        /// <returns>true/false</returns>
        Task<bool> IsConfirmed(string email);

        Task<string> GetRegisterKey(string email);

        /// <summary>
        /// Check if this user has previous sessions
        /// </summary>
        /// <param name="userId">user's id</param>
        /// <returns>is exist</returns>
        Task<bool> IsExistPreviousSession(int userId);

        /// <summary>
        /// Check user's password
        /// </summary>
        /// <param name="password">user's password</param>
        /// <param name="userId">user's id</param>
        /// <returns>if password correct</returns>
        Task<bool> CheckPassword(string password, int userId);

        /// <summary>
        /// Get role by user's id
        /// </summary>
        /// <param name="userId">user's id</param>
        /// <returns>role</returns>
        Task<string> GetRole(int userId);

        /// <summary>
        /// Get email by user role
        /// </summary>
        /// <param name="role">role</param>
        /// <returns>email</returns>
        Task<string> GetEmail(Roles role);

        /// <summary>
        /// Get user's role by token
        /// </summary>
        /// <param name="token">current token</param>
        /// <returns>role</returns>
        Task<string> GetRoleByToken(string token);

        /// <summary>
        /// Get users id from token
        /// </summary>
        /// <param name="token">token from headers</param>
        /// <returns>user id</returns>
        Task<int> GetId(string token);

        /// <summary>
        /// Gets the user authentication information.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>User Auth info if session was found.</returns>
        Task<UserAuthDataModel> GetUserAuthInfo(string token);

        /// <summary>
        /// For saving changes
        /// </summary>
        /// <returns>ok</returns>
        Task Save();
    }
}