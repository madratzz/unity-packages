using System;
using ExtensionMethods;
using NUnit.Framework;

namespace Madratzz.Tests.Core
{
    public class DateTimeExtensionsTests
    {
        private static readonly DateTime Mid = new DateTime(2026, 6, 15, 12, 0, 0);
        private static readonly DateTime Before = new DateTime(2026, 6, 1);
        private static readonly DateTime After = new DateTime(2026, 6, 30);

        [Test]
        public void IsInRange_InclusiveBoundaries()
        {
            Assert.IsTrue(Mid.IsInRange(Before, After));
            Assert.IsTrue(Before.IsInRange(Before, After));   // inclusive lower
            Assert.IsTrue(After.IsInRange(Before, After));    // inclusive upper
            Assert.IsFalse(new DateTime(2026, 7, 1).IsInRange(Before, After));
        }

        [Test]
        public void IsLessThan_And_IsGreaterThan()
        {
            Assert.IsTrue(Before.IsLessThan(After));
            Assert.IsTrue(After.IsGreaterThan(Before));
            Assert.IsFalse(After.IsLessThan(Before));
        }

        [Test]
        public void ToEpoch_UnixEpoch_IsZero()
        {
            Assert.AreEqual(0, new DateTime(1970, 1, 1).ToEpoch());
        }

        [Test]
        public void ToEpoch_KnownDate_MatchesSecondsSince1970()
        {
            // 1970-01-02 = one day after epoch = 86400 seconds
            Assert.AreEqual(86400, new DateTime(1970, 1, 2).ToEpoch());
        }
    }
}
