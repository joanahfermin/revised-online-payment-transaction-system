using Revised_OPTS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revised_OPTS.Service
{
    internal interface IBusinessService
    {
        Business Get(object id);
        List<Business> RetrieveBySearchKeyword(string mpNum);

        void Insert(Business business);
        void Update(Business business);

    }
}
