using ApplicationSwitch.Lib;
using ApplicationSwitch.Lib.Yml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace ApplicationSwitch
{
    internal class DataSerializer
    {
        public static T Load<T>(string path) where T : new()
        {
            try
            {
                return new Deserializer().Deserialize<T>(File.ReadAllText(path));
            }
            catch { }
            return new T();
        }

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

        public static void Show(object obj)
        {
            new SerializerBuilder().
                WithEventEmitter(x => new MultilineScalarFlowStyleEmitter(x)).
                WithEmissionPhaseObjectGraphVisitor(x => new YamlIEnumerableSkipEmptyObjectGraphVisitor(x.InnerVisitor)).
                Build().
                Serialize(Console.Out, obj);
        }
    }
}
