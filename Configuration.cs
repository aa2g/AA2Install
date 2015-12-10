using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;
using Newtonsoft.Json;

namespace AA2Install
{
    static class Configuration
    {
        public static string ReadSetting(string key)
        {
            try
            {
                if (!File.Exists(Paths.CONFIG))
                    return null;

                string json = File.ReadAllText(Paths.CONFIG);
                var appSettings = JsonConvert.DeserializeObject<SerializableDictionary<string, string>>(json);

                if (!appSettings.ContainsKey(key))
                    return null;

                string result = appSettings[key];
                Trace.WriteLine(key + " : " + result);
                return result;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error reading app settings: " + ex.Message);
                return null;
            }
        }
        public static bool WriteSetting(string key, string value)
        {
            try
            {
                Trace.WriteLine(key + " : " + value);
                string json = "";
                if (File.Exists(Paths.CONFIG))
                    json = File.ReadAllText(Paths.CONFIG);
                var settings = JsonConvert.DeserializeObject<SerializableDictionary<string, string>>(json);
                if (settings == null)
                    settings = new SerializableDictionary<string, string>();
                settings[key] = value;

                File.WriteAllText(Paths.CONFIG, JsonConvert.SerializeObject(settings));
                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error writing app settings: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Serialize an object, useful for saving as a key/value pair
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="toSerialize">Object to serialize</param>
        /// <returns>Serialized object</returns>
        public static string SerializeObject<T>(this T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }
        /// <summary>
        /// Serialize an object, useful for saving as a key/value pair
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize</typeparam>
        /// <param name="toDeserialize">String to deserialize</param>
        /// <returns>Deserialized object</returns>
        public static T DeserializeObject<T>(string toDeserialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (StringReader textReader = new StringReader(toDeserialize))
            {
                return (T)xmlSerializer.Deserialize(textReader);
            }
        }

        #region Quick Access
        /// <summary>
        /// Retrieves the value of key in type bool
        /// </summary>
        /// <param name="key">Key of item</param>
        /// <returns>Value of key in type bool</returns>
        public static bool getBool(string key)
        {
            return bool.Parse(Configuration.ReadSetting(key) ?? "False");
        }
        /// <summary>
        /// Saves a list of installed mods to the "MODS" key
        /// </summary>
        /// <param name="list">List of installed mods</param>
        public static void saveMods(Dictionary<string, Mod> list)
        {
            SerializableDictionary<string, Mod> s = new SerializableDictionary<string, Mod>();
            foreach (string key in list.Keys)
            {
                s[key] = list[key];
            }
            WriteSetting("MODS", SerializeObject<SerializableDictionary<string, Mod>>(s));            
        }
        /// <summary>
        /// Loads a list of installed mods from the "MODS" key
        /// </summary>
        /// <returns>List of installed mods</returns>
        public static Dictionary<string, Mod> loadMods()
        {
            if (ReadSetting("MODS") == null) { return new Dictionary<string, Mod>(); }
            Dictionary<string, Mod> d = new Dictionary<string, Mod>();
            SerializableDictionary<string, Mod> s = DeserializeObject<SerializableDictionary<string, Mod>>(ReadSetting("MODS"));
            foreach (string key in s.Keys)
            {
                d[key] = s[key];
            }
            return d;
        }
        #endregion
    }
}
