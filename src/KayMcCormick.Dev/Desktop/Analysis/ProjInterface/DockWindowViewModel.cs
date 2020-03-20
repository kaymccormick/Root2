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
    public sealed class DockWindowViewModel : IViewModel , INotifyPropertyChanged
    {
        private readonly ObservableCollection < IExplorerItem > _rootCollection =
            new ObservableCollection < IExplorerItem > ( ) ;

        private readonly IEnumerable < Meta < Lazy < IView1 > > > _views ;
        private readonly IIconsSource                             _iconsSource ;
        private          string                                   _defaultInputPath ;
        private          FrameworkElement                         _resourcesElement ;
        private          IDictionary                              _iconsResources ;

        public DockWindowViewModel (
            IEnumerable < Meta < Lazy < IView1 > > > views
          , IIconsSource                             iconsSource
        )
        {
            _views       = views ;
            _iconsSource = iconsSource ;
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


        public string DefaultInputPath
        {
            get { return _defaultInputPath ; }
            set { _defaultInputPath = value ; }
        }

        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        private void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }
    }
}