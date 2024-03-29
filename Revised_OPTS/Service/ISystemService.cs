﻿using Inventory_System.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.Service
{
    internal interface ISystemService
    {
        EmailTemplate GetORUploadTemplate();
        EmailAccount GetEmailAccount();
        int GetGmailPort();
        string GetGmailHost();
        void Update(EmailTemplate email);
    }
}
