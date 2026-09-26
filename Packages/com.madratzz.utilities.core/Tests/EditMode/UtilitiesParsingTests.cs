using NUnit.Framework;
using CustomUtilities;

namespace Madratzz.Tests.Core
{
    public class UtilitiesParsingTests
    {
        [Test]
        public void ToInt_ParsesPlainInteger()
        {
            Assert.AreEqual(42, Utilities.ToInt("42"));
        }

        [Test]
        public void ToInt_TruncatesDecimalString()
        {
            Assert.AreEqual(42, Utilities.ToInt("42.9"));
        }

        [Test]
        public void ToInt_EmptyOrDash_ReturnsZero()
        {
            Assert.AreEqual(0, Utilities.ToInt(""));
            Assert.AreEqual(0, Utilities.ToInt("-"));
        }

        [Test]
        public void ToFloat_ParsesInvariantCulture()
        {
            Assert.AreEqual(1.5f, Utilities.ToFloat("1.5"), 1e-6f);
        }

        [Test]
        public void ToFloat_EmptyOrDash_ReturnsZero()
        {
            Assert.AreEqual(0f, Utilities.ToFloat(""));
            Assert.AreEqual(0f, Utilities.ToFloat("-"));
        }

        [Test]
        public void ToDouble_ParsesInvariantCulture()
        {
            Assert.AreEqual(2.25, Utilities.ToDouble("2.25"), 1e-9);
        }

        [Test]
        public void ToBool_AcceptsTrueFalseAndNumeric()
        {
            Assert.IsTrue(Utilities.ToBool("true"));
            Assert.IsTrue(Utilities.ToBool("TRUE"));
            Assert.IsTrue(Utilities.ToBool("1"));
            Assert.IsFalse(Utilities.ToBool("false"));
            Assert.IsFalse(Utilities.ToBool("0"));
            Assert.IsFalse(Utilities.ToBool("anything-else"));
        }

        [Test]
        public void EpochConversion_RoundTrips()
        {
            var date = new System.DateTime(2026, 1, 15, 12, 0, 0, System.DateTimeKind.Unspecified);

            int epoch = Utilities.ConvertDateToEpoch(date);
            var back = Utilities.TryParseDateTime(epoch);

            Assert.AreEqual(date, back);
        }

        [Test]
        public void ConvertDateToEpoch_UnixEpoch_IsZero()
        {
            Assert.AreEqual(0, Utilities.ConvertDateToEpoch(new System.DateTime(1970, 1, 1)));
        }

        [Test]
        public void TimeFromSeconds_FormatsMinutesSeconds()
        {
            Assert.AreEqual("01:05", Utilities.TimeFromSeconds(65));
            Assert.AreEqual("00:00", Utilities.TimeFromSeconds(0));
            Assert.AreEqual("10:00", Utilities.TimeFromSeconds(600));
        }
    }
}
