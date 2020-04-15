#if TERMUI
#region header
// Kay McCormick (mccor)
// 
// Analysis
// ConsoleApp1
// ListView2Base.cs
// 
// 2020-04-03-5:48 AM
// 
// ---
#endregion
using System.Collections.Generic ;
using System.Linq ;
using JetBrains.Annotations ;
using Terminal.Gui ;

namespace ConsoleApp1
{
    internal class ListView2Base < T > : ListView
        where T : class , IEnumerable < T >
    {
        private readonly List < ItemData < T > > _itemDatas = new List < ItemData < T > > ( 20 ) ;
        private readonly ListWrapper < T >       _source ;

        protected ListView2Base ( Rect rect , [ NotNull ] IEnumerable < T > list ) : base (
                                                                                           rect
                                                                                         , new ListWrapper
                                                                                               < T > (
                                                                                                      list
                                                                                                         .ToList ( )
                                                                                                     )
                                                                                          )
        {
            _source = ( ListWrapper < T > ) Source ;
            for ( var i = 0 ; i < _source.Count ; i ++ )
            {
                _itemDatas.Add ( new ItemData < T > ( ) ) ;
            }
        }

        public List < T > List { get { return _source.List ; } }

        public override bool ProcessKey ( KeyEvent kb )
        {
            switch ( kb.Key )
            {
                case Key.CursorRight :
                {
                    var selectedItem = SelectedItem ;
                    var e = List[ selectedItem ] ;
                    var d = _itemDatas[ selectedItem ] ;
                    if ( d.Expanded )
                    {
                        return true ;
                    }

                    d.Expanded = true ;
                    if ( d.RemovedChildren != null )
                    {
                        List.InsertRange ( selectedItem + 1 , d.RemovedChildren ) ;
                        d.SubtreeCount = d.RemovedChildren.Count ;
                    }
                    else
                    {
                        d.InsertedChildren = e.ToList ( ) ;
                        List.InsertRange ( selectedItem + 1 , d.InsertedChildren ) ;
                        var insertedChildrenCount = d.InsertedChildren.Count ;
                        d.SubtreeCount = insertedChildrenCount ;
                    }

                    var c = d.Container ;
                    while ( c != null )
                    {
                        ItemData < T > itemData = null ;
                        for ( var i = selectedItem ; i >= 0 ; i -- )
                        {
                            if ( ReferenceEquals ( List[ i ] , c ) )
                            {
                                itemData = _itemDatas[ i ] ;
                                break ;
                            }
                        }

                        if ( itemData != null )
                        {
                            itemData.SubtreeCount += d.SubtreeCount ;
                            c                     =  itemData.Container ;
                        }
                        else
                        {
                            c = null ;
                        }
                    }

                    _itemDatas.InsertRange (
                                            selectedItem + 1
                                          , Enumerable
                                           .Repeat ( 1 , d.SubtreeCount )
                                           .Select ( x => new ItemData < T > { Container = e } )
                                           ) ;

                    Source       = _source ;
                    SelectedItem = selectedItem ;
                    // foreach ( var r in x.Children )
                    // {
                    return true ;
                    // }
                }
                case Key.CursorLeft :
                {
                    var selectedItem = SelectedItem ;

                    var d = _itemDatas[ selectedItem ] ;
                    if ( ! d.Expanded )
                    {
                        return true ;
                    }

                    var c = d.Container ;
                    while ( c != null )
                    {
                        ItemData < T > itemData = null ;
                        for ( var i = selectedItem ; i >= 0 ; i -- )
                        {
                            if ( ReferenceEquals ( List[ i ] , c ) )
                            {
                                itemData = _itemDatas[ i ] ;
                                break ;
                            }
                        }

                        if ( itemData != null )
                        {
                            itemData.SubtreeCount -= d.SubtreeCount ;
                            c                     =  itemData.Container ;
                        }
                        else
                        {
                            c = null ;
                        }
                    }

                    d.RemovedChildren = List.GetRange ( selectedItem + 1 , d.SubtreeCount ) ;
                    List.RemoveRange ( selectedItem + 1 , d.SubtreeCount ) ;
                    d.Expanded   = false ;
                    Source       = _source ;
                    SelectedItem = selectedItem ;
                    return true ;
                }
                default : return base.ProcessKey ( kb ) ;
            }
        }

        private sealed class ItemData < T2 >
        {
            public T2 Container { get ; set ; }

            public bool Expanded { get ; set ; }

            public List < T2 > InsertedChildren { get ; set ; }

            public int SubtreeCount { get ; set ; }

            public List < T2 > RemovedChildren { get ; set ; }
        }
    }
}
#endif