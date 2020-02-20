#region header
// Kay McCormick (mccor)
// 
// WpfApp
// WpfApp
// CallbackInterceptor.cs
// 
// 2020-02-06-9:57 PM
// 
// ---
#endregion
using System.Linq ;
using Castle.DynamicProxy ;
using NLog ;

namespace WpfApp.Proxy
{
    internal class CallbackInterceptor : ContainerBaseInterceptor
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public override void Intercept ( IInvocation invocation )
        {
            Logger.Warn ( "callback " + invocation.Method.Name ) ;
            Logger.Warn (
                         string.Join ( ", " , invocation.Arguments.Select ( o => o.ToString ( ) ) )
                        ) ;
        }

        /// <summary>Initializes a new instance of the <see cref="System.Object" /> class.</summary>
        public CallbackInterceptor ( ProxyGenerator generator ) : base ( generator )
        {
        }
    }
}