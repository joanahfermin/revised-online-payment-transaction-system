using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace Inventory_System.Utilities
{
    internal class DeletedRecordBoolToIntConverter : ValueConverter<int, bool>
    {
        public DeletedRecordBoolToIntConverter(ConverterMappingHints mappingHints = null)
        : base(
              v => ConvertToDatabase(v),
              v => ConvertFromDatabase(v),
              mappingHints)
        {
        }

        private static bool ConvertToDatabase(int value)
        {
            return value == 1;
            
        }

        private static int ConvertFromDatabase(bool value)
        {
            return value ? 1 : 0;
        }
    }
}
