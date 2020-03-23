#region header
// Kay McCormick (mccor)
// 
// ManagedProd
// AnalysisControls
// SyntaxPanelViewModel.cs
// 
// 2020-03-03-2:16 PM
// 
// ---
#endregion
using System.ComponentModel ;
using System.Runtime.CompilerServices ;
using System.Runtime.Serialization ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;

namespace AnalysisAppLib.ViewModel
{
    internal sealed class SyntaxPanelViewModel : ISyntaxPanelViewModel
    {
        private CompilationUnitSyntax compilationUnitSyntax ;
        private object                selectedItem ;
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
                if (!ReferenceEquals(compilationUnitSyntax, value))
                {
                    OnPropertyChanging();
                    compilationUnitSyntax = value;
                    OnPropertyChanged();
                }
            }
        }

        public object SelectedItem
        {
            get => selectedItem ;
            set
            {
                if ( ! ReferenceEquals ( selectedItem , value ) )
                {
                    OnPropertyChanging ( ) ;
                    selectedItem = value ;
                    OnPropertyChanged ( ) ;
                }
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        private void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }

        #region Implementation of INotifyPropertyChanging
        public event PropertyChangingEventHandler PropertyChanging ;
        #endregion

        private void OnPropertyChanging ([CallerMemberName] string propertyName = null )
        {
            PropertyChanging?.Invoke ( this , new PropertyChangingEventArgs(propertyName) ) ;
        }

        #region Implementation of ISerializable
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion
    }
}