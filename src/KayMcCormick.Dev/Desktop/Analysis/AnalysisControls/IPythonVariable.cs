    #region header
// Kay McCormick (mccor)
// 
// Analysis
// AnalysisControls
// IPythonVariable.cs
// 
// 2020-03-25-9:36 PM
// 
// ---
#endregion
namespace AnalysisControls
{
    public interface IPythonVariable
    {
        string VariableName { get ; }

        dynamic GetVariableValue ( ) ;
    }
}