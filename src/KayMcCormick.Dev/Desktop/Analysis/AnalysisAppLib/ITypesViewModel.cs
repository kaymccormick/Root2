#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// AnalysisControls
// ITypesViewModel.cs
// 
// 2020-03-11-7:06 PM
// 
// ---
#endregion
using System.Collections.Generic ;
using AnalysisAppLib.Syntax ;
using AnalysisAppLib.Xaml ;
using KayMcCormick.Dev ;

namespace AnalysisAppLib
{
    /// <summary>
    /// </summary>
    public interface ITypesViewModel : IViewModel
    {
        /// <summary>
        /// </summary>
        AppTypeInfo Root { get ; set ; }

        /// <summary>
        /// </summary>
        bool ShowBordersIsChecked { get ; set ; }

        /// <summary>
        /// </summary>
        uint[] HierarchyColors { get ; }

        /// <summary>
        /// Home of all the <see cref="AppTypeInfo"/> instances.
        /// </summary>
        TypeMapDictionary Map { get ; }

        /// <summary>
        /// <see cref="AppTypeInfo"/> ordered by structure and natural containment.
        /// </summary>
        AppTypeInfoCollection StructureRoot { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        AppTypeInfo GetAppTypeInfo ( object identifier ) ;

        /// <summary>
        /// Get all app type infos
        /// </summary>
        /// <returns></returns>
        IEnumerable < AppTypeInfo > GetAppTypeInfos ( ) ;
    }
}