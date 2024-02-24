using Inventory_System.DAL;
using Inventory_System.Model;
using Revised_OPTS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.Service
{
    internal class SystemService : ISystemService
    {
        IEmailTemplateRepository emailTemplateRepository = RepositoryFactory.Instance.GetEmailTemplateRepository();

        public EmailTemplate GetORUploadTemplate()
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                return emailTemplateRepository.GetORUploadTemplate();
            }
        }
    }
}
