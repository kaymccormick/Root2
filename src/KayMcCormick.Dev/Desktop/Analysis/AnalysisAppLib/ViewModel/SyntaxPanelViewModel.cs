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
    /// <summary>
    /// 
    /// </summary>
    public sealed class SyntaxPanelViewModel : ISyntaxPanelViewModel , INotifyPropertyChanged
    {
        private CompilationUnitSyntax _compilationUnitSyntax ;
        private object                _selectedItem ;

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged ;
        public object InstanceObjectId { get; set; }

        #region Implementation of INotifyPropertyChanging
        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging ;
        #endregion

        #region Implementation of ISerializable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion

        [ NotifyPropertyChangedInvocator ]
        private void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }

        private void OnPropertyChanging ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanging?.Invoke ( this , new PropertyChangingEventArgs ( propertyName ) ) ;
        }

        #region Implementation of ISyntaxPanelViewModel
        /// <summary>
        /// 
        /// </summary>
        public SyntaxPanelViewModel ( ) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compilationUnitSyntax"></param>
        public SyntaxPanelViewModel ( [ CanBeNull ] CompilationUnitSyntax compilationUnitSyntax = null )
        {
            if ( compilationUnitSyntax != null )
            {
                this._compilationUnitSyntax = compilationUnitSyntax ;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public CompilationUnitSyntax CompilationUnitSyntax
        {
            get { return _compilationUnitSyntax ; }
            set
            {
                if ( ReferenceEquals ( _compilationUnitSyntax , value ) )
                {
                    return ;
                }

                OnPropertyChanging ( ) ;
                _compilationUnitSyntax = value ;
                OnPropertyChanged ( ) ;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public object SelectedItem
        {
            get { return _selectedItem ; }
            set
            {
                if ( ReferenceEquals ( _selectedItem , value ) )
                {
                    return ;
                }

                OnPropertyChanging ( ) ;
                _selectedItem = value ;
                OnPropertyChanged ( ) ;
            }
        }
        #endregion
    }
}