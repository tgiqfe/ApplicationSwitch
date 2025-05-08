using System.Runtime.InteropServices;

namespace ApplicationSwitch.Lib.Yml
{
    internal class NaturalComparer : IComparer<string>
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        public static extern int StrCmpLogicalW(string psz1, string psz2);

        public int Compare(string x, string y)
        {
            return StrCmpLogicalW(x, y);
        }
    }
}
