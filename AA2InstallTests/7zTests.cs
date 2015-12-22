using Microsoft.VisualStudio.TestTools.UnitTesting;
using AA2Install.Archives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AA2Install.Archives.Tests
{
    [TestClass()]
    public class _7zTests
    {
        [TestInitialize()]
        public void Initialize()
        {
            Console.InitializeOutput();
        }

        /// <summary>
        /// Tests the indexing method of the 7z wrapper.
        /// </summary>
        [TestMethod()]
        public void IndexTest()
        {
            Mod m = _7z.Index(Environment.CurrentDirectory + @"\Test Data\test.7z", true);

            Assert.IsTrue(m.Filenames.Contains("1.png"));
            Assert.IsTrue(m.Filenames.Contains("2.jpg"));
            Assert.IsTrue(m.Filenames.Count == 4);

            m = _7z.Index(Environment.CurrentDirectory + @"\Test Data\test.7z", false);

            Assert.IsTrue(m.Filenames.Contains(@"AA2_PLAY\4.jpg"));
            Assert.IsTrue(m.Filenames.Contains(@"AA2_MAKE\3.png"));
            Assert.IsTrue(m.Filenames.Count == 2);
        }
    }
}