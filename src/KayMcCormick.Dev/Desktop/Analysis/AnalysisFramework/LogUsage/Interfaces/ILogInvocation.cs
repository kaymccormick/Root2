#region header
// Kay McCormick (mccor)
// 
// Proj
// AnalysisFramework
// ILogInvocation.cs
// 
// 2020-03-03-7:28 PM
// 
// ---
#endregion
using System.Collections.Generic ;

namespace AnalysisFramework.LogUsage.Interfaces
{
    public interface ILogInvocation
    {
        string SourceLocation { get ; set ; }

        string FollowingCode { get ; set ; }

        string PrecedingCode { get ; set ; }

        string Code { get ; set ; }

        string LoggerType { get ; set ; }

        string MethodName { get ; set ; }

        string MethodDisplayName { get ; }

        IList < ILogInvocationArgument > Arguments { get ; }
    }
}