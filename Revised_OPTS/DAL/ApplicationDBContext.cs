using Microsoft.EntityFrameworkCore;
using Revised_OPTS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revised_OPTS.DAL
{
    internal class ApplicationDBContext : DbContext
    {
        public const string MISCSERVER_TESTDB_CONNECTION_STRING = @"Server=CTO-MISCSERVER;Database=TestDB;User ID = joanahf; Password=Joanah1992; TrustServerCertificate = True";

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

        public DbSet<Rpt> Rpts { get; set; }
        public DbSet<Miscellaneous> Miscellaneous { get; set; }
        public DbSet<Business> purchase { get; set; }
        public DbSet<Bank> banks { get; set; }
    }
}
