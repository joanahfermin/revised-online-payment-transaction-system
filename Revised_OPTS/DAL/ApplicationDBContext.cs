﻿using Amazon.IdentityManagement.Model;
using Inventory_System.Model;
using Inventory_System.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
        //public const string MISCSERVER_TESTDB_CONNECTION_STRING = @"Server=localhost;Database=master;Trusted_Connection=True; TrustServerCertificate = True";

        /// <summary>
        /// 
        /// </summary>
        public static ApplicationDBContext CurrentInstance = null;

        public static ApplicationDBContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseSqlServer(MISCSERVER_TESTDB_CONNECTION_STRING) // Provide your connection string here.
                .Options;
            CurrentInstance = new ApplicationDBContext(options);
            return CurrentInstance;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*
            var converter = new ValueConverter<bool?, int?>(
            v => v.HasValue ? (v.Value ? 1 : 0) : (int?)null,
            v => v.HasValue ? (v.Value == 1) : (bool?)null);

            modelBuilder.Entity<Miscellaneous>()
                .Property(e => e.DeletedRecord)
                .HasConversion(converter);*/
           // /*
            modelBuilder.Entity<Miscellaneous>()
                .Property(e => e.DeletedRecord)
                .HasConversion(new DeletedRecordBoolToIntConverter());
            //*/
            modelBuilder.Entity<AllTaxTypeReport>().HasNoKey();
            modelBuilder.Entity<UserActivityReport>().HasNoKey();
            modelBuilder.Entity<EmailAccount>().HasNoKey();
            modelBuilder.Entity<SystemSetting>().HasNoKey();
            // Configure other entities and their properties here...
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

        public DbSet<RPTAttachPicture> rptPictures { get; set; }
        public DbSet<AllTaxTypeReport> allTaxTypeReports { get; set; }
        public DbSet<UserActivityReport> allUserActivityReports { get; set; }

        public DbSet<EmailTemplate> allEmailTemplates { get; set; }

        public DbSet<EmailAccount> allEmailAccounts { get; set; }
        public DbSet<SystemSetting> allSystemSettings { get; set; }
    }

}
