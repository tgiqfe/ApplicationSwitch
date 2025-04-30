using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSwitch.Lib.Rules
{
    public class AppConfigRule
    {
        #region Parameter

        public List<AppRuleTemplate> Rules { get; set; }

        #endregion

        /// <summary>
        /// Parameter check
        /// </summary>
        /// <returns></returns>
        public bool IsParameterAll()
        {
            return Rules.Any(x => !string.IsNullOrEmpty(x.Action) && !string.IsNullOrEmpty(x.Name));
        }

        /// <summary>
        /// Duplicate check
        /// </summary>
        /// <returns></returns>
        public bool IsDuplicateName()
        {
            //  Duplicat name log output. (memo)
            /*
            Rules.GroupBy(x => x.Name.ToLower()).Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList()
                .ForEach(x => Console.WriteLine($"Duplicate Rule Name: {x}"));
            */

            return !(Rules.GroupBy(x => x.Name.ToLower()).Any(x => x.Count() > 1));
        }
    }
}
