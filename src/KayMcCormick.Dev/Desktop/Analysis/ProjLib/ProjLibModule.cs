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

            Logger.Trace ( "{methodName}" , nameof ( Load ) ) ;


        }
        #endregion
    }
}
