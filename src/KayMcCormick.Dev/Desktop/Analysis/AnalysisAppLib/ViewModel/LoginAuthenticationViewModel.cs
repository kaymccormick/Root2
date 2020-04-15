#region header
// Kay McCormick (mccor)
// 
// Analysis
// AnalysisAppLib
// LoginAuthenticationViewModel.cs
// 
// 2020-04-15-11:54 AM
// 
// ---
#endregion
using System ;
using System.Runtime.Serialization ;
using KayMcCormick.Dev ;
using Microsoft.Graph ;
using Microsoft.Identity.Client ;

namespace AnalysisAppLib.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class LoginAuthenticationViewModel : IViewModel
    {
        private readonly Func < string , GraphServiceClient > _graphFunc ;
        private readonly IPublicClientApplication             _publicClient ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphFunc"></param>
        /// <param name="publicClient"></param>
        public LoginAuthenticationViewModel (
            Func < string , GraphServiceClient > graphFunc
          , IPublicClientApplication             publicClient
        )
        {
            _graphFunc    = graphFunc ;
            _publicClient = publicClient ;
        }

        #region Implementation of ISerializable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion
    }
}