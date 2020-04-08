#if TERMUI
using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.Linq ;
using System.Threading.Tasks ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Lib.Wpf.Command ;
using Terminal.Gui ;
using Attribute = Terminal.Gui.Attribute ;

namespace ConsoleApp1
{
    internal sealed class TermUi
    {
        private const    string         FrameTitle = "Details" ;
        private readonly ModelResources _modelResources ;
        private readonly IEnumerable < IDisplayableAppCommand > _commands ;
        private          ConsoleDriver  _consoleDriver ;
        private          int            _rows ;
        private          int            _cols ;
        private          ListView2      _listView ;
        private          FrameView      _frame ;
        private          TextView       _textView ;
        private          MenuBar        _menuBar ;
        private Toplevel _toplevel ;

        public TermUi ( ModelResources modelResources, IEnumerable<IDisplayableAppCommand> commands )
        {
            _modelResources = modelResources ;
            _commands = commands ;
        }

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

        public void Init ( )
        {
            Application.Init();
            _consoleDriver = Application.Driver;

            _cols = _consoleDriver.Cols;
            _rows = _consoleDriver.Rows;

            _menuBar = CreateMenuBar();

        }
        // ReSharper disable once UnusedMember.Global
        public void InitResourceBrowser( )
        {

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

            MenuItem[] commandsMenuItems = _commands.Select (
                                                             command => new MenuItem (
                                                                                      command
                                                                                         .DisplayName
                                                                                    , ""
                                                                                    , MenuAction(command)
                                                                                     )
                                                            )
                                                    .ToArray ( ) ;
            var commandsMenu = new MenuBarItem ("Commands", commandsMenuItems  );
            var items = new[] { menu1, commandsMenu } ;
            var menuBar = new MenuBar ( items ) ;
            return menuBar ;
        }

        [ NotNull ]
        private Action MenuAction ( IDisplayableAppCommand command )
        {
            return ( ) => command.ExecuteAsync ( ).ContinueWith ( HandleResult ).Wait ( ) ;
        }

        private void HandleResult ( [ NotNull ] Task < IAppCommandResult > obj )
        {
            if ( obj.IsFaulted )
            {

            } else if ( obj.IsCanceled )
            {

            } else if ( obj.IsCompleted )
            {

            }
        }
    }
}
#endif