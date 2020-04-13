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
using System.Collections ;
using KayMcCormick.Dev ;

namespace AnalysisAppLib.XmlDoc
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
        uint[] HierarchyColors { get ; set ; }

        /// <summary>
        /// Home of all the <see cref="AppTypeInfo"/> instances.
        /// </summary>
        TypeMapDictionary Map { get ; set ; }

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
    }
}