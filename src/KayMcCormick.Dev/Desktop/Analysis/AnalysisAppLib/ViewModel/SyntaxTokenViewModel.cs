﻿#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// AnalysisAppLib
// SyntaxTokenViewModel.cs
// 
// 2020-03-29-6:31 AM
// 
// ---
#endregion
using System ;
using System.Collections.ObjectModel ;
using System.Runtime.Serialization ;
using AnalysisAppLib.Syntax ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using NLog ;

namespace AnalysisAppLib.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    [ UsedImplicitly ]
    public sealed class SyntaxTokenViewModel : ISyntaxTokenViewModel
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        public object InstanceObjectId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SyntaxTokenViewModel ( )
        {
            foreach ( SyntaxKind value in Enum.GetValues ( typeof ( SyntaxKind ) ) )
            {
                SyntaxToken ? token = null ;
                if ( SyntaxFacts.IsAnyToken ( value ) )
                {
                    try
                    {
                        token = SyntaxFactory.Token ( value ) ;
                    }
                    catch ( Exception ex )
                    {
                        Logger.Debug ( ex , ex.Message ) ;
                    }
                }

                SyntaxItems.Add (
                                 new SyntaxItem
                                 {
                                     SyntaxKind = value , Token = token , RawKind = ( ushort ) value
                                 }
                                ) ;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection < SyntaxItem > SyntaxItems { get ; } =
            new ObservableCollection < SyntaxItem > ( ) ;

        #region Implementation of ISerializable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion
    }
}