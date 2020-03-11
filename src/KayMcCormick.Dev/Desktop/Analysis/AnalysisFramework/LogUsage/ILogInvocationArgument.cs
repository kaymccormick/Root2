#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// LogInvocationArgument.cs
// 
// 2020-02-25-10:17 PM
// 
// ---
#endregion
#if !NETSTANDARD2_0
#endif


namespace AnalysisFramework.LogUsage
{
    public interface ILogInvocationArgument
    {
        string GetJSON ( ILogInvocationArgument arg );
        void SetJSON(string value);

        object Pojo { get ; set ; }
    }
}