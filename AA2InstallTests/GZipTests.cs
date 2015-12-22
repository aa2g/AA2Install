using Microsoft.VisualStudio.TestTools.UnitTesting;
using AA2Install;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AA2Install.Tests
{
    [TestClass()]
    public class GZipTests
    {
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [TestMethod()]
        public void CompressDecompressStringTest()
        {
            for (int i = 0; i < 1000; i++)
            {
                var str = RandomString(1000);
                var result = GZip.DecompressString(GZip.CompressString(str));
                Assert.AreEqual(str, result, "GZip failed to replicate a string. Expected value: {0}; Actual value: {1}", new object[] { str, result });
            }
        }

        [TestMethod()]
        public void CompressionStrengthTest()
        {
            var big = RandomString(1000000);
            var compressed = GZip.CompressString(big);
            Trace.WriteLine("Compressed value from 1MB: " + (compressed.Length / (1024)).ToString("#,## kB"));
            Trace.WriteLine("Ratio: " + Math.Round((double)(100 * compressed.Length / big.Length), 2).ToString() + "%");
        }
    }
}