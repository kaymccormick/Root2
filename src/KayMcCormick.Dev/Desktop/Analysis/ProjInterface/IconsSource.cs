﻿#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// IconsSource.cs
// 
// 2020-03-20-5:09 AM
// 
// ---
#endregion
using System.Collections ;
using System.IO ;
using System.Windows.Controls ;
using System.Windows.Media ;

namespace ProjInterface
{
    public sealed class IconsSource : IIconsSource
    {
        public Image ProjectDirectoryIcon
        {
            get
            {
                if ( _projectDirectoryIcon == null
                     && IconsResources.Contains ( nameof ( ProjectDirectoryIcon ) ) )
                {
                    var resource = IconsResources[ nameof ( ProjectDirectoryIcon ) ] ;
                    var imageSource = ( ImageSource ) resource ;
                    ProjectDirectoryIconImageSource = imageSource ;
                    _projectDirectoryIcon           = new Image { Source = imageSource } ;
                }

                return _projectDirectoryIcon ;
            }
            set { _projectDirectoryIcon = value ; }
        }

        public ImageSource DirectoryIconImageSource
        {
            get
            {
                if ( _directoryIconImageSource == null )
                {
                    _directoryIconImageSource =
                        ( ImageSource ) IconsResources[ typeof ( Directory ) ] ;
                }

                return _directoryIconImageSource ;
            }
            set { _directoryIconImageSource = value ; }
        }

        public ImageSource ProjectDirectoryIconImageSource
        {
            get { return _projectDirectoryIconImageSource ; }
            set { _projectDirectoryIconImageSource = value ; }
        }

        public ImageSource GetIconForFileExtension ( object extension )
        {
            if ( IconsResources != null
                 && IconsResources.Contains ( extension ) )
            {
                return ( ImageSource ) IconsResources[ extension ] ;
            }

            return ( ImageSource ) IconsResources[ typeof ( File ) ] ;
        }

        public Image DirectoryIcon
        {
            get { return _directoryIcon ; }
            set { _directoryIcon = value ; }
        }

        public IDictionary IconsResources
        {
            get { return _iconsResources ; }
            set
            {
                if ( _iconsResources != value )
                {
                    _iconsResources = value ;
                }

                if ( _iconsResources != null )
                {
                    DirectoryIcon = new Image
                                    {
                                        Source =
                                            ( ImageSource ) _iconsResources[ typeof ( Directory ) ]
                                    } ;
                }
            }
        }

        private Image       _directoryIcon ;
        private Image       _projectDirectoryIcon ;
        private ImageSource _directoryIconImageSource ;
        private IDictionary _iconsResources ;
        private ImageSource _projectDirectoryIconImageSource ;
    }
}