#if TERMUI
using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.Linq ;
using System.Threading ;
using System.Threading.Tasks ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Command ;
using KayMcCormick.Lib.Wpf.Command ;
using Terminal.Gui ;
using Attribute = Terminal.Gui.Attribute ;

namespace ConsoleApp1
{
    internal sealed class TermUi
    {
        private readonly TaskFactory                            factory ;
        private const    string                                 FrameTitle = "Details" ;
        private readonly IEnumerable < IDisplayableAppCommand > _commands ;
#pragma warning disable 649
        private readonly ModelResources                         _modelResources ;
#pragma warning restore 649
        private          int                                    _cols ;
        private          ConsoleDriver                          _consoleDriver ;
        private          FrameView                              _frame ;
        private          ListView2                              _listView ;
        private          MenuBar                                _menuBar ;
        private          int                                    _rows ;
        private          TextView                               _textView ;
        private          Toplevel                               _toplevel ;
        // ReSharper disable once NotAccessedField.Local
        private          Task                                   _commandTask ;
        private          Action < string >                      _commandOutputAction ;

        public TermUi (
            //ModelResources                                     modelResources
          [ NotNull ] IEnumerable < IDisplayableAppCommand > commands
          , TaskFactory                                        factory = null
        )
        {
          //  _modelResources = modelResources ;
            this.factory    = factory ;
            if ( this.factory == null )
            {
                this.factory = new TaskFactory (
                                                CancellationToken.None
                                              , TaskCreationOptions.AttachedToParent
                                              , TaskContinuationOptions.None
                                              , TaskScheduler.Current
                                               ) ;
            }

            var commandsary = commands as IDisplayableAppCommand[] ?? commands.ToArray ( ) ;
            _commands = commandsary ;
            if ( ! commandsary.Any ( ) )
            {
                throw new InvalidOperationException ( "No commands" ) ;
            }
        }

        public Toplevel Toplevel1 { get { return _toplevel ; } set { _toplevel = value ; } }

        public IEnumerable < IDisplayableAppCommand > Commands { get { return _commands ; } }

        private void TerminalResized ( )
        {
            _rows = _consoleDriver.Rows ;
            _cols = _consoleDriver.Cols ;
            var r1 = RecalculateRects ( _cols , _rows , out var r2 ) ;
            _listView.Frame = r1 ;
            _frame.Frame    = r2 ;
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
            Application.Init ( ) ;
            _consoleDriver = Application.Driver ;

            _cols = _consoleDriver.Cols ;
            _rows = _consoleDriver.Rows ;

            _menuBar = CreateMenuBar ( ) ;
            var window = new Window ( "MyApp" )
                         {
                             X = 1 , Y = 1 , Width = Dim.Fill ( ) , Height = Dim.Fill ( )
                         } ;
            _commandOutputAction = ShowCommandOutputView ( out var view ) ;
            window.Add ( view ) ;
            InitTopLevel ( window ) ;
        }

        // ReSharper disable once UnusedMember.Global
        public void InitResourceBrowser ( )
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
            _listView.SelectedChanged += ( )
                => _textView.Text = _listView.List[ _listView.SelectedItem ].ToString ( ) ;

            Application.Driver.SetTerminalResized ( TerminalResized ) ;
        }

        private void InitTopLevel ( View w )
        {
            Toplevel1 = Application.Top ;
            Toplevel1.Add ( _menuBar ) ;
            Toplevel1.Add ( w ) ;
            // Toplevel1.Add ( _listView ) ;
            // Toplevel1.Add ( _frame ) ;
        }

        public void Run ( ) { Application.Run ( ) ; }

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

            var list = new List < MenuItem > ( ) ;
            foreach ( var command in Commands )
            {
                list.Add ( new MenuItem ( command.DisplayName , "" , MenuAction ( command ) ) ) ;
            }

            var commandsMenuItems = list.ToArray ( ) ;
            var commandsMenu = new MenuBarItem ( "Commands" , commandsMenuItems ) ;
            var items = new[] { menu1 , commandsMenu } ;
            var menuBar = new MenuBar ( items ) ;
            return menuBar ;
        }

        [ NotNull ]
        private Action MenuAction ( IBaseLibCommand command )
        {
            return ( ) => {
                var outputFunc = _commandOutputAction ;
                command.Argument = outputFunc ;
                DebugUtils.WriteLine ( "Executing async command" ) ;
                _commandTask = command.ExecuteAsync ( ).ContinueWith ( HandleResult ) ;
                DebugUtils.WriteLine ( "Returning from lambda" ) ;
            } ;
        }

        [ NotNull ]
        private Action < string > ShowCommandOutputView ( [ NotNull ] out View view )
        {
            var outputView = new TextView { CanFocus = false } ;
            // var viewFrame = new FrameView ( "Output" ) { outputView } ;

            view = outputView ;
            return o => factory.StartNew(() => outputView.Text += "\r\n" + o) ;
        }

        private static void HandleResult ( [ NotNull ] Task < IAppCommandResult > obj )
        {
            if ( obj.IsFaulted )

            {
                DebugUtils.WriteLine ( $"faulted: {obj.Exception}" ) ;
            }
            else if ( obj.IsCanceled )
            {
                DebugUtils.WriteLine ( "cancelled" ) ;
            }
            else if ( obj.IsCompleted )
            {
                DebugUtils.WriteLine ( "completed" ) ;
            }
        }
    }

    public class CommandOutput : View

    {
        #region Overrides of View
        public override void Redraw ( Rect region ) { }
        #endregion
    }
}
#endif