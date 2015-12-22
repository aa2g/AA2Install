using Microsoft.VisualStudio.TestTools.UnitTesting;
using AA2Install;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AA2Install.Tests
{
    [TestClass()]
    public class ConfigurationTests
    {
        [TestInitialize()]
        public void TestInitialization()
        {
            if (File.Exists(Paths.CONFIG))
                File.Delete(Paths.CONFIG);
        }

        /// <summary>
        /// Tests reading and writing a setting to configuration.
        /// </summary>
        [TestMethod()]
        public void ReadWriteSettingTest()
        {
            var kv = new KeyValuePair<string, string>("key", "value");

            Configuration.WriteSetting(kv.Key, kv.Value);

            Assert.AreEqual(kv.Value, Configuration.ReadSetting(kv.Key));
        }

        /// <summary>
        /// Tests serializing an object.
        /// </summary>
        [TestMethod()]
        public void SerializeObjectTest()
        {
            string testData = "wew lad";
            Assert.AreEqual(testData, Configuration.DeserializeObject<string>(Configuration.SerializeObject(testData)));
        }

        /// <summary>
        /// Tests writing and reading a boolean value.
        /// </summary>
        [TestMethod()]
        public void getBoolTest()
        {
            bool value = true;
            
            var kv = new KeyValuePair<string, bool>("key", value);

            Configuration.WriteSetting(kv.Key, kv.Value.ToString());

            Assert.AreEqual(value, Configuration.getBool(kv.Key));
        }

        /// <summary>
        /// Tests saving and loading a mod dictionary.
        /// </summary>
        [TestMethod()]
        public void saveLoadModsTest()
        {
            ModDictionary modDict = new ModDictionary();

            Mod m = new Mod("Test");

            modDict.Add("key", m);

            Configuration.WriteSetting("MODS", Configuration.SerializeObject(modDict));

            ModDictionary deserialized = Configuration.DeserializeObject<ModDictionary>(Configuration.ReadSetting("MODS"));

            Assert.AreEqual(modDict["key"].Name, deserialized["key"].Name);
        }
    }
}