#region header
// Kay McCormick (mccor)
// 
// Deployment
// KayMcCormick.Lib.Wpf
// TitleMetadataAttribute.cs
// 
// 2020-03-16-10:50 AM
// 
// ---
#endregion
using System ;
using System.ComponentModel.Composition ;

namespace KayMcCormick.Dev.Attributes
{
    /// <summary>
    ///     Metadata atttribute to indicate the title of a component or command.
    ///     Intended for user dusplay. Right now is transferred from views to menu
    ///     commands.
    /// </summary>
    /// 
    [ MetadataAttribute ]
    public class TitleMetadataAttribute : Attribute
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="title"></param>
        public TitleMetadataAttribute ( string title ) { Title = title ; }

        /// <summary>
        ///     Title
        /// </summary>
        public string Title { get ; }
    }
}