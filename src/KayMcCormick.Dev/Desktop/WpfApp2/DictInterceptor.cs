#region header
// Kay McCormick (mccor)
// 
// WpfApp2
// WpfApp2
// DictInterceptor.cs
// 
// 2020-02-10-6:23 PM
// 
// ---
#endregion
using Castle.DynamicProxy ;
using NLog ;

namespace WpfApp2
{
    public class DictInterceptor : IInterceptor
    {
        // ReSharper disable once InconsistentNaming
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public void Intercept ( IInvocation invocation )
        {
            Logger.Debug( "{methodName}" , invocation.Method.Name ) ;
            invocation.Proceed ( ) ;
        }
    }
}