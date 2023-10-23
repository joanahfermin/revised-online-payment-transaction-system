using Inventory_System.Model;
using Microsoft.EntityFrameworkCore;
using Revised_OPTS.DAL;
using Revised_OPTS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.DAL
{
    internal class SecondApplicationDBContext : DbContext
    {
        public const string MISCREPORTV_CONNECTION_STRING = @"Server=CTO-MISCREPORTV;Database=ITDD-FMU_DAILY_2022;User ID = jfermin; Password=Joanah1992; TrustServerCertificate = True";

        public static SecondApplicationDBContext Instance = Create();

        private static SecondApplicationDBContext Create()
        {
            var options = new DbContextOptionsBuilder<SecondApplicationDBContext>()
                .UseSqlServer(MISCREPORTV_CONNECTION_STRING) // Provide your connection string here.
                .Options;

            return new SecondApplicationDBContext(options);
        }

        public SecondApplicationDBContext(DbContextOptions<SecondApplicationDBContext> options) : base(options)
        {

        }

        public DbSet<RptTaxbillTPN> rptRetrieveTaxpayerNames { get; set; }

    }
}
