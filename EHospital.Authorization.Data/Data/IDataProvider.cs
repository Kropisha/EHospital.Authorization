using System;
using System.Threading.Tasks;
using EHospital.Authorization.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace EHospital.Authorization.Data.Data
{
    public interface IDataProvider : IDisposable
    {
         DbSet<UsersData> UsersData { get; set; }

         DbSet<Logins> Logins { get; set; }

         DbSet<Roles> Roles { get; set; }

         DbSet<Secrets> Secrets { get; set; }

         DbSet<Sessions> Sessions { get; set; }

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
        /// Add new user's data
        /// </summary>
        /// <param name="usersData">user's data</param>
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
        /// Change user's data
        /// </summary>
        /// <param name="usersData">user's data</param>
        /// <returns>new user's data</returns>
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
        /// Get user's role by token
        /// </summary>
        /// <param name="token">current token</param>
        /// <returns>role</returns>
        Task<string> GetRoleByToken(string token);

        /// <summary>
        /// For saving changes
        /// </summary>
        /// <returns>ok</returns>
        Task Save();
    }
}