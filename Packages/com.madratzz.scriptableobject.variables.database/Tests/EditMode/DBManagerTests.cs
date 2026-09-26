using NUnit.Framework;
using ProjectCore.Variables;
using UnityEngine;

namespace Madratzz.Tests.VariablesDatabase
{
    [TestFixture]
    public class DBManagerTests
    {
        private string _key;

        [SetUp]
        public void SetUp()
        {
            _key = "dbm_" + System.Guid.NewGuid().ToString("N");
        }

        [TearDown]
        public void TearDown()
        {
            if (PlayerPrefs.HasKey(_key))
                PlayerPrefs.DeleteKey(_key);
            PlayerPrefs.Save();
        }

        [Test]
        public void SetInt_ThenHasKey_True()
        {
            DBManager.SetInt(null, _key, 9);

            Assert.IsTrue(DBManager.HasKey(null, _key));
        }

        [Test]
        public void SetInt_ThenGetInt_RoundTrips()
        {
            DBManager.SetInt(null, _key, 9);

            Assert.AreEqual(9, DBManager.GetInt(null, _key));
        }

        [Test]
        public void SetBool_ThenGetBool_RoundTrips()
        {
            DBManager.SetBool(null, _key, true);

            Assert.IsTrue(DBManager.GetBool(null, _key));
        }

        [Test]
        public void SetString_ThenGetString_RoundTrips()
        {
            DBManager.SetString(null, _key, "value");

            Assert.AreEqual("value", DBManager.GetString(null, _key));
        }

        [Test]
        public void SetFloat_ThenGetFloat_RoundTrips()
        {
            DBManager.SetFloat(null, _key, 0.25f);

            Assert.AreEqual(0.25f, DBManager.GetFloat(null, _key), 1e-6f);
        }
    }
}
