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
namespace AnalysisControls.Scripting
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPythonVariable
    {
        /// <summary>
        /// 
        /// </summary>
        string VariableName { get ; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        dynamic GetVariableValue ( ) ;
    }
}