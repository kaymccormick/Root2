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
using System ;
using System.Collections ;
using System.IO ;
using System.Windows.Controls ;
using System.Windows.Media ;
using JetBrains.Annotations ;

namespace ProjInterface
{
    public sealed class IconsSource : IIconsSource
    {
        private Image       _directoryIcon ;
        private ImageSource _directoryIconImageSource ;
        private IDictionary _iconsResources ;
        private Image       _projectDirectoryIcon ;
        private ImageSource _projectDirectoryIconImageSource ;

        public IDictionary IconsResources
        {
            get { return _iconsResources ; }
            set
            {
                if ( ! Equals ( _iconsResources , value ) )
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

        [ CanBeNull ] public Image ProjectDirectoryIcon
        {
            get
            {
                if ( _projectDirectoryIcon != null
                     || ! IconsResources.Contains ( nameof ( ProjectDirectoryIcon ) ) )
                {
                    return _projectDirectoryIcon ;
                }

                var resource = IconsResources[ nameof ( ProjectDirectoryIcon ) ] ;
                var imageSource = ( ImageSource ) resource ;
                ProjectDirectoryIconImageSource = imageSource ;
                _projectDirectoryIcon           = new Image { Source = imageSource } ;

                return _projectDirectoryIcon ;
            }
            set { _projectDirectoryIcon = value ; }
        }

        public ImageSource DirectoryIconImageSource
        {
            get
            {
                return _directoryIconImageSource
                       ?? ( _directoryIconImageSource =
                                ( ImageSource ) IconsResources[ typeof ( Directory ) ] ) ;
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
            if ( IconsResources?.Contains ( extension ) == true )
            {
                return ( ImageSource ) IconsResources[ extension ] ;
            }

            if ( IconsResources != null )
            {
                return ( ImageSource ) IconsResources[ typeof ( File ) ] ;
            }

            throw new AppInvalidOperationException ( ) ;
        }

        public Image DirectoryIcon
        {
            get { return _directoryIcon ; }
            set { _directoryIcon = value ; }
        }
    }
}