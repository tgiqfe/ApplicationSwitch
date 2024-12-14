using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core;
using YamlDotNet.Serialization.ObjectGraphVisitors;
using YamlDotNet.Serialization;


namespace ProfileList2.Lib.ScriptLanguage.Yml
{
    public class YamlIEnumerableSkipEmptyObjectGraphVisitor : ChainedObjectGraphVisitor
    {
        public YamlIEnumerableSkipEmptyObjectGraphVisitor(IObjectGraphVisitor<IEmitter> nextVisitor) : base(nextVisitor)
        {
        }

        public override bool EnterMapping(IObjectDescriptor key, IObjectDescriptor value, IEmitter context, ObjectSerializer serializer)
        {
            var retVal = false;

            if (value.Value == null)
            {
                return false;
            }

            if (value.Value is IEnumerable enumerableObject)
            {
                if (enumerableObject.GetEnumerator().MoveNext())
                {
                    retVal = base.EnterMapping(key, value, context, serializer);
                }
            }
            else
            {
                retVal = base.EnterMapping(key, value, context, serializer);
            }

            return retVal;
        }
    }
}
