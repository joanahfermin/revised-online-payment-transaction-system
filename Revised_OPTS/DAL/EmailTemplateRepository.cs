using Inventory_System.Model;
using Revised_OPTS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.DAL
{
    internal class EmailTemplateRepository : BaseRepository<EmailTemplate>, IEmailTemplateRepository
    {
        public EmailTemplate GetORUploadTemplate()
        {
            return getDbSet().Where(et => et.isReceipt == true && et.Deleted != 1).First();
        }
    }
}
