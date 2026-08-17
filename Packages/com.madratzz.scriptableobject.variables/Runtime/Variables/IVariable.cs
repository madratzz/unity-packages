
namespace ProjectCore.Variables
{
    // Abstract IVariable interface to handle basic operations
    public interface IVariable<T>
    {
        T GetValue();
        T GetDefaultValue();
        void SetValue(T value);
        void SetDefaultValue(T value);
        void ResetToDefaultValue();
    }
    
    // A more general interface for "changes" that could be applied across different variable types
    public interface IApplyChange<T>
    {
        void ApplyChange(T amount);
    }
}