using Inventory_System.Model;
using Revised_OPTS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revised_OPTS.DAL
{
    internal interface IRptRepository : IRepository<Rpt>
    {
        List<Rpt> retrieveBySearchKeyword(string tdn);
        List<Rpt> retrieveBySearchKeywordForPaymentValidationOnly(string tdn);

        List<Rpt> retrieveBySameRefNumAndReqParty(string refNum, string reqParty);
        List<Rpt> retrieveBySameRefNum(string refNum);
        List<Rpt> checkExistingRecord(Rpt rpt);
        List<Rpt> RetrieveForORUploadRegular(DateTime date, string validatedBy);
        List<Rpt> RetrieveBySameRefNumInUploadingEpayment(string refNum);
        List<Rpt> RetrieveNoName();

        int CoundForORUploadWithPhoto(List<long> rptIDList);
        void ConfirmSendOrUpload(List<long> rptIDList);
        void UpdateStatus(List<long> rptIDList, string Status, string Displayname, DateTime Date);
        int CountORUploadRemainingToSend(string uploadedBy);
        List<Rpt> ListORUploadRemainingToSend(string uploadedBy);

        List<Rpt> ListForLocationCodeAssignment(string locationCode);
        void AssignmentLocationCode(List<long> rptIDList, string locationCode);
    }
}
