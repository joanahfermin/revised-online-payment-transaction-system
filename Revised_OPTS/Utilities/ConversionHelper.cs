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
            rpt.AmountToPay = ep.AmountTransferred;
            rpt.AmountTransferred = ep.AmountTransferred;
            rpt.TotalAmountTransferred = ep.AmountTransferred;
            rpt.ExcessShortAmount = 0;// excessShortAmount;
            rpt.Bank = ep.ServiceProvider;
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
            bus.TaxpayersName = ep.BillerInfo2;
            bus.BusinessName = null;
            bus.BillNumber = ep.BillerRef;
            bus.BillAmount = ep.AmountTransferred;
            bus.BillAmount = ep.AmountTransferred;
            bus.PaymentChannel = ep.ServiceProvider;
            //bus.Year = ep.BillerInfo1;
            //bus.Qtrs = ep.Quarter;
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
            misc.OrderOfPaymentNum = ep.BillerRef;
            misc.ModeOfPayment = ep.ServiceProvider;
            misc.OPATrackingNum = ep.BillerId.ToString();
            misc.AmountToBePaid = ep.AmountTransferred;
            misc.TransferredAmount = ep.AmountTransferred;
            misc.ExcessShort = 0;
            misc.PaymentDate = ep.Date;
            misc.Status = TaxStatus.ForPaymentVerification;
            misc.RequestingParty = ep.BillerInfo3;

            return misc;
        }

        public static Miscellaneous ConvertToMiscOvrTtmd(ElectronicPayment ep)
        {
            Miscellaneous misc = new Miscellaneous();

            misc.MiscType = TaxTypeUtil.MISCELLANEOUS_OVR;
            misc.TaxpayersName = ep.BillerInfo2;
            misc.OrderOfPaymentNum = ep.BillerRef;
            misc.ModeOfPayment = ep.ServiceProvider;
            misc.OPATrackingNum = ep.BillerId.ToString();
            misc.AmountToBePaid = ep.AmountTransferred;
            misc.TransferredAmount = ep.AmountTransferred;
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
            misc.TaxpayersName = ep.BillerInfo2;
            misc.OrderOfPaymentNum = ep.BillerRef;
            misc.ModeOfPayment = ep.ServiceProvider;
            misc.OPATrackingNum = ep.BillerId.ToString();
            misc.AmountToBePaid = ep.AmountTransferred;
            misc.TransferredAmount = ep.AmountTransferred;
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
            misc.OrderOfPaymentNum = ep.BillerInfo3;
            misc.ModeOfPayment = ep.ServiceProvider;
            misc.OPATrackingNum = ep.BillerRef;
            misc.AmountToBePaid = ep.AmountTransferred;
            misc.TransferredAmount = ep.AmountTransferred;
            misc.ExcessShort = 0;
            misc.PaymentDate = ep.Date;
            misc.Status = TaxStatus.ForPaymentVerification;
            misc.RequestingParty = ep.BillerInfo2;

            return misc;
        }
        public static Miscellaneous ConvertToMiscZoning(ElectronicPayment ep)
        {
            Miscellaneous misc = new Miscellaneous();

            misc.MiscType = TaxTypeUtil.MISCELLANEOUS_MARKET;
            misc.TaxpayersName = ep.BillerInfo2;
            misc.OrderOfPaymentNum = ep.BillerRef;
            misc.ModeOfPayment = ep.ServiceProvider;
            //misc.OPATrackingNum = ep.BillerId.ToString();
            misc.AmountToBePaid = ep.AmountTransferred;
            misc.TransferredAmount = ep.AmountTransferred;
            misc.ExcessShort = 0;
            misc.PaymentDate = ep.Date;
            misc.Status = TaxStatus.ForPaymentVerification;
            misc.RequestingParty = ep.BillerInfo3;

            return misc;
        }


    }
}
