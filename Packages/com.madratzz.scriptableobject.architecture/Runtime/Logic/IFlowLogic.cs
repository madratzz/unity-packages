using ProjectCore.Architecture;

namespace ProjectCore.Architecture
{
    /// <summary>
    /// Pure-function decision contract: given the current flow context and the
    /// reason a UI view was closed, return the <see cref="FlowIntent"/> the
    /// application should execute. Implementations should be deterministic and
    /// side-effect free — they're invoked by
    /// <see cref="ApplicationFlowController"/> on every UI close.
    /// </summary>
    public interface IFlowLogic
    {
        FlowIntent GetDecision(FlowContext context, UICloseReasons reason);
    }
}
