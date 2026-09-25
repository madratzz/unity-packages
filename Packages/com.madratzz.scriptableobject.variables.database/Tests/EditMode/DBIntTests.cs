using NUnit.Framework;
using ProjectCore.Variables;
using UnityEngine;

namespace Madratzz.Tests.VariablesDatabase
{
    [TestFixture]
    public class DBIntTests
    {
        private DBInt _variable;
        private string _key;

        [SetUp]
        public void SetUp()
        {
            _variable = ScriptableObject.CreateInstance<DBInt>();
            _key = "dbint_" + System.Guid.NewGuid().ToString("N");
            _variable.SetKey(_key);
        }

        [TearDown]
        public void TearDown()
        {
            if (PlayerPrefs.HasKey(_key))
                PlayerPrefs.DeleteKey(_key);
            PlayerPrefs.Save();
            Object.DestroyImmediate(_variable);
        }

        [Test]
        public void SetValue_PersistsToPlayerPrefs()
        {
            _variable.SetValue(55);

            Assert.AreEqual(55, PlayerPrefs.GetInt(_key));
        }

        [Test]
        public void ApplyChange_PersistsAccumulatedValue()
        {
            _variable.SetValue(10);
            _variable.ApplyChange(5);

            Assert.AreEqual(15, _variable.GetValue());
            Assert.AreEqual(15, PlayerPrefs.GetInt(_key));
        }

        [Test]
        public void Load_WithPersistedValue_OverridesCurrentValue()
        {
            PlayerPrefs.SetInt(_key, 77);
            PlayerPrefs.Save();
            _variable.SetValue(1); // also persists 1; force-write below
            PlayerPrefs.SetInt(_key, 77);
            PlayerPrefs.Save();

            _variable.Load();

            Assert.AreEqual(77, _variable.GetValue());
        }

        [Test]
        public void SetKey_GetKey_RoundTrips()
        {
            Assert.AreEqual(_key, _variable.GetKey());
            Assert.AreEqual(_key, _variable.PlayerPrefsKey);
        }

        // Regression: with no saved value, Load used to ignore DefaultValue and
        // hard-zero the variable whenever ResetToDefaultOnPlay was false, so a
        // first-run user got 0 instead of the author's default.
        [Test]
        public void Load_WithNoSavedValue_UsesDefaultValue_EvenWhenNotResettingOnPlay()
        {
            var so = new UnityEditor.SerializedObject(_variable);
            so.FindProperty("DefaultValue").intValue = 7;
            so.FindProperty("ResetToDefaultOnPlay").boolValue = false;
            so.ApplyModifiedPropertiesWithoutUndo();
            Assert.IsFalse(PlayerPrefs.HasKey(_key), "precondition: nothing saved under this key");

            _variable.Load();

            Assert.AreEqual(7, _variable.GetValue());
        }

        [Test]
        public void Load_WithASavedValue_PrefersTheSavedValueOverDefault()
        {
            var so = new UnityEditor.SerializedObject(_variable);
            so.FindProperty("DefaultValue").intValue = 7;
            so.FindProperty("ResetToDefaultOnPlay").boolValue = false;
            so.ApplyModifiedPropertiesWithoutUndo();
            PlayerPrefs.SetInt(_key, 42);

            _variable.Load();

            Assert.AreEqual(42, _variable.GetValue());
        }
    }
}
