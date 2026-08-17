using CustomEditorUtilities;
using NUnit.Framework;
using UnityEngine;

namespace Madratzz.Tests.BuildAutomation
{
    public class BuilderConfigJsonTests
    {
        private BuilderConfig _config;

        [SetUp]
        public void SetUp()
        {
            _config = ScriptableObject.CreateInstance<BuilderConfig>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_config);
        }

        [Test]
        public void TryParseJson_FullFile_ParsesAllFields()
        {
            const string json = "{\"keystorePassword\":\"ks\",\"keyAliasPassword\":\"ka\",\"buildVersion\":\"2.0.1\",\"outputDirectory\":\"CIBuilds\"}";

            Assert.IsTrue(BuilderConfig.TryParseJson(json, out var data));
            Assert.AreEqual("ks", data.keystorePassword);
            Assert.AreEqual("ka", data.keyAliasPassword);
            Assert.AreEqual("2.0.1", data.buildVersion);
            Assert.AreEqual("CIBuilds", data.outputDirectory);
        }

        [Test]
        public void TryParseJson_InvalidJson_ReturnsFalse()
        {
            Assert.IsFalse(BuilderConfig.TryParseJson("{not json", out _));
            Assert.IsFalse(BuilderConfig.TryParseJson("", out _));
            Assert.IsFalse(BuilderConfig.TryParseJson(null, out _));
        }

        [Test]
        public void PopulateFromJsonFile_MissingFile_ReturnsFalse_KeepsInspectorValues()
        {
            // buildsettings.json does not exist at this project's root.
            _config.KeystorePassword = "inspector-value";

            Assert.IsFalse(_config.PopulateFromJsonFile());
            Assert.AreEqual("inspector-value", _config.KeystorePassword);
        }

        [Test]
        public void ApplyJson_PartialFile_MergesOverInspectorValues()
        {
            _config.KeystorePassword = "inspector-ks";
            _config.KeyAliasPassword = "inspector-ka";
            _config.OutputDirectory = "Builds";

            Assert.IsTrue(BuilderConfig.TryParseJson("{\"buildVersion\":\"3.1.0\"}", out var data));
            _config.ApplyJson(data);

            Assert.AreEqual("3.1.0", _config.BuildVersionOverride);   // from file
            Assert.AreEqual("inspector-ks", _config.KeystorePassword); // untouched
            Assert.AreEqual("inspector-ka", _config.KeyAliasPassword); // untouched
            Assert.AreEqual("Builds", _config.OutputDirectory);        // untouched
        }

        [Test]
        public void ApplyJson_EmptyStrings_DoNotOverwrite()
        {
            _config.KeystorePassword = "inspector-ks";

            Assert.IsTrue(BuilderConfig.TryParseJson("{\"keystorePassword\":\"\",\"outputDirectory\":\"Out\"}", out var data));
            _config.ApplyJson(data);

            Assert.AreEqual("inspector-ks", _config.KeystorePassword);
            Assert.AreEqual("Out", _config.OutputDirectory);
        }

        [Test]
        public void Current_WithToggleOff_DoesNotReadFile()
        {
            BuilderConfig.ResetCache();
            var config = BuilderConfig.Current;
            config.ReadFromResourcesFile = false;
            config.KeystorePassword = "inspector-only";

            // With the toggle off, Current must not attempt file population.
            Assert.AreSame(config, BuilderConfig.Current);
            Assert.AreEqual("inspector-only", config.KeystorePassword);
            BuilderConfig.ResetCache();
        }
    }
}
