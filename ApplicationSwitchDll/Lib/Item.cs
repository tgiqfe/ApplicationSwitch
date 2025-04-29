using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApplicationSwitch.Lib
{
    internal class Item
    {
        public static string EvacuateDirectory = Path.Combine(Environment.CurrentDirectory, "Evacuate");
        public static bool HiddenEvacuateDirectory = true;

        public static readonly string Hostname = Environment.MachineName;
        public static readonly string Hostname_numText = Regex.Match(Hostname, @"\d*$").Value;
        public static readonly int? Hostname_number = int.TryParse(Hostname_numText, out int n) ? n : null;
        public static readonly string Hostname_baseName = Hostname.Substring(0, Hostname.Length - Hostname_numText.Length);
        public static readonly int Hostname_length = Hostname.Length;
    }
}
