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
        public UserRepository(DbContext dBContext) : base(dBContext)
        {
        }

        public UserAccount FindByUserName(string userName)
        {
            //return dbSet.Find(userName);
            return dbSet.FirstOrDefault(u => u.UserName == userName);
        }
    }
}
