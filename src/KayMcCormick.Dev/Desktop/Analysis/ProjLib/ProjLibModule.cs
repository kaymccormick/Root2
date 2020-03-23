#region header
// Kay McCormick (mccor)
// 
// AnalyzeConsole
// ProjLib
// ProjLibModule.cs
// 
// 2020-03-09-10:06 PM
// 
// ---
#endregion
using AnalysisAppLib ;
using Autofac ;
using NLog ;
using ProjLib.Interfaces ;

namespace ProjLib
{
    public class ProjLibModule : Module
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        #region Overrides of Module
        protected override void Load ( ContainerBuilder builder )
        {
#pragma warning disable CA1303 // Do not pass literals as localized parameters
            Logger.Trace ( "{methodName}" , nameof ( Load ) ) ;
#pragma warning restore CA1303 // Do not pass literals as localized parameters

        }
        #endregion
    }
}
