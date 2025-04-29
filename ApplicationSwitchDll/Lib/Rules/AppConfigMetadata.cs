using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApplicationSwitch.Lib.Rules
{
    public class AppConfigMetadata
    {
        #region Parameter

        /// <summary>
        /// Rule file name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Version
        /// </summary>
        public string Version { get; set; }

        #endregion

        private static readonly Version _currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
        private static readonly Regex _pattern_version = new Regex(@"^(\d+\.){0,3}\d+$");

        /// <summary>
        /// Version check.
        ///   Major: mismatch fail
        ///   Minor: mismatch fail
        ///   Build: [current version >= this.Version] success
        ///   Revision: [match or mismatch] success
        /// </summary>
        /// <returns></returns>
        public bool IsValidVersion()
        {
            if (!string.IsNullOrEmpty(this.Version) && _pattern_version.IsMatch(this.Version))
            {
                var array = this.Version.Split('.').Select(x => int.Parse(x)).ToArray();
                if (array.Length == 1)
                {
                    return _currentVersion.Major == array[0];
                }
                else if (array.Length == 2)
                {
                    return _currentVersion.Major == array[0] && _currentVersion.Minor == array[1];
                }
                else if (array.Length >= 3)
                {
                    return _currentVersion.Major == array[0] && _currentVersion.Minor == array[1] && _currentVersion.Build >= array[2];
                }
            }
            return true;
        }
    }
}
