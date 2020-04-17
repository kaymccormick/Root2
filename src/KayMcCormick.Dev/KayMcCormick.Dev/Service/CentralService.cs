#region header
// Kay McCormick (mccor)
// 
// WpfClient1
// WpfClient1
// CentralService.cs
// 
// 2020-03-14-4:59 AM
// 
// ---
#endregion
using System ;
using System.ServiceModel ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.Interfaces ;

namespace KayMcCormick.Dev.Service
{
    /// <summary>
    /// </summary>
#if NETFRAMEWORK
    [ ServiceBehavior ( InstanceContextMode = InstanceContextMode.Single ) ]
#endif
    public sealed class CentralService : ICentralService , IDisposable
    {
        #region Implementation of ICentralService
        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ CanBeNull ]
        public RegisterApplicationInstanceResponse RegisterApplicationInstance (
            RegisterApplicationInstanceRequest request
        )
        {
            return null ;
        }
        #endregion

        #region IDisposable
        /// <summary>
        /// </summary>
        public void Dispose ( ) { }
        #endregion
    }
}