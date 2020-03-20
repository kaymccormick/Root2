#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// IIconsSource.cs
// 
// 2020-03-20-5:09 AM
// 
// ---
#endregion
using System.Windows.Controls ;
using System.Windows.Media ;

namespace ProjInterface
{
    public interface IIconsSource
    {
        Image DirectoryIcon { get ; set ; }

        Image ProjectDirectoryIcon { get ; set ; }

        ImageSource DirectoryIconImageSource { get ; set ; }

        ImageSource ProjectDirectoryIconImageSource { get ; set ; }

        ImageSource GetIconForFileExtension ( object extension ) ;
    }
}