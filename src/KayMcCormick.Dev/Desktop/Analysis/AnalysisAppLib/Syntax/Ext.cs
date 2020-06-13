#region header
// Kay McCormick (mccor)
// 
// WpfApp2
// CodeAnalysisApp1
// Ext.cs
// 
// 2020-02-14-1:05 AM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using System.IO ;
using System.Linq ;
using JetBrains.Annotations ;
using KayMcCormick.Dev;
using Microsoft.CodeAnalysis ;

namespace AnalysisAppLib.Syntax
{
    /// <summary>Extension class</summary>
    // ReSharper disable once UnusedType.Global
    public static class Ext
    {
        // public static implicit operator IEnumerable < ITypeSymbol ? > ( this TypeInfo me )
        //     => new[] { me.Type , me.ConvertedType } ;
        /// <summary>Types the infos.</summary>
        /// <param name="me">Me.</param>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for TypeInfos
        [ JetBrains.Annotations.NotNull ]
        // ReSharper disable once UnusedMember.Global
        public static IEnumerable < ITypeSymbol > TypeInfos ( this TypeInfo me )
        {
            return new[] { me.Type , me.ConvertedType } ;
        }

        /// <summary>Deconstructs the specified type1.</summary>
        /// <param name="me">Me.</param>
        /// <param name="type1">The type1.</param>
        /// <param name="type2">The type2.</param>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for Deconstruct
        // ReSharper disable once UnusedMember.Global
        public static void Deconstruct (
            this TypeInfo    me
          , [ CanBeNull ] out  ITypeSymbol type1
          , [ CanBeNull ] out  ITypeSymbol type2
        )
        {
            type1 = me.Type ;
            type2 = me.ConvertedType ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        [ JetBrains.Annotations.NotNull ]
        // ReSharper disable once UnusedMember.Global
        public static string ShortenedPath ( [ JetBrains.Annotations.NotNull ] this Document doc )
        {
            if ( doc == null )
            {
                throw new ArgumentNullException ( nameof ( doc ) ) ;
            }

            if ( doc.FilePath == null )
            {
                return "";
            }

            var strings = doc.FilePath.Split ( Path.DirectorySeparatorChar ) ;
            const int numElems = 4 ;
            var p = strings.ToList ( )
                           .GetRange (
                                      strings.Length < numElems ? 0 : strings.Length - numElems
                                    , strings.Length >= numElems ? 3 : strings.Length
                                     ) ;
            return string.Join ( Path.DirectorySeparatorChar.ToString ( ) , p ) ;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        [ JetBrains.Annotations.NotNull ]
        public static string RelativePath ( [ JetBrains.Annotations.NotNull ] this Document doc )
        {
            if ( doc.FilePath != null )
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                return GetRelativePath ( doc.Project.FilePath , doc.FilePath ) ;
            }

            throw new AppInvalidOperationException ( ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relativeTo"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        [ JetBrains.Annotations.NotNull ]
        public static string GetRelativePath ( [ JetBrains.Annotations.NotNull ] string relativeTo , [ JetBrains.Annotations.NotNull ] string path )
        {
            var uri = new Uri ( relativeTo ) ;
            var rel = Uri
                     .UnescapeDataString ( uri.MakeRelativeUri ( new Uri ( path ) ).ToString ( ) )
                     .Replace ( Path.AltDirectorySeparatorChar , Path.DirectorySeparatorChar ) ;
            if ( rel.Contains ( Path.DirectorySeparatorChar.ToString ( ) ) == false )
            {
                rel = $".{Path.DirectorySeparatorChar}{rel}" ;
            }

            return rel ;
        }
    }
}