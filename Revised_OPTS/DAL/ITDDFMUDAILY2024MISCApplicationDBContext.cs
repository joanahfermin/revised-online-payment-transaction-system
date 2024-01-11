using Inventory_System.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.DAL
{
    internal class ITDDFMUDAILY2024MISCApplicationDBContext : DbContext
    {
        public const string MISCREPORTV_CONNECTION_STRING = @"Server=CTO-MISCREPORTV;Database=ITDD-FMU_DAILY_2024;User ID = jfermin; Password=Joanah1992; TrustServerCertificate = True";

        public static ITDDFMUDAILY2024MISCApplicationDBContext Instance = Create();

        private static ITDDFMUDAILY2024MISCApplicationDBContext Create()
        {
            var options = new DbContextOptionsBuilder<ITDDFMUDAILY2024MISCApplicationDBContext>()
                .UseSqlServer(MISCREPORTV_CONNECTION_STRING) // Provide your connection string here.
                .Options;

            return new ITDDFMUDAILY2024MISCApplicationDBContext(options);
        }

        public ITDDFMUDAILY2024MISCApplicationDBContext(DbContextOptions<ITDDFMUDAILY2024MISCApplicationDBContext> options) : base(options)
        {

        }

        public DbSet<MiscDetailsBillingStage> miscDetailsBillingStages { get; set; }

    }
}
