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
        [TestMethod()]
        public void ReadWriteSettingTest()
        {
            MemoryStream output;
            var kv = new KeyValuePair<string, string>("key", "value");

            Configuration.WriteSetting(kv.Key, kv.Value, new MemoryStream(), out output);

            Assert.AreEqual(kv.Value, Configuration.ReadSetting(kv.Key, output));
        }

        [TestMethod()]
        public void SerializeObjectTest()
        {
            string testData = "wew lad";
            Assert.AreEqual(testData, Configuration.DeserializeObject<string>(Configuration.SerializeObject(testData)));
        }

        [TestMethod()]
        public void getBoolTest()
        {
            bool value = true;

            MemoryStream output;
            var kv = new KeyValuePair<string, bool>("key", value);

            Configuration.WriteSetting(kv.Key, kv.Value.ToString(), new MemoryStream(), out output);

            Assert.AreEqual(value, Configuration.getBool(kv.Key, output));
        }

        [TestMethod()]
        public void saveLoadModsTest()
        {
            SerializableDictionary<Mod> modDict = new SerializableDictionary<Mod>();
            MemoryStream output;

            Mod m = new Mod();
            m.Name = "Test";

            modDict.Add("key", m);

            Configuration.WriteSetting("MODS", Configuration.SerializeObject(modDict), new MemoryStream(), out output);

            SerializableDictionary<Mod> deserialized = Configuration.DeserializeObject<SerializableDictionary<Mod>>(Configuration.ReadSetting("MODS", output));

            Assert.AreEqual(modDict["key"].Name, deserialized["key"].Name);
        }
    }
}