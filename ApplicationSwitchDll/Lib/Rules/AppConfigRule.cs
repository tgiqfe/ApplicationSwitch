using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSwitch.Lib.Rules
{
    public class AppConfigRule
    {
        public List<AppRuleTemplate> Rules { get; set; }

        public bool NameDuplicateCheck()
        {
            /*
            Rules.GroupBy(x => x.Name.ToLower()).Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList()
                .ForEach(x => Console.WriteLine($"Duplicate Rule Name: {x}"));
            */
            var duplicateNames = Rules.GroupBy(x => x.Name.ToLower()).Where(x => x.Count() > 1);
            if (duplicateNames.Count() > 0)
            {
                return false;
            }
            return true;
        }
    }
}
