using Revised_OPTS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revised_OPTS.DAL
{
    internal class BankRepository : BaseRepository<Bank>, IBankRepository
    {
        public List<Bank> GetBanks()
        {
            return dbSet.OrderBy(bank => bank.BankName).ToList();
        }
    }
}
