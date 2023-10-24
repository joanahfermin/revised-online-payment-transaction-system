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
    internal class ITDDFMUDAILY2022ApplicationDBContext : DbContext
    {
        public const string MISCREPORTV_CONNECTION_STRING = @"Server=CTO-MISCREPORTV;Database=ITDD-FMU_DAILY_2022;User ID = jfermin; Password=Joanah1992; TrustServerCertificate = True";

        public static ITDDFMUDAILY2022ApplicationDBContext Instance = Create();

        private static ITDDFMUDAILY2022ApplicationDBContext Create()
        {
            var options = new DbContextOptionsBuilder<ITDDFMUDAILY2022ApplicationDBContext>()
                .UseSqlServer(MISCREPORTV_CONNECTION_STRING) // Provide your connection string here.
                .Options;

            return new ITDDFMUDAILY2022ApplicationDBContext(options);
        }

        public ITDDFMUDAILY2022ApplicationDBContext(DbContextOptions<ITDDFMUDAILY2022ApplicationDBContext> options) : base(options)
        {

        }

        public DbSet<RptTaxbillTPN> rptRetrieveTaxpayerNames { get; set; }

        public DbSet<BusinessMasterDetailTPN> businessRetrieveTaxpayerNames { get; set; }

    }
}
