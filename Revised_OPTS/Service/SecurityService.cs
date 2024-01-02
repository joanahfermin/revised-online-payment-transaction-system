using Inventory_System.DAL;
using Inventory_System.Exception;
using Inventory_System.Model;
using Microsoft.VisualBasic.ApplicationServices;
using Revised_OPTS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.Service
{
    internal class SecurityService: ISecurityService
    {
        private static UserAccount LOGIN_USER;

        IUserRepository userRepository = RepositoryFactory.Instance.GetUserRepository();

        public void login(string userName, string passWord)
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                UserAccount user = userRepository.FindByUserName(userName);

                if (user == null)
                {
                    throw new RptException("Invalid Username.");
                }

                if (user.PassWord != passWord)
                {
                    throw new RptException("Invalid Password.");
                }
                LOGIN_USER = user;
            }
        }

        public UserAccount getLoginUser()
        {
            return LOGIN_USER;
        }

    }
}
