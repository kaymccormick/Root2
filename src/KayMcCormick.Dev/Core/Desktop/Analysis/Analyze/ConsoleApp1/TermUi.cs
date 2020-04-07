#if TERMUI
using System.Diagnostics ;
using System.Linq ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using Terminal.Gui ;

namespace ConsoleApp1
{
    internal sealed class TermUi
    {
        private const    string         FrameTitle = "Details" ;
        private readonly ModelResources _modelResources ;
        private          ConsoleDriver  _consoleDriver ;
        private          int            _rows ;
        private          int            _cols ;
        private          ListView2      _listView ;
        private          FrameView      _frame ;
        private          TextView       _textView ;
        private          MenuBar        _menuBar ;
        private Toplevel _toplevel ;

        public TermUi ( ModelResources modelResources ) { _modelResources = modelResources ; }

        public Toplevel Toplevel1 { get { return _toplevel ; } set { _toplevel = value ; } }

        private void TerminalResized ( )
        {

            _rows = _consoleDriver.Rows ;
            _cols = _consoleDriver.Cols ;
            var r1 = RecalculateRects ( _cols , _rows , out var r2 ) ;
            _listView.Frame = r1 ;
            _frame.Frame = r2 ;
        }

        public static Rect RecalculateRects ( int width , int height , out Rect r2 )
        {
            var x = 1 ;
            var y = 0 ;
            /* Menu bar occupies row 1 */
            x += 2 ;
            // ReSharper disable once UselessBinaryOperation
            y += 2 ;

            var right = width - 2 ;
            var bottom = y    + ( height - y - 4 ) / 2 ;

            var r1 = Rect.FromLTRB ( x , y , right , bottom ) ;

            y      = bottom + 3 ;
            bottom = y      + 10 ;
            r2     = Rect.FromLTRB ( x , y , right , bottom ) ;
            return r1 ;
        }

        public void Init( )
        {
            Application.Init ( ) ;
            _consoleDriver = Application.Driver ;

            _cols = _consoleDriver.Cols ;
            _rows = _consoleDriver.Rows ;

            _menuBar = CreateMenuBar ( ) ;

            var listViewRect = RecalculateRects ( _cols , _rows , out var textViewRect ) ;

            _listView = new ListView2 (
                                       listViewRect
                                     , _modelResources.AllResourcesCollection.ToList ( )
                                      ) ;


            _frame = new FrameView ( textViewRect , FrameTitle ) ;

            _textView = new TextView
                        {
                            Text = ""
                          , ColorScheme = new ColorScheme
                                          {
                                              Normal = Attribute.Make (
                                                                       Color.BrightMagenta
                                                                     , Color.Black
                                                                      )
                                          }
                        } ;
            _frame.Add ( _textView ) ;
            _listView.SelectedChanged += ( ) => _textView.Text = _listView.List[ _listView.SelectedItem ].ToString ( ) ;

            Application.Driver.SetTerminalResized ( TerminalResized ) ;

            Toplevel1 = Application.Top ;
            Toplevel1.Add ( _menuBar ) ;
            Toplevel1.Add ( _listView ) ;
            Toplevel1.Add ( _frame ) ;
            
        }

        public void Run ( )
        {
            Application.Run();
        }

        [ NotNull ]
        private MenuBar CreateMenuBar ( )
        {
            var quitItem = new MenuItem (
                                         "Quit"
                                       , ""
                                       , ( ) => Process.GetCurrentProcess ( ).Kill ( )
                                        ) ;
            var menuItems = new[] { quitItem } ;
            var menu1 = new MenuBarItem ( "File" , menuItems ) ;

            var items = new[] { menu1 } ;
            var menuBar = new MenuBar ( items ) ;
            return menuBar ;
        }
    }
}
#endif