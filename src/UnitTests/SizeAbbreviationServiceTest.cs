using AquaExplorer.Services;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class SizeAbbreviationServiceTest
    {
        [TestCase("0 bytes", 0)]
        [TestCase("1 byte", 1)]
        [TestCase("2 bytes", 2)]
        [TestCase("25 bytes", 25)]
        [TestCase("999 bytes", 999)]
        [TestCase("0.98 KB", 1000)]
        [TestCase("1.00 KB", 1023)]
        [TestCase("1.00 KB", 1024)]
        [TestCase("1.00 KB", 1024)]
        [TestCase("1.00 KB", 1025)]
        [TestCase("1.01 KB", 1034)]
        [TestCase("1.01 KB", 1035)]
        [TestCase("1.02 KB", 1044)]
        [TestCase("1.10 KB", 1126)]
        [TestCase("2.00 KB", 2048)]
        [TestCase("9.77 KB", 10000)]
        [TestCase("97.7 KB", 100000)]
        [TestCase("977 KB", 1000000)]
        [TestCase("0.98 MB", 1024000)]
        [TestCase("1.00 MB", 1047551)]
        [TestCase("1.00 MB", 1047552)]
        [TestCase("1.00 MB", 1047553)]
        [TestCase("1.00 MB", 1048570)]
        [TestCase("1.00 MB", 1048576)]
        [TestCase("1.00 MB", 1048580)]
        [TestCase("1.03 MB", 1080032)]
        [TestCase("1.30 MB", 1363149)]
        [TestCase("2.00 MB", 2097152)]
        [TestCase("2.00 MB", 2097153)]
        [TestCase("21.3 MB", 22355640)]
        [TestCase("754 MB", 790867476)]
        [TestCase("999 MB", 999*1024*1024)]
        [TestCase("0.98 GB", 1000*1024*1024)]
        [TestCase("1.00 GB", 1073741824)]
        [TestCase("1.01 GB", 1084479242)]
        [TestCase("1.20 GB", 1288490189)]
        public void TestAbbreviation(string abbreviation, long size)
        {
            Assert.AreEqual(abbreviation, new SizeAbbreviationService().Abbreviate(size));
        }

        [TestCase("200 of 400 bytes", 200, 400)]
        [TestCase("200 bytes of 1.00 KB", 200, 1024)]
        [TestCase("2.00 KB of 10.0 MB", 2048, 1024*1024*10)]
        [TestCase("5.00 of 10.0 MB", 1024*1024*5, 1024*1024*10)]
        public void TestAbbreviationPart(string abbreviation, long partSize, long wholeSize)
        {
            Assert.AreEqual(abbreviation, new SizeAbbreviationService().Abbreviate(partSize, wholeSize));
        }

    }
}

