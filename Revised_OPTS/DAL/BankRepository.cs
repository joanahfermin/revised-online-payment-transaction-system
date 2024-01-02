using Microsoft.EntityFrameworkCore;
using Revised_OPTS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revised_OPTS.DAL
{
    /// <summary>
    /// BankRepository class is the implementation of the IBankRepository interface. 
    /// BankRepository class inherits from its' parent class: BaseRepository and IBankRepository interface.
    /// </summary>
    internal class BankRepository : BaseRepository<Bank>, IBankRepository
    {
        /// <summary>
        /// Returns the bank name of from the Bank table.
        /// </summary>
        /// <returns></returns>
        public List<Bank> GetBanks()
        {
            return getDbSet().OrderBy(bank => bank.BankName).ToList();
        }
    }
}
