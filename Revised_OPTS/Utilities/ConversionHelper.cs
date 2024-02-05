using Inventory_System.Model;
using Revised_OPTS.Model;
using Revised_OPTS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.Utilities
{
    internal static class ConversionHelper
    {
        public static Rpt ConvertToRpt(ElectronicPayment ep)
        {
            Rpt rpt = new Rpt();

            rpt.TaxDec = ep.BillerId.ToString();
            rpt.TaxPayerName = ep.BillerInfo2;
            rpt.AmountToPay = ep.AmountDue;
            rpt.AmountTransferred = ep.AmountDue;
            rpt.TotalAmountTransferred = ep.AmountDue;
            rpt.ExcessShortAmount = 0;// excessShortAmount;
            rpt.Bank = ep.ServiceProvider.ToUpper();
            rpt.YearQuarter = ep.BillerInfo1;
            rpt.Quarter = ep.Quarter;
            rpt.PaymentType = null;
            rpt.BillingSelection = ep.BillingSelection;
            rpt.Status = TaxStatus.ForPaymentVerification;
            rpt.RequestingParty = ep.BillerInfo3;
            rpt.PaymentDate = ep.Date;

            return rpt;
        }

        public static Business ConvertToBusiness(ElectronicPayment ep)
        {
            Business bus = new Business();

            bus.Business_Type = null;
            bus.MP_Number = ep.BillerId;

            bus.TaxpayersName = ep.BillerInfo2.ToUpper();
            bus.BusinessName = null;

            bus.BillNumber = ep.BillerRef.ToUpper();
            bus.BillAmount = ep.AmountDue;
            bus.TotalAmount = ep.AmountDue;
            bus.PaymentChannel = ep.ServiceProvider.ToUpper();

            //bus.Year = ep.BillerInfo1;
            //bus.Qtrs = ep.Quarter;

            bus.Year = "-";
            bus.Qtrs = "-";

            bus.Status = TaxStatus.ForPaymentVerification;
            bus.RequestingParty = ep.BillerInfo3;
            bus.DateOfPayment = ep.Date;

            return bus;
        }

        public static Miscellaneous ConvertToMiscOccuPermit(ElectronicPayment ep)
        {
            Miscellaneous misc = new Miscellaneous();

            misc.MiscType = TaxTypeUtil.MISCELLANEOUS_OCCUPERMIT;
            misc.TaxpayersName = ep.BillerInfo2.ToUpper();
            misc.OrderOfPaymentNum = ep.BillerRef.ToUpper();
            misc.ModeOfPayment = ep.ServiceProvider.ToUpper();
            misc.OPATrackingNum = ep.BillerId.ToString();
            misc.AmountToBePaid = ep.AmountDue;
            misc.TransferredAmount = ep.AmountDue;
            misc.ExcessShort = 0;
            misc.PaymentDate = ep.Date;
            misc.Status = TaxStatus.ForPaymentVerification;
            //misc.RequestingParty = ep.BillerInfo3;
            //misc.RequestingParty = "-";

            return misc;
        }

        public static Miscellaneous ConvertToMiscOvrTtmd(ElectronicPayment ep)
        {
            Miscellaneous misc = new Miscellaneous();

            misc.MiscType = TaxTypeUtil.MISCELLANEOUS_OVR;
            misc.TaxpayersName = ep.BillerInfo2.ToUpper();
            misc.OrderOfPaymentNum = ep.BillerRef.ToUpper();
            misc.ModeOfPayment = ep.ServiceProvider.ToUpper();
            misc.OPATrackingNum = ep.BillerId.ToString().ToUpper();
            misc.AmountToBePaid = ep.AmountDue;
            misc.TransferredAmount = ep.AmountDue;
            misc.ExcessShort = 0;
            misc.PaymentDate = ep.Date;
            misc.Status = TaxStatus.ForPaymentVerification;
            //misc.RequestingParty = ep.BillerInfo3;

            return misc;
        }

        public static Miscellaneous ConvertToMiscOvrDpos(ElectronicPayment ep)
        {
            Miscellaneous misc = new Miscellaneous();

            misc.MiscType = TaxTypeUtil.MISCELLANEOUS_OVR;
            misc.TaxpayersName = ep.BillerInfo2.ToUpper();
            misc.OrderOfPaymentNum = ep.BillerRef.ToUpper();
            misc.ModeOfPayment = ep.ServiceProvider.ToUpper();
            misc.OPATrackingNum = ep.BillerId.ToString().ToUpper();
            misc.AmountToBePaid = ep.AmountDue;
            misc.TransferredAmount = ep.AmountDue;
            misc.ExcessShort = 0;
            misc.PaymentDate = ep.Date;
            misc.Status = TaxStatus.ForPaymentVerification;
            //misc.RequestingParty = ep.BillerInfo3;

            return misc;
        }

        public static Miscellaneous ConvertToMiscMarket(ElectronicPayment ep)
        {
            Miscellaneous misc = new Miscellaneous();

            misc.MiscType = TaxTypeUtil.MISCELLANEOUS_MARKET;
            misc.TaxpayersName = ep.BillerInfo1.ToUpper();
            misc.OrderOfPaymentNum = ep.BillerInfo3.ToUpper();
            misc.ModeOfPayment = ep.ServiceProvider.ToUpper();
            misc.OPATrackingNum = ep.BillerRef.ToUpper();
            misc.AmountToBePaid = ep.AmountDue;
            misc.TransferredAmount = ep.AmountDue;
            misc.ExcessShort = 0;
            misc.PaymentDate = ep.Date;
            misc.Status = TaxStatus.ForPaymentVerification;
            misc.RequestingParty = ep.BillerInfo2;

            return misc;
        }

        public static Miscellaneous ConvertToMiscZoning(ElectronicPayment ep)
        {
            Miscellaneous misc = new Miscellaneous();

            misc.MiscType = TaxTypeUtil.MISCELLANEOUS_ZONING;
            misc.TaxpayersName = ep.BillerInfo2.ToUpper();
            misc.OrderOfPaymentNum = ep.BillerRef.ToUpper();
            misc.ModeOfPayment = ep.ServiceProvider.ToUpper();
            misc.OPATrackingNum = ep.BillerId.ToUpper();
            misc.AmountToBePaid = ep.AmountDue;
            misc.TransferredAmount = ep.AmountDue;
            misc.ExcessShort = 0;
            misc.PaymentDate = ep.Date;
            misc.Status = TaxStatus.ForPaymentVerification;
            misc.RequestingParty = ep.BillerInfo3;

            return misc;
        }

        public static Miscellaneous ConvertToMiscLiquor(ElectronicPayment ep)
        {
            Miscellaneous misc = new Miscellaneous();

            misc.MiscType = TaxTypeUtil.MISCELLANEOUS_LIQUOR;
            misc.TaxpayersName = ep.BillerInfo2.ToUpper();
            misc.OrderOfPaymentNum = ep.BillerRef.ToUpper();
            misc.ModeOfPayment = ep.ServiceProvider.ToUpper();
            //misc.OPATrackingNum = ep.BillerId;
            misc.AmountToBePaid = ep.AmountDue;
            misc.TransferredAmount = ep.AmountDue;
            misc.ExcessShort = 0;
            misc.PaymentDate = ep.Date;
            misc.Status = TaxStatus.ForPaymentVerification;
            misc.RequestingParty = ep.BillerInfo3;

            return misc;
        }
    }
}
