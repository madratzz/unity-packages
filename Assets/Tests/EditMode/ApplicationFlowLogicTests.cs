using NUnit.Framework;
using ProjectCore.Architecture;

namespace Madratzz.Tests.Architecture
{
    /// <summary>
    /// Pure-function tests for the strategy table. No MonoBehaviour
    /// instantiation — the controller's UI/transition wiring is integration-
    /// tested in the consumer project.
    /// </summary>
    public class ApplicationFlowLogicTests
    {
        [Test]
        public void Boot_GameCloseReason_GoesToGame()
        {
            var logic = new ApplicationFlowLogic();

            var result = logic.GetDecision(FlowContext.Boot, UICloseReasons.Game);

            Assert.AreEqual(FlowIntent.GoToGame, result);
        }

        [Test]
        public void LevelFail_GameCloseReason_GoesToGame()
        {
            var logic = new ApplicationFlowLogic();

            var result = logic.GetDecision(FlowContext.LevelFail, UICloseReasons.Game);

            Assert.AreEqual(FlowIntent.GoToGame, result);
        }

        [Test]
        public void UnknownContext_DefaultsToGame()
        {
            var logic = new ApplicationFlowLogic();

            // Sending a context+reason that's not in the strategy table.
            var result = logic.GetDecision(FlowContext.None, UICloseReasons.Home);

            Assert.AreEqual(FlowIntent.DefaultToGame, result);
        }

        [TestCase(FlowContext.MainMenu,  UICloseReasons.Game,       FlowIntent.GoToGame)]
        [TestCase(FlowContext.MainMenu,  UICloseReasons.Settings,   FlowIntent.OpenSettings)]
        [TestCase(FlowContext.MainMenu,  UICloseReasons.Store,      FlowIntent.OpenStore)]
        [TestCase(FlowContext.Settings,  UICloseReasons.ResumeGame, FlowIntent.ResumePrevious)]
        [TestCase(FlowContext.Settings,  UICloseReasons.Home,       FlowIntent.GoToMainMenu)]
        [TestCase(FlowContext.Store,     UICloseReasons.ResumeGame, FlowIntent.ResumePrevious)]
        [TestCase(FlowContext.Store,     UICloseReasons.Home,       FlowIntent.GoToMainMenu)]
        [TestCase(FlowContext.Gameplay,  UICloseReasons.Home,       FlowIntent.GoToMainMenu)]
        [TestCase(FlowContext.Gameplay,  UICloseReasons.Settings,   FlowIntent.OpenSettings)]
        [TestCase(FlowContext.Gameplay,  UICloseReasons.Store,      FlowIntent.OpenStore)]
        [TestCase(FlowContext.LevelFail, UICloseReasons.Store,      FlowIntent.OpenStore)]
        [TestCase(FlowContext.LevelFail, UICloseReasons.Home,       FlowIntent.GoToMainMenu)]
        [TestCase(FlowContext.Boot,      UICloseReasons.Home,       FlowIntent.GoToMainMenu)]
        public void ScreenRoutes_ResolveToExpectedIntent(FlowContext context, UICloseReasons reason, FlowIntent expected)
        {
            var logic = new ApplicationFlowLogic();

            var result = logic.GetDecision(context, reason);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SubclassCanExtendStrategyTable()
        {
            // (Gameplay, Revive) is deliberately absent from the base table, so
            // this only passes because the subclass added it — keeping the test
            // an honest check of the protected Add() extension point.
            Assert.AreEqual(
                FlowIntent.DefaultToGame,
                new ApplicationFlowLogic().GetDecision(FlowContext.Gameplay, UICloseReasons.Revive),
                "Base table should not define (Gameplay, Revive); pick another pair for this test.");

            var logic = new ExtendedLogic();

            var result = logic.GetDecision(FlowContext.Gameplay, UICloseReasons.Revive);

            Assert.AreEqual(FlowIntent.GoToGame, result);
        }

        [Test]
        public void SubclassCanOverrideAnExistingStrategy()
        {
            // Add() writes through an indexer, so a subclass can replace a base
            // mapping as well as add new ones.
            var logic = new OverridingLogic();

            var result = logic.GetDecision(FlowContext.MainMenu, UICloseReasons.Game);

            Assert.AreEqual(FlowIntent.OpenStore, result);
        }

        private sealed class ExtendedLogic : ApplicationFlowLogic
        {
            public ExtendedLogic()
            {
                Add(FlowContext.Gameplay, UICloseReasons.Revive, FlowIntent.GoToGame);
            }
        }

        private sealed class OverridingLogic : ApplicationFlowLogic
        {
            public OverridingLogic()
            {
                Add(FlowContext.MainMenu, UICloseReasons.Game, FlowIntent.OpenStore);
            }
        }
    }
}
