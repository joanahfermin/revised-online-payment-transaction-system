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
        List<Miscellaneous> RetrieveBySearchKeyword(string opNum);
    }
}
