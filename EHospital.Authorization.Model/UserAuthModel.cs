using System;

namespace EHospital.Authorization.Models
{
     public class UserAuthDataModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the role identifier.
        /// </summary>
        /// <value>
        /// The role identifier.
        /// </value>
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or sets the user token expiration date.
        /// </summary>
        /// <value>
        /// The user token expiration date.
        /// </value>
        public DateTime UserTokenExpirationDate { get; set; }
    }
}
