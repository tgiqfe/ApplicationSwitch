using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApplicationSwitch.Test
{
    internal class sample_AppConfig003
    {
        public static void Test01()
        {
            string[] array = new string[] { "test*", "*test1", "testaa*2", "test3*" };
            Console.WriteLine(WildcardMatch("test", array[0]));
            Console.WriteLine(WildcardMatch("test1", array[1]));
            Console.WriteLine(WildcardMatch("testaaaaaaaaaa2", array[2]));
            Console.WriteLine(WildcardMatch("test3", array[3]));
        }

        public static bool WildcardMatch(string input, string word)
        {
            return new Regex(word.Replace("*", ".*"), RegexOptions.IgnoreCase).IsMatch(input);
        }

    }
}
