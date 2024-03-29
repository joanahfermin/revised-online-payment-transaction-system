﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.Model
{
    internal class AllTaxTypeReport: ICloneable
    {
        public string TaxType { get; set; }
        public string? BillNumber { get; set; }
        //public string? TaxPayerName { get; set; }
        public decimal? Collection { get; set; }
        public decimal? Billing { get; set; }
        //public decimal TotalAmountTransferrred { get; set; }
        public decimal? ExcessShort { get; set; }
        public string? Remarks { get; set; }

        public decimal? MiscFees { get; set; }


        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
