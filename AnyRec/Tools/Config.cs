using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
namespace AnyRec.Tools
{
    internal class Config
    {
        private static readonly string ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
        private static Dictionary<string, JToken> Settings = new Dictionary<string, JToken>();
        static Config()
        {
            Load();
        }
        public static void Load()
        {
            if (!File.Exists(ConfigPath))
            {
                Settings = new Dictionary<string, JToken>();
                Save();
                return;
            }
            try
            {
                string json = File.ReadAllText(ConfigPath);
                Settings = JsonConvert.DeserializeObject<Dictionary<string, JToken>>(json) ?? new Dictionary<string, JToken>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load config: {ex.Message}");
                Settings = new Dictionary<string, JToken>();
            }
        }
        public static void Save()
        {
            try
            {
                string json = JsonConvert.SerializeObject(Settings, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(ConfigPath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save config: {ex.Message}");
            }
        }
        public static void Set(string key, bool value)
        {
            Settings[key] = JToken.FromObject(value);
            Save();
        }
        public static void Set(string key, string value)
        {
            Settings[key] = JToken.FromObject(value);
            Save();
        }
        public static void Set(string key, int value)
        {
            Settings[key] = JToken.FromObject(value);
            Save();
        }
        public static bool GetBool(string key, bool defaultValue = false)
        {
            if (Settings.TryGetValue(key, out JToken value) && value.Type == JTokenType.Boolean)
                return value.Value<bool>();
            return defaultValue;
        }
        public static string GetString(string key, string defaultValue = "")
        {
            if (Settings.TryGetValue(key, out JToken value) && value.Type == JTokenType.String)
                return value.Value<string>();
            return defaultValue;
        }
        public static int GetInt(string key, int defaultValue = 0)
        {
            if (Settings.TryGetValue(key, out JToken value) && value.Type == JTokenType.Integer)
            {
                try
                {
                    return value.Value<int>();
                }
                catch (Exception)
                {
                    return defaultValue;
                }
            }
            return defaultValue;
        }
        public static bool Exists(string key)
        {
            return Settings.ContainsKey(key);
        }
        public static void Remove(string key)
        {
            if (Settings.Remove(key))
                Save();
        }
        public static void Clear()
        {
            Settings.Clear();
            Save();
        }
        public static void SetDefault(string key, bool value)
        {
            if (!Exists(key))
                Set(key, value);
        }
        public static void SetDefault(string key, string value)
        {
            if (!Exists(key))
                Set(key, value);
        }
        public static void SetDefault(string key, int value)
        {
            if (!Exists(key))
                Set(key, value);
        }
    }
}
