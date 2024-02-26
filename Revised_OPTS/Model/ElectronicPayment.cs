using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.Model
{
    internal class ElectronicPayment
    {
        public string? EpaymentRef { get; set; } //COLUMN A
        public string? EpaymentTransactionRef { get; set; } //COLUMN B
        public string? BillerRef { get; set; } //COLUMN C
        public string? ServiceProvider { get; set; } //COLUMN D (Payment channel ng lahat)
        public string? BillerId { get; set; } //COLUMN E
        public string? BillerInfo1 { get; set; } //COLUMN F (YEAR)
        public string? Quarter { get; set; } 
        public string? BillerInfo2 { get; set; } //COLUMN G
        public string? BillingSelection { get; set; } 
        public string? BillerInfo3 { get; set; } //COLUMN H
        public decimal? AmountDue { get; set; } //COLUMN J
        public DateTime? Date { get; set; } //COLUMN M
    }
}
