using CustomEditorUtilities;
using NUnit.Framework;
using UnityEditor;

namespace Madratzz.Tests.BuildAutomation
{
    public class BuilderVersionCodeTests
    {
        [Test]
        public void TryGenerateVersionCode_SemanticVersion_StripsDots()
        {
            Assert.IsTrue(Builder.TryGenerateVersionCode("1.2.3", out int code));
            Assert.AreEqual(123, code);
        }

        [Test]
        public void TryGenerateVersionCode_SingleNumber_Parses()
        {
            Assert.IsTrue(Builder.TryGenerateVersionCode("42", out int code));
            Assert.AreEqual(42, code);
        }

        [Test]
        public void TryGenerateVersionCode_NullOrEmpty_ReturnsFalse()
        {
            Assert.IsFalse(Builder.TryGenerateVersionCode(null, out _));
            Assert.IsFalse(Builder.TryGenerateVersionCode("", out _));
        }

        [Test]
        public void TryGenerateVersionCode_NonNumeric_ReturnsFalse()
        {
            Assert.IsFalse(Builder.TryGenerateVersionCode("1.2.beta", out _));
        }
    }

    public class BuilderSceneListTests
    {
        [Test]
        public void GetEnabledScenePaths_FiltersDisabledScenes()
        {
            var scenes = new[]
            {
                new EditorBuildSettingsScene("Assets/Scenes/Boot.unity", true),
                new EditorBuildSettingsScene("Assets/Scenes/Dev.unity", false),
                new EditorBuildSettingsScene("Assets/Scenes/Game.unity", true),
            };

            var paths = Builder.GetEnabledScenePaths(scenes);

            Assert.AreEqual(2, paths.Length);
            Assert.AreEqual("Assets/Scenes/Boot.unity", paths[0]);
            Assert.AreEqual("Assets/Scenes/Game.unity", paths[1]);
        }

        [Test]
        public void GetEnabledScenePaths_EmptyList_ReturnsEmpty()
        {
            Assert.AreEqual(0, Builder.GetEnabledScenePaths(new EditorBuildSettingsScene[0]).Length);
        }
    }

    public class BuilderConfigTests
    {
        [TearDown]
        public void TearDown()
        {
            BuilderConfig.ResetCache();
        }

        [Test]
        public void Current_WithoutAsset_ReturnsNonNullBlankConfig()
        {
            // No Resources/BuilderConfig asset exists in this project — Current
            // must return a blank instance rather than null (regression: archived
            // code NRE'd on the unassigned static config field).
            BuilderConfig.ResetCache();
            var config = BuilderConfig.Current;

            Assert.IsNotNull(config);
            Assert.AreEqual(string.Empty, config.KeystorePassword);
            Assert.AreEqual(string.Empty, config.KeyAliasPassword);
        }

        [Test]
        public void Current_IsCachedAcrossCalls()
        {
            BuilderConfig.ResetCache();
            Assert.AreSame(BuilderConfig.Current, BuilderConfig.Current);
        }
    }
}
