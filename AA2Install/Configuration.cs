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
        /// <summary>
        /// Reads a setting from the configuration.
        /// </summary>
        /// <param name="key">Key of item to retrieve.</param>
        /// <param name="configStream">If supplied, reads configuration file from this stream instead of the default path</param>
        /// <returns>True if successful, otherwise false.</returns>
        public static string ReadSetting(string key, Stream configStream = null)
        {
            try
            {
                key = key.ToLower();
                string json;

                if (configStream != null)
                {
                    using (BinaryReader br = new BinaryReader(configStream))
                        json = Encoding.Unicode.GetString(br.ReadBytes((int)configStream.Length));
                }
                else
                {
                    if (!File.Exists(Paths.CONFIG + ".gz"))
                        return null;
                    json = GZip.DecompressString(File.ReadAllBytes(Paths.CONFIG + ".gz"));
                }
                
                var appSettings = JsonConvert.DeserializeObject<SerializableDictionary<string, string>>(json);

                if (!appSettings.ContainsKey(key))
                    return null;
                
                return appSettings[key];
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error reading app settings: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Writes a setting to the configuration.
        /// </summary>
        /// <param name="key">Key of item to write.</param>
        /// <param name="value">Value of item to write.</param>
        /// <returns>True if successful, otherwise false.</returns>
        public static bool WriteSetting(string key, string value)
        {
            try
            {
                key = key.ToLower();
                string json = "";
                if (File.Exists(Paths.CONFIG + ".gz"))
                    json = GZip.DecompressString(File.ReadAllBytes(Paths.CONFIG + ".gz"));
                var settings = JsonConvert.DeserializeObject<SerializableDictionary<string, string>>(json);
                if (settings == null)
                    settings = new SerializableDictionary<string, string>();
                settings[key] = value;
                
                File.WriteAllBytes(Paths.CONFIG + ".gz", GZip.CompressString(JsonConvert.SerializeObject(settings)));
                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error writing app settings: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Writes a setting to the configuration, using the supplied stream.
        /// </summary>
        /// <param name="key">Key of item to write.</param>
        /// <param name="value">Value of item to write.</param>
        /// <param name="input">Stream to load configuration data from.</param>
        /// <param name="output">Output stream of new configuration data.</param>
        /// <returns>True if successful, otherwise false.</returns>
        public static bool WriteSetting(string key, string value, Stream input, out MemoryStream output)
        {
            try
            {
                key = key.ToLower();
                string json;

                using (StreamReader sr = new StreamReader(input))
                    json = sr.ReadToEnd();

                var settings = JsonConvert.DeserializeObject<SerializableDictionary<string, string>>(json);
                if (settings == null)
                    settings = new SerializableDictionary<string, string>();
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
        /// Serialize an object.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize.</typeparam>
        /// <param name="toSerialize">Object to serialize.</param>
        /// <returns>Serialized object.</returns>
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
        /// Deserialize an object.
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize.</typeparam>
        /// <param name="toDeserialize">String to deserialize.</param>
        /// <returns>Deserialized object</returns>
        public static T DeserializeObject<T>(this string toDeserialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (StringReader textReader = new StringReader(toDeserialize))
            {
                return (T)xmlSerializer.Deserialize(textReader);
            }
        }

        #region Quick Access
        /// <summary>
        /// Retrieves the value of key as a bool.
        /// </summary>
        /// <param name="key">Key of item.</param>
        /// <returns>Value of key as a bool.</returns>
        public static bool getBool(string key) => bool.Parse(ReadSetting(key) ?? "False");
        public static bool getBool(string key, Stream stream) => bool.Parse(ReadSetting(key, stream) ?? "False");
        /// <summary>
        /// Saves a list of installed mods to the "MODS" key
        /// </summary>
        /// <param name="list">List of installed mods.</param>
        /// <returns>True if successful, otherwise false</returns>
        public static bool saveMods(ModDictionary list) => WriteSetting("MODS", SerializeObject(list));
        /// <summary>
        /// Loads a list of installed mods from the "MODS" key in configuration.
        /// </summary>
        /// <returns>List of installed mods.</returns>
        public static bool loadMods(out ModDictionary dict)
        {
            dict = new ModDictionary();
            if (ReadSetting("MODS") == null) { return true; }
            try
            {
                dict = DeserializeObject<ModDictionary>(ReadSetting("MODS"));
                return true;
            }
            catch (InvalidOperationException ex)
            {
                Trace.Write("Invalid mod configuration caught. Details: " + ex.Message, "Exception");
            }
            return false;
        }
        #endregion
    }
}
