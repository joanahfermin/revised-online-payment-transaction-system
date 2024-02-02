using Revised_OPTS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.Exception
{
    internal class DuplicateRecordException : RptException
    {
        public List<Rpt> duplicateRptList = null;
        public List<Business> duplicateBusList = null;
        public List<Miscellaneous> duplicateMiscList = null;

        public DuplicateRecordException(string message) : base(message)
        {
        }

        public DuplicateRecordException(string message, List<Rpt> DuplicateRptList, List<Business> DuplicateBusList, List<Miscellaneous> DuplicateMiscList) : base(message)
        {
            duplicateRptList = DuplicateRptList;
            duplicateBusList = DuplicateBusList;
            duplicateMiscList = DuplicateMiscList;
        }

        public DuplicateRecordException(string message, List<Rpt> DuplicateRptList) : base(message)
        {
            duplicateRptList = DuplicateRptList;
        }

        public DuplicateRecordException(string message, List<Business> DuplicateBusList) : base(message)
        {
            duplicateBusList = DuplicateBusList;
        }

        public DuplicateRecordException(string message, List<Miscellaneous> DuplicateMiscList) : base(message)
        {
            duplicateMiscList = DuplicateMiscList;
        }

    }
}
