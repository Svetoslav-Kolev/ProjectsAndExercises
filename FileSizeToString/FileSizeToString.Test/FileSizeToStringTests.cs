using FileSizeToString.Classes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileSizeToString.Test
{
    [TestFixture]
    public class FileSizeToStringTests
    {
        [Test]
        public void SizeShouldWorkCorrectlyForBytes()
        {
            Assert.AreEqual("0 bytes", FormattingExtensions.FileSizeToString(0));
            Assert.AreEqual("1 bytes", FormattingExtensions.FileSizeToString(1));
            Assert.AreEqual("2 bytes", FormattingExtensions.FileSizeToString(2));
            Assert.AreEqual("511 bytes", FormattingExtensions.FileSizeToString(511));
            Assert.AreEqual("512 bytes", FormattingExtensions.FileSizeToString(512));
            Assert.AreEqual("1023 bytes", FormattingExtensions.FileSizeToString(1023));
            Assert.AreEqual("0 bytes", FormattingExtensions.FileSizeToString(000000000));
        }
        [Test]
        public void SizeShouldWorkCorrectlyForKilobytes()
        {
            Assert.AreEqual("1.00 KB", FormattingExtensions.FileSizeToString(1024));
            Assert.AreEqual("1.00 KB", FormattingExtensions.FileSizeToString(1025)); // Rounding to 2 floating point digits by default
            Assert.AreEqual("97.66 KB", FormattingExtensions.FileSizeToString(100000));
        }
        [Test]
        public void SizeShouldWorkCorrectlyForMegabytes()
        {
            Assert.AreEqual("1.00 MB", FormattingExtensions.FileSizeToString(1048575)); // Result is 1023,9990234375KB rounded to 1024kb = 1mb
            Assert.AreEqual("1.00 MB", FormattingExtensions.FileSizeToString(1048576));
            Assert.AreEqual("1.00 MB", FormattingExtensions.FileSizeToString(1048577));
            Assert.AreEqual("102.40 MB", FormattingExtensions.FileSizeToString(107374182));
        }
        [Test]
        public void SizeShouldWorkCorrectlyWithCustomPrecision()
        {
            Assert.AreEqual("1.1 KB", FormattingExtensions.FileSizeToString(1126, 1));
            Assert.AreEqual("1.1 KB", FormattingExtensions.FileSizeToString(1127, 1));
            Assert.AreEqual("1.2 KB", FormattingExtensions.FileSizeToString(1178, 1)); //1178 = 1.150390625, rounded = 1.2
            Assert.AreEqual("1 KB", FormattingExtensions.FileSizeToString(1024, 0));
            Assert.AreEqual("1 KB", FormattingExtensions.FileSizeToString(1127, 0));
            Assert.AreEqual("2 KB", FormattingExtensions.FileSizeToString(1536, 0));
            Assert.AreEqual("20 bytes", FormattingExtensions.FileSizeToString(20, 0));
            Assert.AreEqual("1 PB", FormattingExtensions.FileSizeToString(1125899906842624,0));
        }
        [Test]
        public void SizeShouldWorkCorrectlyForTerabytes()
        {
            Assert.AreEqual("12.99 TB", FormattingExtensions.FileSizeToString(14288043651787));
            Assert.AreEqual("1.00 TB", FormattingExtensions.FileSizeToString(1099511627776));
        }
        [Test]
        public void SizeShouldWorkCorrectlyForPetabytes()
        {
            Assert.AreEqual("1.00 PB", FormattingExtensions.FileSizeToString(1125899906842624));
            Assert.AreEqual("1.00 PB", FormattingExtensions.FileSizeToString(1125899906842623));
        }   
        [Test]
        public void SizeShouldWorkCorrectlyIfDigitsAreEqualToDifferentSize()
        {
            Assert.AreEqual("933.05 TB", FormattingExtensions.FileSizeToString(1025899906842623));// same number of digits as a petabyte test
            Assert.AreEqual("986.33 KB", FormattingExtensions.FileSizeToString(1010000));
        }
      
    }
}
