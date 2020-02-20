#region header
// Kay McCormick (mccor)
// 
// WpfApp
// WpfApp
// CallBackHook.cs
// 
// 2020-02-06-9:57 PM
// 
// ---
#endregion
using System ;
using System.Reflection ;
using Castle.DynamicProxy ;
using NLog ;

namespace WpfApp.Proxy
{
    internal class CallBackHook : IProxyGenerationHook
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;


        public void MethodsInspected ( ) { }


        public void NonProxyableMemberNotification ( Type type , MemberInfo memberInfo )
        {
            Logger.Warn ( $"cant proxy {memberInfo.Name}" ) ;
        }


        public bool ShouldInterceptMethod ( Type type , MethodInfo methodInfo )
        {
            Logger.Info ( $"can proxy {methodInfo.Name}" ) ;
            return true ;
        }
    }
}