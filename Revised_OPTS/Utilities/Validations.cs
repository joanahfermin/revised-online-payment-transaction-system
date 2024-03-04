using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Inventory_System.Utilities
{
    internal class Validations
    {
        public const string NO_RETRIEVED_NAME = "**NO RECORD FOUND IN DATABASE: ITDDFMUDAILY2024 [V_MiscMasterBilling OR V_BusBillingMaster]**";
        public const string NO_RETRIEVED_BUSINESS_NAME = "**NONE BUS. NAME: ITDDFMUDAILY2024 [V_MiscMasterBilling OR V_BusBillingMaster]**";

        public static bool HaveErrors(ErrorProvider ep)
        {
            foreach (Control formControl in ep.ContainerControl.Controls)
                if (formControl is Panel)
                {
                    Panel panel = (Panel)formControl;
                    foreach (Control panelControl in panel.Controls)

                        if (ep.GetError(panelControl) != "")
                            return true;
                }
                else
                {
                    if (ep.GetError(formControl) != "")
                        return true;
                }

            return false;
        }

        private static bool hasExistingError(ErrorProvider ep, Control tb)
        {
            return ep.GetError(tb).Length > 0;
        }

        public static void validateRequired(ErrorProvider ep, Control tb, string propertyName)
        {
            if (hasExistingError(ep, tb))
            {
                return;
            }
            if (tb.Text.Trim() == "")
            {
                ep.SetError(tb, $"{propertyName} is required.");
            }
        }

        public static void validateFormat(ErrorProvider ep, Control tb, string propertyName, string format)
        {
            if (hasExistingError(ep, tb))
            {
                return;
            }

            if (tb.Text.Trim() != "")
            {
                Regex re = new Regex(format);

                if (!re.IsMatch(tb.Text.Trim()))
                {
                    ep.SetError(tb, $"{propertyName} is in wrong format.");
                }
            }
        }

        public static void validateAmountFormat(ErrorProvider ep, Control tb, string propertyName)
        {
            if (hasExistingError(ep, tb))
            {
                return;
            }
            if (!decimal.TryParse(tb.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal value))
            {
                ep.SetError(tb, $"{propertyName} is in the wrong format.");
            }
        }
    }
}
