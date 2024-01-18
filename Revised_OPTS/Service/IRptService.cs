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
        void UpdateSelectedRecordsStatus(List<Rpt> rpt);
        void RevertSelectedRecordStatus(Rpt rpt);
        void Delete(Rpt rpt);
        void SaveAll(List<Rpt> rpt, List<Rpt> rptToDelte, decimal totalAmountTransferred);

        void UploadReceipt(RPTAttachPicture pix);
        RPTAttachPicture getRptReceipt(long rptId);
    }
}
