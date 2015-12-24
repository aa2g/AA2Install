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
    [DeploymentItem(@"AA2InstallTests\tools\", "tools")]
    [DeploymentItem(@"AA2InstallTests\Test Data\", "Test Data")]
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

            Assert.IsTrue(m.SubFilenames.Contains("1.png"));
            Assert.IsTrue(m.SubFilenames.Contains("2.jpg"));
            Assert.IsTrue(m.SubFilenames.Count == 4);

            m = _7z.Index(Environment.CurrentDirectory + @"\Test Data\test.7z", false);

            Assert.IsTrue(m.SubFilenames.Contains(@"AA2_PLAY\4.jpg"));
            Assert.IsTrue(m.SubFilenames.Contains(@"AA2_MAKE\3.png"));
            Assert.IsTrue(m.SubFilenames.Count == 2);

            Assert.IsTrue(Console.ConsoleLog.Count > 0);
        }
    }
}