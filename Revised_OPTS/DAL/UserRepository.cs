using Inventory_System.Model;
using Microsoft.EntityFrameworkCore;
using Revised_OPTS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.DAL
{
    internal class UserRepository: BaseRepository<UserAccount>, IUserRepository
    {

        public UserAccount FindByUserName(string userName)
        {
            return getDbSet().FirstOrDefault(u => u.UserName == userName);
        }

        public List<UserAccount> GetValidators()
        {
            return getDbSet().Where(u => u.isValidator == true).OrderBy(u => u.DisplayName).ToList();
        }
    }
}
