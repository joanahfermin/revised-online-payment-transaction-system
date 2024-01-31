using Revised_OPTS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.Exception
{
    internal class RptDuplicateRecordException : RptException
    {
        public List<Rpt> duplicateRptList = null; 

        public RptDuplicateRecordException(string message) : base(message)
        {
        }
        public RptDuplicateRecordException(string message, List<Rpt> DuplicateRptList) : base(message)
        {
            duplicateRptList = DuplicateRptList;
        }
    }
}
