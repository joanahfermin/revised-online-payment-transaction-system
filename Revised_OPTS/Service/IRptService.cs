using Inventory_System.Model;
using Revised_OPTS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revised_OPTS.Service
{
    internal interface IRptService 
    {
        Rpt Get(object id);
        List<Rpt> GetAll();
        List<Bank> GetAllBanks();
        List<Bank> GetRegularBanks();
        List<Bank> GetElectronicBanks();

        List<Rpt> RetrieveBySearchKeyword(string tdn);
        List<Rpt> RetrieveBySameRefNumAndReqParty(string refNum, string reqParty);
        List<Rpt> RetrieveForORUploadRegular(DateTime date, string validatedBy);
        List<Rpt> RetrieveBySameRefNumInUploadingEpayment(string taxdec);

        void Insert(Rpt rpt);
        void Update(Rpt rpt);
        void ReleaseReceipt(List<Rpt> rptList, string status, string repName, string contactNum, string releaser);
        void UpdateSelectedRecordsStatus(List<Rpt> rpt, string status);
        //void CheckAttachedPicture(List<Rpt> rptStatusList);
        void CheckRevertStatus(List<Rpt> rptStatusList, string selectedStatusInSubMenuItemText);
        void RevertSelectedRecordStatus(List<Rpt> rpt, string selectedStatusInSubMenuItemText);
        void Delete(Rpt rpt);
        void SaveAll(List<Rpt> rpt, List<Rpt> rptToDelete, decimal totalAmountTransferred);
        void SaveAllEPayment(List<Rpt> rpt, List<Business> bussiness, List<Miscellaneous> misc);
        void UpdateAllinDuplicateRecordForm(List<Rpt> listOfRptsToSave);

        void UploadReceipt(RPTAttachPicture pix);
        RPTAttachPicture getRptReceipt(long rptId);
        void DeleteAttachedOR(long rptId);

        List<AllTaxTypeReport> RetrieveByValidatedDate(DateTime dateFrom, DateTime dateTo);

    }
}
