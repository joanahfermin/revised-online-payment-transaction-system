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

        IEmailAccountRepository emailAccountRepository = RepositoryFactory.Instance.GetEmailAccountRepository();

        ISystemSettingRepository systemSettingRepository = RepositoryFactory.Instance.GetSystemSettingRepository();


        public EmailTemplate GetORUploadTemplate()
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                return emailTemplateRepository.GetORUploadTemplate();
            }
        }

        public EmailAccount GetEmailAccount()
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                return emailAccountRepository.GetEmailAccount();
            }
        }

        public int GetGmailPort()
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                return Int32.Parse(systemSettingRepository.SelectBySettingName("GMAIL_PORT").SettingValue);
            }
        }

        public string GetGmailHost()
        {
            using (var dbContext = ApplicationDBContext.Create())
            {
                return systemSettingRepository.SelectBySettingName("GMAIL_HOST").SettingValue;
            }
        }

    }
}
