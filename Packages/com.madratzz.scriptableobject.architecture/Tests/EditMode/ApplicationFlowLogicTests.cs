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

        [Test]
        public void SubclassCanExtendStrategyTable()
        {
            // Concrete subclass adds a new context (Settings) the base class
            // doesn't know about. The protected Add() hook is the extension point.
            var logic = new ExtendedLogic();

            var result = logic.GetDecision(FlowContext.Settings, UICloseReasons.ResumeGame);

            Assert.AreEqual(FlowIntent.ResumePrevious, result);
        }

        private sealed class ExtendedLogic : ApplicationFlowLogic
        {
            public ExtendedLogic()
            {
                Add(FlowContext.Settings, UICloseReasons.ResumeGame, FlowIntent.ResumePrevious);
            }
        }
    }
}
