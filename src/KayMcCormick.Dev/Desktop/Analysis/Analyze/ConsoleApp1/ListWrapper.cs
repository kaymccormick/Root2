#region header
// Kay McCormick (mccor)
// 
// Analysis
// ConsoleApp1
// ListWrapper.cs
// 
// 2020-04-03-5:48 AM
// 
// ---
#endregion
using System ;
using System.Collections ;
using System.Collections.Generic ;
using JetBrains.Annotations ;
using NStack ;
using Terminal.Gui ;

namespace ConsoleApp1
{
    public sealed class ListWrapper < T > : IListDataSource
    {
        private readonly int      count ;
        private readonly BitArray marks ;

        public ListWrapper ( [ NotNull ] List < T > source )
        {
            count = source.Count ;
            marks = new BitArray ( count ) ;
            List  = source ;
        }

        public List < T > List { get ; }

        public int Count { get { return List.Count ; } }

        public void Render (
            [ NotNull ] ListView container
          , ConsoleDriver        driver
          , bool                 marked
          , int                  item
          , int                  col
          , int                  line
          , int                  width
        )
        {
            container.Move ( col , line ) ;
            var t = List[ item ] ;

            RenderUstr (
                        driver
                      , t.ToString ( )
                      , col
                      , line
             ,          width
                       ) ;
        }

        public bool IsMarked ( int item )
        {
            if ( item    >= 0
                 && item < count )
            {
                return marks[ item ] ;
            }

            return false ;
        }

        public void SetMark ( int item , bool value )
        {
            if ( item    >= 0
                 && item < count )
            {
                marks[ item ] = value ;
            }
        }

        private void RenderUstr (
            ConsoleDriver       driver
          , [ NotNull ] ustring ustr
            // ReSharper disable once UnusedParameter.Local
          , int col
            // ReSharper disable once UnusedParameter.Local
          , int line
          , int width
        )
        {
            var byteLen = ustr.Length ;
            var used = 0 ;
            for ( var i = 0 ; i < byteLen ; )
            {
                var (rune , size) = Utf8.DecodeRune ( ustr , i , i - byteLen ) ;
                var columnWidth = Rune.ColumnWidth ( rune ) ;
                if ( used + columnWidth >= width )
                {
                    break ;
                }

                driver.AddRune ( rune ) ;
                used += columnWidth ;
                i    += size ;
            }

            for ( ; used < width ; used ++ )
            {
                driver.AddRune ( ' ' ) ;
            }
        }
    }
}