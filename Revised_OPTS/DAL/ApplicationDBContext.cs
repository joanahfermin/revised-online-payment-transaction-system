using Amazon.IdentityManagement.Model;
using Inventory_System.Model;
using Microsoft.EntityFrameworkCore;
using Revised_OPTS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revised_OPTS.DAL
{
    /// <summary>
    /// ApplicationDBContext inherits from the DbContext which came from Entity Framework.
    /// </summary>
    internal class ApplicationDBContext : DbContext
    {
        /// <summary>
        /// Provides the connection of the database.
        /// </summary>
        public const string MISCSERVER_TESTDB_CONNECTION_STRING = @"Server=CTO-MISCSERVER;Database=TestDB;User ID = joanahf; Password=Joanah1992; TrustServerCertificate = True";

        /// <summary>
        /// 
        /// </summary>
        public static ApplicationDBContext Instance = Create();

        private static ApplicationDBContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseSqlServer(MISCSERVER_TESTDB_CONNECTION_STRING) // Provide your connection string here.
                .Options;

            return new ApplicationDBContext(options);
        }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        /// <summary>
        /// DbSet communicates with the RPT table from the database. 
        /// </summary>
        public DbSet<Rpt> Rpts { get; set; }

        /// <summary>
        /// DbSet communicates with the Miscellaneous table from the database. 
        /// </summary>
        public DbSet<Miscellaneous> Miscellaneous { get; set; }

        /// <summary>
        /// DbSet communicates with the Business table from the database. 
        /// </summary>
        public DbSet<Business> purchase { get; set; }

        /// <summary>
        /// DbSet communicates with the Bank table from the database. 
        /// </summary>
        public DbSet<Bank> banks { get; set; }

        /// <summary>
        /// DbSet communicates with the UserAccount table from the database. 
        /// </summary>
        public DbSet<UserAccount> users { get; set; }

    }

}
