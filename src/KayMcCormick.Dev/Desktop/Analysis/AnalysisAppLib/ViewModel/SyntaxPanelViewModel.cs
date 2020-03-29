﻿#region header
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
    public sealed class SyntaxPanelViewModel : ISyntaxPanelViewModel , INotifyPropertyChanged
    {
        private CompilationUnitSyntax compilationUnitSyntax ;
        private object                selectedItem ;

        public event PropertyChangedEventHandler PropertyChanged ;

        #region Implementation of INotifyPropertyChanging
        public event PropertyChangingEventHandler PropertyChanging ;
        #endregion

        #region Implementation of ISerializable
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
        public SyntaxPanelViewModel ( ) { }

        public SyntaxPanelViewModel ( CompilationUnitSyntax compilationUnitSyntax = null )
        {
            if ( compilationUnitSyntax != null )
            {
                this.compilationUnitSyntax = compilationUnitSyntax ;
            }
        }

        public CompilationUnitSyntax CompilationUnitSyntax
        {
            get { return compilationUnitSyntax ; }
            set
            {
                if ( ! ReferenceEquals ( compilationUnitSyntax , value ) )
                {
                    OnPropertyChanging ( ) ;
                    compilationUnitSyntax = value ;
                    OnPropertyChanged ( ) ;
                }
            }
        }

        public object SelectedItem
        {
            get { return selectedItem ; }
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
    }
}