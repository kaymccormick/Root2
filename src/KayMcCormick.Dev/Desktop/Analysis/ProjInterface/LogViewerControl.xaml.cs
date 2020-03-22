using System ;
using System.Collections.Specialized ;
using System.ComponentModel ;
using System.Diagnostics ;
using System.Linq ;
using System.Runtime.CompilerServices ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Data ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Lib.Wpf ;

namespace ProjInterface
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    [ TitleMetadata ( "Log Viewer" ) ]
    public partial class LogViewerControl : UserControl , INotifyPropertyChanged , IView1, IControlView
    {
        private readonly LogViewerConfig _config ;
        private          ICollectionView _defView ;
        private          string          _viewTitle ;

        public LogViewerControl ( ) { InitializeComponent ( ) ; }

        public LogViewerControl ( [ NotNull ] LogViewerConfig config )
        {
            _config = config ?? throw new ArgumentNullException ( nameof ( config ) ) ;
            InitializeComponent ( ) ;
        }

        public ICollectionView DefView
        {
            get { return _defView ; }
            set
            {
                _defView = value ;
                OnPropertyChanged ( ) ;
            }
        }

        private void DefViewOnCollectionChanged (
            object                           sender
          , NotifyCollectionChangedEventArgs e
        )
        {
            Debug.WriteLine ( e.Action ) ;
        }

        #region Overrides of FrameworkElement
        protected override void OnInitialized ( EventArgs e )
        {
            base.OnInitialized ( e ) ;
            lv.SelectionChanged += LvOnSelectionChanged ;
        }

        private void LvOnSelectionChanged ( object sender , SelectionChangedEventArgs e ) { }
        #endregion
        #region Overrides of FrameworkElement
        public override void OnApplyTemplate ( )
        {
            base.OnApplyTemplate ( ) ;
            Debug.WriteLine ( DataContext ) ;
            Debug.WriteLine ( _defView ) ;
            var groupDescription = new PropertyGroupDescription { PropertyName = "LoggerName" } ;
            DefView = CollectionViewSource.GetDefaultView ( lv.ItemsSource ) ;
            if ( DefView == null )
            {
                return ;
            }

            DefView.GroupDescriptions.Add ( groupDescription ) ;
            DefView.CollectionChanged += DefViewOnCollectionChanged ;
        }
        #endregion

        private void CVR ( object sender , CollectionViewRegisteringEventArgs e )
        {
            var groupDescription = new PropertyGroupDescription ( ) ;
            if ( ! e.CollectionView.CanGroup )
            {
                return ;
            }

            Debug.WriteLine ( e.CollectionView.GetType ( ) ) ;
            Debug.WriteLine ( e.CollectionView.Count ) ;
            if ( ! ( e.CollectionView is ListCollectionView l )
                 || l.ItemProperties == null )
            {
                return ;
            }

            foreach ( var itemPropertyInfo in l.ItemProperties )
            {
                Debug.WriteLine (
                                 itemPropertyInfo.Name
                                 + "\t"
                                 + itemPropertyInfo.PropertyType
                                 + "\t"
                                 + itemPropertyInfo.Descriptor
                                ) ;
                if ( itemPropertyInfo.Descriptor is PropertyDescriptor r )
                {
                    Debug.WriteLine ( r.ComponentType ) ;
                    if ( r.ComponentType != typeof ( LogEventInstance ) )
                    {
                        return ;
                    }
                }
            }

            var p = l.ItemProperties.FirstOrDefault ( info => info.Name == "LoggerName" ) ;
            if ( p == null )
            {
                return ;
            }

            groupDescription.PropertyName = "LoggerName" ;
            e.CollectionView.GroupDescriptions.Add ( groupDescription ) ;
        }

        private void Dg_OnAutoGeneratingColumn (
            object                                sender
          , DataGridAutoGeneratingColumnEventArgs e
        )
        {
            if ( e.PropertyName == "Properties" )
            {
                var dataGridTemplateColumn = new DataGridTemplateColumn ( ) ;
                dataGridTemplateColumn.Header = "Properties" ;
                dataGridTemplateColumn.CellTemplate =
                    ( DataTemplate ) TryFindResource ( "PropertiesTemplate" ) ;
                e.Column = dataGridTemplateColumn ;
            }
        }

        private void Dg_OnSorting ( object sender , DataGridSortingEventArgs e )
        {
            e.Handled = true ;
        }

        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        protected virtual void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }

        #region Implementation of IView1
        public string ViewTitle { get { return _viewTitle ; } set { _viewTitle = value ; } }
        #endregion
    }
}