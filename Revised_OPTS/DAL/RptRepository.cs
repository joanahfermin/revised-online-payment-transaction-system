using Inventory_System.Forms;
using Inventory_System.Model;
using Inventory_System.Service;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Revised_OPTS.Model;
using Revised_OPTS.Service;
using Revised_OPTS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revised_OPTS.DAL
{
    internal class RptRepository : BaseRepository<Rpt>, IRptRepository
    {
        private IBankRepository bankRepository = RepositoryFactory.Instance.GetBankRepository();
        ISecurityService securityService = ServiceFactory.Instance.GetSecurityService();

        public List<Rpt> checkExistingRecord(Rpt rpt)
        {
            return getDbSet()
                    .Where(jo => jo.TaxDec == rpt.TaxDec
                        && jo.YearQuarter == rpt.YearQuarter
                        && jo.Quarter == rpt.Quarter
                        && jo.DeletedRecord != 1
                        && jo.DuplicateRecord == 0
                        && jo.BillingSelection == rpt.BillingSelection
                        && jo.RptID != rpt.RptID)
                    .ToList();
        }

        public List<Rpt> retrieveBySameRefNumAndReqParty(string refNum, string reqParty)
        {
            return getDbSet().Where(j => j.RefNum == refNum && j.DeletedRecord != 1 && j.RequestingParty == reqParty)
            .OrderBy(j => j.RptID).ThenBy(j => j.TaxDec).ToList();
        }

        public List<Rpt> retrieveBySameRefNum(string refNum)
        {
            return getDbSet().Where(j => j.RefNum == refNum && j.DeletedRecord != 1)
            .OrderBy(j => j.RptID).ToList();
        }

        public List<Rpt> RetrieveBySameRefNumInUploadingEpayment(string taxdec)
        {
            return getDbSet().FromSqlRaw<Rpt>(
                ///*and Bank in (Select BankName from Jo_RPT_Banks where isEbank=1)*/
                $"SELECT *FROM Jo_RPT where TaxDec LIKE @TaxDec and DeletedRecord != 1 and Status = 'FOR O.R UPLOAD' and SendReceiptReady = 0 UNION " +
                $"SELECT *FROM Jo_RPT where RefNum in (select RefNum FROM Jo_RPT where TaxDec LIKE @TaxDec) and DeletedRecord != 1 and Status = 'FOR O.R UPLOAD' and SendReceiptReady = 0 " +
                $"order by RefNum desc, EncodedDate asc", new[] { new SqlParameter("@TaxDec", "%" + taxdec + "%") }).ToList();
        }
            /** return getDbSet()
                //SELECT * FROM Jo_RPT where TaxDec LIKE @TaxDec and DeletedRecord != 1
                .Where(j => j.TaxDec.Contains(taxdec) && j.DeletedRecord != 1)
                //UNION
                .Union(
                    //SELECT *FROM Jo_RPT where RefNum
                    getDbSet()
                        .Where(j => getDbSet()
                            // (select RefNum FROM Jo_RPT where TaxDec LIKE @TaxDec)
                            .Where(subJ => subJ.TaxDec.Contains(taxdec))
                            .Select(subJ => subJ.RefNum)
                            //RefNum in (select RefNum
                            .Contains(j.RefNum)
                            //and DeletedRecord != 1  AND RefNum IS NOT NULL AND RefNum != ''
                            && j.DeletedRecord != 1 && j.RefNum != null && j.RefNum != "" && j.Status == "FOR O.R UPLOAD")
                )
                //order by RefNum desc, EncodedDate asc
                .OrderByDescending(j => j.RefNum)
                .ThenBy(j => j.EncodedDate)
                .ToList(); 
            **/
        //}

        public List<Rpt> RetrieveForORUploadRegular(DateTime date, string validatedBy)
        {
            List<string> regularBanks = bankRepository.GetRegularBanks().Select(p => p.BankName).ToList();

            return getDbSet().Where(rpt =>
                    rpt.ValidatedDate.HasValue &&
                    rpt.ValidatedDate.Value.Date == date.Date &&
                    regularBanks.Contains(rpt.Bank) &&
                    rpt.ValidatedBy == validatedBy &&
                    rpt.SendReceiptReady == false
                ).OrderBy(rpt => rpt.ValidatedDate).ToList();
        }

        public int CoundForORUploadWithPhoto(List<long> rptIDList)
        {
            var query = from rpt in getContext().Rpts
                        where rpt.DeletedRecord != 1
                              && rpt.Status == "FOR O.R UPLOAD"
                              && getContext().rptPictures.Any(p => p.RptId == rpt.RptID)
                              && rptIDList.Contains(rpt.RptID)
                        select rpt;
            return query.Count();
        }

        public List<Rpt> retrieveBySearchKeyword(string tdn)
        {
            return getDbSet()
                //SELECT * FROM Jo_RPT where TaxDec LIKE @TaxDec and DeletedRecord != 1
                .Where(j => j.TaxDec.Contains(tdn) && j.DeletedRecord != 1)
                //UNION
                .Union(
                    //SELECT *FROM Jo_RPT where RefNum
                    getDbSet()
                        .Where(j => getDbSet()
                            // (select RefNum FROM Jo_RPT where TaxDec LIKE @TaxDec)
                            .Where(subJ => subJ.TaxDec.Contains(tdn))
                            .Select(subJ => subJ.RefNum)
                            //RefNum in (select RefNum
                            .Contains(j.RefNum)
                            //and DeletedRecord != 1  AND RefNum IS NOT NULL AND RefNum != ''
                            && j.DeletedRecord != 1 && j.RefNum != null && j.RefNum != "")
                )
                //order by RefNum desc, EncodedDate asc
                .OrderByDescending(j => j.RefNum)
                .ThenBy(j => j.EncodedDate)
                .ToList();

            //var parameterValue = new SqlParameter("@TaxDec", tdn);

            //var sql = "SELECT * FROM Jo_RPT where TaxDec LIKE @TaxDec and DeletedRecord != 1 " +
            //          "UNION SELECT *FROM Jo_RPT where RefNum in (select RefNum FROM Jo_RPT where TaxDec LIKE @TaxDec) " +
            //          "and DeletedRecord != 1  AND RefNum IS NOT NULL AND RefNum != '' order by RefNum desc, EncodedDate asc";

            //return  getDbSet().FromSqlRaw(sql, parameterValue).ToList();
        }

        public void ConfirmSendOrUpload(List<long> rptIDList)
        {
            var query = from rpt in getContext().Rpts
                        where rpt.DeletedRecord != 1
                              && rpt.Status == "FOR O.R UPLOAD"
                              && getContext().rptPictures.Any(p => p.RptId == rpt.RptID)
                              && rptIDList.Contains(rpt.RptID)
                        select rpt;
            var rptList = query.ToList();

            foreach (var rpt in rptList)
            {
                rpt.SendReceiptReady = true;
            }
        }

        public int CountORUploadRemainingToSend(string uploadedBy)
        {
            var query = from rpt in getDbSet()
                        where rpt.DeletedRecord != 1
                              && rpt.Status == "FOR O.R UPLOAD"
                              && rpt.SendReceiptReady == true
                              && rpt.UploadedBy == uploadedBy
                        select rpt;
            return query.Count();
        }

        public List<Rpt> ListORUploadRemainingToSend(string uploadedBy)
        {
            var query = (from rpt in getDbSet()
                        where rpt.DeletedRecord != 1
                              && rpt.Status == "FOR O.R UPLOAD"
                              && rpt.SendReceiptReady == true
                              && rpt.UploadedBy == uploadedBy
                         orderby rpt.ORConfirmDate ascending, rpt.ORAttachedDate ascending
                         select rpt).Take(5); ;
            return query.ToList();
        }

        public List<Rpt> ListForLocationCodeAssignment(string locationCode)
        {
            var query = (from rpt in getDbSet()
                         where rpt.DeletedRecord != 1
                               && (
                                    (rpt.Status == TaxStatus.ForORUpload && rpt.UploadedBy != null) ||
                                    (rpt.Status == TaxStatus.ForORPickup)
                               )
                               && 
                               (  (locationCode.Trim().Length == 0 && rpt.LocCode == null) || (locationCode.Trim().Length>0 && rpt.LocCode == locationCode))
                         orderby rpt.ORConfirmDate ascending, rpt.ORAttachedDate ascending
                         select rpt);
            return query.ToList();
        }
        public void AssignmentLocationCode(List<long> rptIDList, string locationCode)
        {
            var rptEntitiesToUpdate = getDbSet().Where(r => rptIDList.Contains(r.RptID)).ToList();

            // Update LocationCode for fetched entities
            foreach (var rpt in rptEntitiesToUpdate)
            {
                rpt.LocCode = locationCode;
            }
        }
    }
}
