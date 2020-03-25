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
    [ MetadataAttribute ]
    public class TitleMetadataAttribute : Attribute
    {
        public string Title { get ; private set ; }

        public TitleMetadataAttribute ( string title ) { Title = title ; }
    }
}