using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.Exception
{
    internal class RptException : IOException
    {
        public RptException(string message) : base(message)
        {

        }
    }
}
