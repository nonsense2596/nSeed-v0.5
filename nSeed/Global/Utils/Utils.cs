using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace nSeed.Global.Utils
{
    public static class Utils
    {
        public static IEnumerable<KeyValuePair<string, string>> ToEnumerable(this NameValueCollection collection, bool duplicateKeysIfMulti = false)
        {
            foreach (string key in collection.Keys)
            {
                var value = collection[key];
                if (value == null) value = "";  // fix for tags
                if (duplicateKeysIfMulti)
                    foreach (var val in value.Split(','))
                        yield return new KeyValuePair<string, string>(key, val);
                else
                    yield return new KeyValuePair<string, string>(key, value);
            }
        }
    }
}
