using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Gaspra.Logging.Serializer.Extensions
{
    public static class ConfigurationReaderExtensions
    {
        public static bool TryGetItem(this IConfigurationSection config, string key, out string value)
        {
            value = "";

            if (config[key] != null)
            {
                value = config[key];
                return true;
            }

            return false;
        }

        public static bool TryGetSection(this IConfiguration config, string key, out IConfigurationSection section)
        {
            section = config.GetSection(key);
            return section.Exists();
        }

        public static IDictionary<string, string> AddSection(this IDictionary<string, string> dictionary, IConfigurationSection section)
        {
            if (section.Exists())
            {
                dictionary.RecurseSection(section, section.Path);
            }

            return dictionary;
        }

        private static IDictionary<string, string> RecurseSection(this IDictionary<string, string> dictionary, IConfigurationSection section, string root)
        {
            foreach (var child in section.GetChildren())
            {
                var deepChildren = child.GetChildren();

                if (deepChildren.Any())
                {
                    dictionary.RecurseSection(child, root);
                }

                if (!string.IsNullOrWhiteSpace(child.Value))
                    dictionary.AddOrUpdateItem(child.Path.DeriveKey(root), child.Value);
            }

            return dictionary;
        }

        private static void AddOrUpdateItem(this IDictionary<string, string> dictionary, string key, string value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        private static string DeriveKey(this string path, string root)
        {
            var key = string.Join(".", path
                .Replace($"{root}:", "")
                .Split(':')
                );

            return key;
        }
    }
}
