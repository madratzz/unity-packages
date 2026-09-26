using NUnit.Framework;
using ProjectCore.Variables;
using UnityEngine;

namespace Madratzz.Tests.VariablesDatabase
{
    [TestFixture]
    public class DatabaseTests
    {
        private Database _database;
        private string _key;

        [SetUp]
        public void SetUp()
        {
            _database = ScriptableObject.CreateInstance<Database>();
            _key = "dbtest_" + System.Guid.NewGuid().ToString("N");
        }

        [TearDown]
        public void TearDown()
        {
            if (_database.HasKey(_key))
                PlayerPrefs.DeleteKey(_key);
            PlayerPrefs.Save();
            Object.DestroyImmediate(_database);
        }

        [Test]
        public void HasKey_FalseForMissingKey()
        {
            Assert.IsFalse(_database.HasKey(_key));
        }

        [Test]
        public void SetInt_ThenGetInt_RoundTrips()
        {
            _database.SetInt(_key, 123);

            Assert.IsTrue(_database.HasKey(_key));
            Assert.AreEqual(123, _database.GetInt(_key));
        }

        [Test]
        public void SetFloat_ThenGetFloat_RoundTrips()
        {
            _database.SetFloat(_key, 1.5f);

            Assert.AreEqual(1.5f, _database.GetFloat(_key), 1e-6f);
        }

        [Test]
        public void SetBool_ThenGetBool_RoundTrips()
        {
            _database.SetBool(_key, true);
            Assert.IsTrue(_database.GetBool(_key));

            _database.SetBool(_key, false);
            Assert.IsFalse(_database.GetBool(_key));
        }

        [Test]
        public void SetString_ThenGetString_RoundTrips()
        {
            _database.SetString(_key, "persisted");

            Assert.AreEqual("persisted", _database.GetString(_key));
        }

        [Test]
        public void GetInt_MissingKey_ReturnsZero()
        {
            Assert.AreEqual(0, _database.GetInt(_key));
        }
    }
}
