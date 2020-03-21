using System;
using System.Collections.ObjectModel ;
using System.Collections.Specialized ;
using System.ComponentModel ;
using System.Diagnostics ;
using System.Linq;
using System.Runtime.CompilerServices ;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;

namespace LogViewer1
{
    public class LogEventInstanceCollection : ObservableCollection < LogEventInstance >
    {

    }
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class LogViewerControl : UserControl, INotifyPropertyChanged
    {
        private ICollectionView _defView ;

        public LogViewerControl()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
                // < DataGrid Visibility = "Hidden" Name = "dg"  ItemsSource = "{Binding LogEntries}" AutoGeneratingColumn = "Dg_OnAutoGeneratingColumn" Sorting = "Dg_OnSorting" CanUserAddRows = "False" CanUserReorderColumns = "True" CanUserDeleteRows = "False" CanUserResizeColumns = "True"  CanUserSortColumns = "True" CanUserResizeRows = "True" IsReadOnly = "True" Grid.Column = "1" / >
                // var view = (CollectionViewSource)TryFindResource("EntriesCollectionView");
            // System.Diagnostics.Debug.WriteLine(view?.GetType());
            // PropertyGroupDescription groupDescription = new PropertyGroupDescription();
            // groupDescription.PropertyName = "LoggerName";
// BindingOperations.CollectionViewRegistering += CVR;
            
            // if(view.View != null)
// {
// view.View.GroupDescriptions.Add(groupDescription);
// }
        }

        public ICollectionView DefView
        {
            get => _defView ;
            set
            {
                _defView = value ;
                OnPropertyChanged();
            }
        }

        private void OnLoaded ( object sender , RoutedEventArgs e )
        {
            Debug.WriteLine(DataContext);
            
            Debug.WriteLine(_defView);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription();
            groupDescription.PropertyName = "LoggerName";
            DefView = CollectionViewSource.GetDefaultView(lv.ItemsSource);
            if(DefView == null)
            {
                return;
            }
            DefView.GroupDescriptions.Add ( groupDescription ) ;
            DefView.CollectionChanged += DefViewOnCollectionChanged;
            
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
            lv.SelectionChanged += LvOnSelectionChanged;
            
        }

        private void LvOnSelectionChanged ( object sender , SelectionChangedEventArgs e )
        {
        }
    
        #endregion
        #region Overrides of FrameworkElement
        public override void OnApplyTemplate ( ) { base.OnApplyTemplate ( ) ; }
        #endregion

        private void CVR ( object sender , CollectionViewRegisteringEventArgs e )
        {
            PropertyGroupDescription groupDescription = new PropertyGroupDescription ( ) ;
            if ( ! e.CollectionView.CanGroup )
            {
                return ;
            }
            Debug.WriteLine(e.CollectionView.GetType());
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
                if ( itemPropertyInfo.Descriptor is System.ComponentModel.PropertyDescriptor r )
                {
                    Debug.WriteLine(r.ComponentType);
                    if ( r.ComponentType != typeof ( LogEventInstance ) ) return ;
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
            if (e.PropertyName == "Properties")
            {
                var dataGridTemplateColumn = new DataGridTemplateColumn();
                dataGridTemplateColumn.Header       = "Properties";
                dataGridTemplateColumn.CellTemplate = (DataTemplate)TryFindResource("PropertiesTemplate");
                e.Column                            = dataGridTemplateColumn;
            }

        }

        private void Dg_OnSorting ( object sender , DataGridSortingEventArgs e )
        {
            e.Handled = true;
        }

        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        protected virtual void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;  
        }
    }
}
