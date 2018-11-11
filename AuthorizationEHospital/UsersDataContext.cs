using eHospital.Authorization.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eHospital.Authorization
{
    public class UsersDataContext : DbContext
    {
        public UsersDataContext(DbContextOptions<UsersDataContext> options) : base (options)
        {

        }
        public DbSet<UsersData> usersDatas { get; set; }

        public DbSet<Logins> logins { get; set; }

        public DbSet<Roles> roles { get; set; }

        public DbSet<Secrets> secrets { get; set; }

        public DbSet<Sessions> sessions { get; set; }
    }
}
