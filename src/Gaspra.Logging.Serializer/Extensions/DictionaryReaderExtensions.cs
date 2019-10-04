using System;
using System.Collections.Generic;
using System.Text;

namespace Gaspra.Logging.Serializer.Extensions
{
    public static class DictionaryReaderExtensions
    {
        public static string TryGet(this IDictionary<string, object> dictionary, string key, bool throwException = false)
        {
            return TryGet<string>(dictionary, key, throwException);
        }

        public static T TryGet<T>(this IDictionary<string, object> dictionary, string key, bool throwException = false)
        {
            if (dictionary.ContainsKey(key))
            {
                try
                {
                    var value = (T)Convert.ChangeType(dictionary[key], typeof(T));

                    return value;
                }
                catch (Exception ex)
                {
                    /*
                        Was unable to get the value only throw an exception
                        if the throwException param is true, otherwise return default
                    */

                    if (throwException)
                        throw new Exception($"Was unable to get {key} from {nameof(dictionary)} -> {ex.Message}", ex);
                }
            }
            return default;
        }
    }
}
