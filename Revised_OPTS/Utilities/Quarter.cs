using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revised_OPTS.Utilities
{
    internal class Quarter
    {
        public const string FIRST_QUARTER = "1-1";
        public const string FIRSTQUARTER_TO_SECONDQUARTER = "1-2";
        public const string FIRSTQUARTER_TO_THIRDQUARTER = "1-3";
        public const string FULL_YEAR = "1-4";

        public const string SECOND_QUARTER = "2-2";
        public const string SECONDQUARTER_TO_THIRDQUARTER = "2-3";
        public const string SECONDQUARTER_TO_FOURTHDQUARTER = "2-4";

        public const string THIRD_QUARTER = "3-3";
        public const string THIRDQUARTER_TO_FOURTHQUARTER = "3-4";

        public const string FOURTHQUARTER_TO_FOURTHQUARTER = "4-4";

        public static string[] ALL_QUARTER = { "-", FULL_YEAR, FIRST_QUARTER, FIRSTQUARTER_TO_SECONDQUARTER, FIRSTQUARTER_TO_THIRDQUARTER, 
            SECOND_QUARTER, SECONDQUARTER_TO_THIRDQUARTER, SECONDQUARTER_TO_FOURTHDQUARTER, THIRD_QUARTER, THIRDQUARTER_TO_FOURTHQUARTER,
            FOURTHQUARTER_TO_FOURTHQUARTER };


    }
}
