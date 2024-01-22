using Inventory_System.Model;
using Inventory_System.Utilities;
using Revised_OPTS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.DAL
{
    internal class RPTAttachPictureRepository : BaseRepository<RPTAttachPicture>, IRPTAttachPictureRepository
    {
        public RPTAttachPicture getRptReceipt(long rptId)
        {
            return getDbSet().FirstOrDefault(p => p.RptId == rptId && p.DocumentType == DocumentType.RECEIPT);
        }
    }
}
