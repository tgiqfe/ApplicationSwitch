using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSwitch
{
    internal class Functions
    {
        #region Enable/Disable Check

        private readonly static string[] candidate_enable =
            new string[] { "Enable", "enable", "on", "true", "1", "true", "tru", "$true" };
        private readonly static string[] candidate_disable =
            new string[] { "Disable", "disable", "off", "false", "0", "false", "fals", "$false" };

        public static bool IsEnable(string text)
        {
            return candidate_enable.Any(x => x.Equals(text, StringComparison.OrdinalIgnoreCase));
        }

        public static bool IsDisable(string text)
        {
            return candidate_disable.Any(x => x.Equals(text, StringComparison.OrdinalIgnoreCase));
        }

        #endregion
    }
}
