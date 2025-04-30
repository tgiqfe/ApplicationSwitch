using System.Text.RegularExpressions;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ApplicationSwitch.Lib
{
    internal class Item
    {
        public static string EvacuateDirectory = Path.Combine(Environment.CurrentDirectory, "Evacuate");
        public static bool HiddenEvacuateDirectory = true;

        #region Hostname parts

        public static readonly string Hostname = Environment.MachineName;
        public static readonly string Hostname_numText = Regex.Match(Hostname, @"\d*$").Value;
        public static readonly int? Hostname_number = _ExpandNumber(Hostname_numText);
        public static readonly string Hostname_baseName = Hostname.Substring(0, Hostname.Length - Hostname_numText.Length);
        public static readonly int Hostname_length = Hostname.Length;

        private static int? _ExpandNumber(string num)
        {
            if (int.TryParse(num, out int n))
            {
                return n;
            }
            return null;
        }

        #endregion
    }
}
