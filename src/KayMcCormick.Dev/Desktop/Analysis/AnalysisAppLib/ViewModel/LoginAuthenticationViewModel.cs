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
    // ReSharper disable once UnusedType.Global
    public sealed class LoginAuthenticationViewModel : IViewModel
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly Func < string , GraphServiceClient > _graphFunc ;
        // ReSharper disable once NotAccessedField.Local
        private readonly IPublicClientApplication             _publicClient ;
        public object InstanceObjectId { get; set; }

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