using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.Service
{
    internal interface ISecurityService
    {
        void login(string userName, string passWord);

    }
}
