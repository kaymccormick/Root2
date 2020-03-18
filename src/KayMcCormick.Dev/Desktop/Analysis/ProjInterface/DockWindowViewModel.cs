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
using System.IO ;
using System.Linq ;
using System.Windows ;
using System.Windows.Media ;
using Autofac.Features.Metadata ;
using DynamicData ;
using ExplorerCtrl ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Interfaces ;
using KayMcCormick.Lib.Wpf ;

namespace ProjInterface
{
    public class DockWindowViewModel : IViewModel, IIconsSource
    {
        private readonly IEnumerable < Meta < Lazy < IView1 > > > _views ;

        private IExplorerItem explorerItem ;

        private ObservableCollection < IExplorerItem > _rootCollection = new ObservableCollection < IExplorerItem > ();
        private FrameworkElement _resourcesElement ;
        private string _defaultInputPath ;
        private ImageSource _directoryIcon ;
        private IDictionary _iconsResources ;

        public DockWindowViewModel (IEnumerable<Meta <Lazy <IView1> > >  views)
        {
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

        public IEnumerable < Meta < Lazy < IView1 > > > Views
        {
            get { return _views ; }
        }

        public IExplorerItem Item { get { return explorerItem ; } set { explorerItem = value ; } }

        public ObservableCollection<IExplorerItem> RootCollection { get { return _rootCollection; } }

        public FrameworkElement ResourcesElement
        {
            get { return _resourcesElement ; }
            set
            {
                _resourcesElement = value ;
                if ( _resourcesElement != null )
                {
                    Item = new AppExplorerItem ( DefaultInputPath , this ) ;
                    RootCollection.AddRange (
                                             explorerItem.Children.Where (
                                                                          item => item.IsDirectory
                                                                         )
                                            ) ;
                    IconsResources =
                        _resourcesElement.TryFindResource ( "IconsResources" ) as IDictionary ;
                }
            }
        }

        public IDictionary IconsResources
        {
            get
            {
                return _iconsResources ;
            }
            set
            {
                _iconsResources = value ;
                if ( _iconsResources != null )
                {
                    _directoryIcon = ( ImageSource ) _iconsResources[ typeof ( Directory ) ] ;
                }
            }
        }

        public string DefaultInputPath { get { return _defaultInputPath ; } set { _defaultInputPath = value ; } }

        #region Implementation of IViewModel
        public object TryFindResource ( object resourceKey ) { return _resourcesElement.TryFindResource ( resourceKey ) ; }
        #endregion

        #region Implementation of IIconsSource
        public ImageSource GetIconForFileExtension ( object extension )
        {
            if ( IconsResources != null && IconsResources.Contains ( extension ) )
            {
                return ( ImageSource ) IconsResources[ extension ] ;
            }

            return null ;
        }

        public ImageSource DirectoryIcon { get { return _directoryIcon ; } set { _directoryIcon = value ; } }
        #endregion
    }

    public interface IIconsSource
    {
        ImageSource DirectoryIcon { get ; set ; }

        ImageSource GetIconForFileExtension ( object extension ) ;
    }
}