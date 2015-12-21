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
    public static class Configuration
    {
        public static string ReadSetting(string key, Stream configStream = null)
        {
            try
            {
                string json;

                if (configStream != null)
                {
                    using (BinaryReader br = new BinaryReader(configStream))
                        json = Encoding.Unicode.GetString(br.ReadBytes((int)configStream.Length));
                }
                else
                {
                    if (!File.Exists(Paths.CONFIG))
                        return null;
                    json = File.ReadAllText(Paths.CONFIG);
                }
                
                var appSettings = JsonConvert.DeserializeObject<SerializableDictionary<string>>(json);

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
                var settings = JsonConvert.DeserializeObject<SerializableDictionary<string>>(json);
                if (settings == null)
                    settings = new SerializableDictionary<string>();
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
        public static bool WriteSetting(string key, string value, Stream input, out MemoryStream output)
        {
            try
            {
                Trace.WriteLine(key + " : " + value);
                string json;

                using (StreamReader sr = new StreamReader(input))
                    json = sr.ReadToEnd();

                var settings = JsonConvert.DeserializeObject<SerializableDictionary<string>>(json);
                if (settings == null)
                    settings = new SerializableDictionary<string>();
                settings[key] = value;

                output = new MemoryStream(Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(settings)));
                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error writing app settings: " + ex.Message);
                output = null;
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
        public static bool getBool(string key) => bool.Parse(ReadSetting(key) ?? "False");
        public static bool getBool(string key, Stream stream) => bool.Parse(ReadSetting(key, stream) ?? "False");
        /// <summary>
        /// Saves a list of installed mods to the "MODS" key
        /// </summary>
        /// <param name="list">List of installed mods</param>
        public static void saveMods(SerializableDictionary<Mod> list)
        {
            WriteSetting("MODS", SerializeObject(list));            
        }
        /// <summary>
        /// Loads a list of installed mods from the "MODS" key
        /// </summary>
        /// <returns>List of installed mods</returns>
        public static SerializableDictionary<Mod> loadMods()
        {
            if (ReadSetting("MODS") == null) { return new SerializableDictionary<Mod>(); }
            SerializableDictionary<Mod> s = DeserializeObject<SerializableDictionary<Mod>>(ReadSetting("MODS"));
            return s;
        }
        #endregion
    }
}
