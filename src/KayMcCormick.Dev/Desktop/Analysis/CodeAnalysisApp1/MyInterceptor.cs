#region header
// Kay McCormick (mccor)
// 
// WpfApp2
// CodeAnalysisApp1
// MyInterceptor.cs
// 
// 2020-02-12-7:30 PM
// 
// ---
#endregion
using System ;
using System.Linq ;
using Castle.DynamicProxy ;
using NLog ;

namespace CodeAnalysisApp1
{
    /// <summary>
    /// Basic interceptor.
    /// </summary>
    public class MyInterceptor : IInterceptor
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        /// <summary>
        /// Handle intercept invocation.
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept ( IInvocation invocation )
        {
            Logger.Debug (
                          "{MethodName} ( {Args} )"
                        , invocation.Method.Name
                        , string.Join ( ", " , invocation.Arguments.Select(( o , i ) => o is string s1 ? $"\"{s1.Replace("\\", "\\\\").Replace("\"", "\\")}\"" : o.ToString())));
            try
            {
                invocation.Proceed ( ) ;
            }
            catch ( Exception ex )
            {
                Logger.Error ( ex , ex.Message ) ;


            }
        }
    }
}