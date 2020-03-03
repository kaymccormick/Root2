using System;
using System.Collections.Generic;
using System.ComponentModel ;
using System.Linq;
using System.Runtime.CompilerServices ;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using ProjLib ;

namespace AnalysisControls
{
    /// <summary>
    /// Interaction logic for SyntaxPanel.xaml
    /// </summary>
    public partial class SyntaxPanel : UserControl, IView <ISyntaxPanelViewModel>
    {
        private ISyntaxPanelViewModel viewModel = new SyntaxPanelViewModel();

        public SyntaxPanel()
        {
            InitializeComponent();
        }

        private void tv_SelectedItemChanged (
            object                                    sender
          , RoutedPropertyChangedEventArgs < object > e
        )
        {
            var view = (CollectionViewSource)TryFindResource("Compilation");
            view.View.MoveCurrentTo(e.NewValue);
        }

        #region Implementation of IView<ISyntaxPanelViewModel>
        public ISyntaxPanelViewModel ViewModel { get => viewModel ; set => viewModel = value ; }
        #endregion
    }

    public interface ISyntaxPanelViewModel : IViewModel, INotifyPropertyChanged
    {
        CompilationUnitSyntax CompilationUnitSyntax { get ; set ; }
    }

    class SyntaxPanelViewModel : ISyntaxPanelViewModel
    {
        private CompilationUnitSyntax compilationUnitSyntax ;
        #region Implementation of ISyntaxPanelViewModel
        public SyntaxPanelViewModel ( CompilationUnitSyntax compilationUnitSyntax = null)
        {
            if ( compilationUnitSyntax != null )
            {
                this.compilationUnitSyntax = compilationUnitSyntax ;
            }
        }

        public CompilationUnitSyntax CompilationUnitSyntax
        {
            get => compilationUnitSyntax ;
            set
            {
                compilationUnitSyntax = value ;
                OnPropertyChanged();
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        protected virtual void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }
    }
}
