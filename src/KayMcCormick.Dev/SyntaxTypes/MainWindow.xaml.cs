using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AnalysisAppLib.Syntax;
using AnalysisControls;
using AnalysisControls.ViewModel;
using JetBrains.Annotations;
using KayMcCormick.Dev;

namespace SyntaxTypes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private TypesViewModel _viewModel;
        private ObservableCollection<AppTypeInfo> _appTypeInfos = new ObservableCollection<AppTypeInfo>();
        private ICollectionView _view;
        private bool _viewFlat = true;
        private bool _viewHier;
        private TreeView _treeView;
        private ComboBox _combobox;

        public MainWindow()
        {
            ViewModel = TypesViewModelFactory.CreateModel();
            SyntaxTypesModel = new SyntaxTypesModel();

            SyntaxTypesModel.SetBinding(SyntaxTypesModel.SyntaxTypeInfosProperty,
                new Binding("AppTypeInfos") {Source = this});
            SyntaxTypesModel.SetBinding(SyntaxTypesModel.TopLevelTypeInfosProperty,
                new Binding("ViewModel.Root.SubTypeInfos") {Source = this});
            foreach (var appTypeInfo in ViewModel.GetAppTypeInfos())
            {
                AppTypeInfos.Add(appTypeInfo);
            }

            InitializeComponent();

        }

        public override void OnApplyTemplate()
        {

            View = CollectionViewSource.GetDefaultView(SyntaxTypesModel.SyntaxTypeInfos);
            View.CurrentChanged += ViewOnCurrentChanged;
            // _treeView = (TreeView) item.FindName("TreeView");
            // _treeView.SelectedItemChanged += TreeViewOnSelectedItemChanged;
        }

        private void ViewOnCurrentChanged(object sender, EventArgs e)
        {
            DebugUtils.WriteLine(View.CurrentItem.ToString());
        }

        private void TreeViewOnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SyntaxPanel.SyntaxTypeInfo = e.NewValue as AppTypeInfo;
            _combobox.IsDropDownOpen = false;
        }

        public TypesViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                if (Equals(value, _viewModel)) return;
                _viewModel = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<AppTypeInfo> AppTypeInfos
        {
            get { return _appTypeInfos; }
            set
            {
                if (Equals(value, _appTypeInfos)) return;
                _appTypeInfos = value;
                OnPropertyChanged();
            }
        }

        public ICollectionView View
        {
            get { return _view; }
            set
            {
                if (Equals(value, _view)) return;
                _view = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            View.MoveCurrentToPrevious();
        }

        private void CommandBinding_OnExecuted2(object sender, ExecutedRoutedEventArgs e)
        {
            View.MoveCurrentToNext();

        }

        private void CommandBinding_OnExecuted3(object sender, ExecutedRoutedEventArgs e)
        {
//            Selector.DisplayMode = SyntaxTypeDisplayMode.Flat;
        }

        public static readonly DependencyProperty SyntaxTypesModelProperty = DependencyProperty.Register(
            "SyntaxTypesModel", typeof(SyntaxTypesModel), typeof(MainWindow),
            new PropertyMetadata(default(SyntaxTypesModel)));

        public SyntaxTypesModel SyntaxTypesModel
        {
            get { return (SyntaxTypesModel) GetValue(SyntaxTypesModelProperty); }
            set { SetValue(SyntaxTypesModelProperty, value); }
        }

        private void CommandBinding_OnExecuted4(object sender, ExecutedRoutedEventArgs e)
        {
            //Selector.DisplayMode = SyntaxTypeDisplayMode.Nested;
        }

        private void OpenExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            View.MoveCurrentTo(e.Parameter);
        }
    }

}
