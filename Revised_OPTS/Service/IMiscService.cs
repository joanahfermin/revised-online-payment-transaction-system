using Revised_OPTS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revised_OPTS.Service
{
    internal interface IMiscService
    {
        Miscellaneous Get(object id);
        List<Miscellaneous> RetrieveBySearchKeyword(string opNum);
        List<Miscellaneous> RetrieveBySearchKeywordForPaymentValidationOnly(string opNum);


        void Insert(List<Miscellaneous> misc);
        void Insert(List<Miscellaneous> misc, bool validate);
        void Update(Miscellaneous misc);

        void RevertSelectedRecordStatus(List<Miscellaneous> misc);
        void UpdateSelectedRecordsStatus(List<Miscellaneous> miscList, string status);

        void validateMiscDuplicateRecord(List<Miscellaneous> listOfMiscToSave);
        List<Miscellaneous> RetrieveBySameRefNum(string refNum);
        void DeleteAll(List<Miscellaneous> miscToDelete);
    }
}
