using Microsoft.Extensions.Configuration;
using System;

namespace Gaspra.Logging.Providers.Fluentd.Static
{
    public static class ConfigurationReaderExtensions
    {
        public static string GetString(this IConfigurationSection config, string item, bool hasDefault = false, string defaultsTo = default)
        {
            return config.GetItem(item, hasDefault, defaultsTo);
        }

        public static int GetInt(this IConfigurationSection config, string item, bool hasDefault = false, int defaultsTo = default)
        {
            return config.GetItem(item, hasDefault, defaultsTo);
        }

        public static T GetItem<T>(this IConfigurationSection config, string item, bool hasDefault = false, T defaultsTo = default)
        {
            try
            {
                if (config[item] != null)
                {
                    var value = (T)Convert.ChangeType(config[item], typeof(T));
                    return value;
                }
            }
            catch { }

            if (hasDefault)
            {
                return defaultsTo;
            }

            throw new ArgumentException($"Unable to get requested item: `{item}` from config section: `{config.Key}`", nameof(item));
        }
    }
}
