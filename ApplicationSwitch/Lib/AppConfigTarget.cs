using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApplicationSwitch.Lib
{
    internal class AppConfigTarget
    {
        public string Enable { get; set; }
        public string Disable { get; set; }



        private bool WildcardMatch(string input, string word)
        {
            string patternStrign = Regex.Replace(word, ".",
                x =>
                {
                    return x.Value switch
                    {
                        string s when s == "?" => ".",
                        string s when s == "*" => ".*",
                        _ => Regex.Escape(x.Value)
                    };
                });
            if (!patternStrign.StartsWith("*")) patternStrign = "^" + patternStrign;
            if (!patternStrign.EndsWith("*")) patternStrign = patternStrign + "$";
            var reg = new Regex(patternStrign, RegexOptions.IgnoreCase);
            return reg.IsMatch(input);
        }
    }
}
