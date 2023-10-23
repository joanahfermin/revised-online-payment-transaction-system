using Inventory_System.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.DAL
{
    internal class ITDDFMUDAILY2023ApplicationDBContext : DbContext
    {
        public const string MISCREPORTV_CONNECTION_STRING = @"Server=CTO-MISCREPORTV;Database=ITDD-FMU_DAILY_2023;User ID = jfermin; Password=Joanah1992; TrustServerCertificate = True";

        public static ITDDFMUDAILY2023ApplicationDBContext Instance = Create();

        private static ITDDFMUDAILY2023ApplicationDBContext Create()
        {
            var options = new DbContextOptionsBuilder<ITDDFMUDAILY2023ApplicationDBContext>()
                .UseSqlServer(MISCREPORTV_CONNECTION_STRING) // Provide your connection string here.
                .Options;

            return new ITDDFMUDAILY2023ApplicationDBContext(options);
        }

        public ITDDFMUDAILY2023ApplicationDBContext(DbContextOptions<ITDDFMUDAILY2023ApplicationDBContext> options) : base(options)
        {

        }

        public DbSet<MiscDetailsBillingStage> miscDetailsBillingStages { get; set; }

    }
}
