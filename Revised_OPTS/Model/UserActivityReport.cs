using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_System.Model
{
    internal class UserActivityReport
    {
        public string DisplayName { get; set; }
        public int EncodedCount { get; set; }
        public int VerifiedCount { get; set; }
        public int ValidatedCount { get; set; }
        public int UploadCount { get; set; }
        public int ReleasedCount { get; set; }

        public int MiscEncodedCount { get; set; }
        public int MiscVerifiedCount { get; set; }
        public int MiscValidatedCount { get; set; }

        //public int MiscUploadCount { get; set; }
        public int MiscReleasedCount { get; set; }


        public int BusinessEncodedCount { get; set; }
        public int BusinessVerifiedCount { get; set; }
        public int BusinessValidatedCount { get; set; }

    }
}
