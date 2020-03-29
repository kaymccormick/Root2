#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// KayMcCormick.Dev
// TestApplicationInstance.cs
// 
// 2020-03-20-12:56 AM
// 
// ---
#endregion
using System ;
using System.Collections ;
using Autofac ;
using Autofac.Core ;
using KayMcCormick.Dev.Application ;
using KayMcCormick.Dev.Logging ;

namespace KayMcCormick.Dev
{
    /// <summary>
    /// </summary>
    public class TestApplicationInstance : ApplicationInstanceBase
    {
        /// <summary>
        /// </summary>
        /// <param name="logMethod"></param>
        public TestApplicationInstance ( LogDelegates.LogMethod logMethod ) : base ( logMethod ) { }

        #region Overrides of ApplicationInstanceBase
        /// <summary>
        /// </summary>
        /// <param name="appModule"></param>
        public override void AddModule ( IModule appModule )
        {
            throw new NotImplementedException ( ) ;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        protected override IContainer BuildContainer ( ) { throw new NotImplementedException ( ) ; }

        /// <summary>
        /// </summary>
        /// <param name="logMethod2"></param>
        /// <returns></returns>
        protected override IEnumerable LoadConfiguration ( LogDelegates.LogMethod logMethod2 )
        {
            yield break ;
        }

        /// <summary>
        /// </summary>
        public override void Dispose ( ) { }
        #endregion
    }
}