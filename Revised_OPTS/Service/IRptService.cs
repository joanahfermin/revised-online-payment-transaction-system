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
        List<Rpt> RetrieveBySearchKeyword(string tdn);

        void Insert(Rpt rpt);
        void Update(Rpt rpt);
    }
}
