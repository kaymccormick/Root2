#region header
// Kay McCormick (mccor)
// 
// Deployment
// ProjInterface
// DockWindowViewModel.cs
// 
// 2020-03-16-10:04 AM
// 
// ---
#endregion
using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.IO ;
using System.Linq ;
using System.Runtime.CompilerServices ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Media ;
using Autofac.Features.Metadata ;
using DynamicData ;
using ExplorerCtrl ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Lib.Wpf ;

namespace ProjInterface
{
    public sealed class DockWindowViewModel : IViewModel , IIconsSource , INotifyPropertyChanged
    {
        private readonly ObservableCollection < IExplorerItem > _rootCollection =
            new ObservableCollection < IExplorerItem > ( ) ;

        
        private readonly IEnumerable < Meta < Lazy < IView1 > > > _views ;
        private          string                                   _defaultInputPath ;
        private          ImageSource                              _directoryIconImageSource ;
        private          IDictionary                              _iconsResources ;
        private          Image                                    _projectDirectoryIcon ;
        private          FrameworkElement                         _resourcesElement ;

        public DockWindowViewModel ( IEnumerable < Meta < Lazy < IView1 > > > views )
        {
            // This or your preferred way of querying for Visual Studio services
            // IVsImageService2 imageService = (IVsImageService2)Package.GetGlobalService(typeof(SVsImageService));
            _views = views ;
            DefaultInputPath = Path.Combine (
                                             Environment.GetFolderPath (
                                                                        Environment
                                                                           .SpecialFolder
                                                                           .UserProfile
                                                                       )
                                           , @"source\repos"
                                            ) ;
        }

        public IEnumerable < Meta < Lazy < IView1 > > > Views { get { return _views ; } }

        public IExplorerItem Item { get ; set ; }

        public ObservableCollection < IExplorerItem > RootCollection
        {
            get { return _rootCollection ; }
        }

        public FrameworkElement ResourcesElement
        {
            get { return _resourcesElement ; }
            set
            {
                _resourcesElement = value ;
                if ( _resourcesElement != null )
                {
                    Item = new AppExplorerItem ( DefaultInputPath , this ) ;
                    RootCollection.AddRange ( Item.Children.Where ( item => item.IsDirectory ) ) ;
                    _iconsResources =
                        _resourcesElement.TryFindResource ( "IconsResources" ) as IDictionary ;
                }
            }
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

        public string DefaultInputPath
        {
            get { return _defaultInputPath ; }
            set { _defaultInputPath = value ; }
        }

        public event PropertyChangedEventHandler PropertyChanged ;

        #region Implementation of IViewModel
        public object TryFindResource ( object resourceKey )
        {
            return _resourcesElement.TryFindResource ( resourceKey ) ;
        }
        #endregion

        [ NotifyPropertyChangedInvocator ]
        private void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }

        #region Implementation of IIconsSource
        public Image ProjectDirectoryIcon
        {
            get
            {
                if ( _projectDirectoryIcon == null
                     && IconsResources.Contains ( nameof ( ProjectDirectoryIcon ) ) )
                {
                    var resource = IconsResources[ nameof ( ProjectDirectoryIcon ) ] ;
                    var imageSource = ( ImageSource ) resource ;
                    ProjectDirectoryIconImageSsource = imageSource ;
                    _projectDirectoryIcon            = new Image { Source = imageSource } ;
                }

                return _projectDirectoryIcon ;
            }
            set { _projectDirectoryIcon = value ; }
        }

        public ImageSource DirectoryIconImageSource
        {
            get
            {
                if(_directoryIconImageSource == null)
                {
                    _directoryIconImageSource =
                        ( ImageSource ) IconsResources[ typeof ( Directory ) ] ;
                }
                return _directoryIconImageSource ;
            }
            set { _directoryIconImageSource = value ; }
        }

        public ImageSource ProjectDirectoryIconImageSsource { get ; set ; }

        public ImageSource GetIconForFileExtension ( object extension )
        {
            if ( IconsResources != null
                 && IconsResources.Contains ( extension ) )
            {
                return ( ImageSource ) IconsResources[ extension ] ;
            }

            return ( ImageSource ) IconsResources[ typeof ( File ) ] ;
        }

        public Image DirectoryIcon { get ; set ; }
        #endregion
    }

    public interface IIconsSource
    {
        Image DirectoryIcon { get ; set ; }

        Image ProjectDirectoryIcon { get ; set ; }

        ImageSource DirectoryIconImageSource { get ; set ; }

        ImageSource ProjectDirectoryIconImageSsource { get ; set ; }

        ImageSource GetIconForFileExtension ( object extension ) ;
    }
}