using ApplicationSwitch.Lib.Yml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace ApplicationSwitch.Lib
{
    internal class Functions
    {
        #region Enable/Disable Check

        private readonly static string[] candidate_enable =
            new string[] { "Enable", "enable", "on", "true", "1", "true", "tru", "$true" };
        private readonly static string[] candidate_disable =
            new string[] { "Disable", "disable", "off", "false", "0", "false", "fals", "$false" };

        /// <summary>
        /// bool text -> bool. (enable check).
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsEnable(string text)
        {
            return candidate_enable.Any(x => x.Equals(text, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// bool text -> bool. (disable check).
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsDisable(string text)
        {
            return candidate_disable.Any(x => x.Equals(text, StringComparison.OrdinalIgnoreCase));
        }

        #endregion
        #region Load / Save YAML

        /// <summary>
        /// Load yaml file and deserialize to objcet.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T Load<T>(string path) where T : new()
        {
            try
            {
                var ret = UtfUnknown.CharsetDetector.DetectFromFile(path).Detected;
                var encoding = ret.Encoding;
                return new DeserializerBuilder().
                    WithCaseInsensitivePropertyMatching().
                    IgnoreUnmatchedProperties().
                    Build().
                    Deserialize<T>(File.ReadAllText(path, encoding));
            }
            catch { }
            return new T();
        }

        /// <summary>
        /// Save serialize to yaml file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="obj"></param>
        public static void Save(string path, object obj)
        {
            using (var sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                new SerializerBuilder().
                    WithEventEmitter(x => new MultilineScalarFlowStyleEmitter(x)).
                    WithEmissionPhaseObjectGraphVisitor(x => new YamlIEnumerableSkipEmptyObjectGraphVisitor(x.InnerVisitor)).
                    Build().
                    Serialize(sw, obj);
            }
        }

        /// <summary>
        /// Show object to console.
        /// </summary>
        /// <param name="obj"></param>
        public static void Show(object obj)
        {
            new SerializerBuilder().
                WithEventEmitter(x => new MultilineScalarFlowStyleEmitter(x)).
                WithEmissionPhaseObjectGraphVisitor(x => new YamlIEnumerableSkipEmptyObjectGraphVisitor(x.InnerVisitor)).
                Build().
                Serialize(Console.Out, obj);
        }

        public static string ToText(object obj)
        {
            return new SerializerBuilder().
                WithEventEmitter(x => new MultilineScalarFlowStyleEmitter(x)).
                WithEmissionPhaseObjectGraphVisitor(x => new YamlIEnumerableSkipEmptyObjectGraphVisitor(x.InnerVisitor)).
                Build().
                Serialize(obj);
        }

        #endregion

        public static string ExpandEnvironmentText(string text)
        {
            string retText = text;
            if (text.Contains("%"))
            {
                retText = Environment.ExpandEnvironmentVariables(retText);
            }

            return retText;
        }
    }
}
