#region header
// Kay McCormick (mccor)
// 
// Deployment
// ProjInterface
// Shell.cs
// 
// 2020-03-16-10:00 AM
// 
// ---
#endregion
using System ;
using System.Diagnostics ;
using System.Runtime.InteropServices ;
using System.Threading.Tasks ;
using System.Windows.Interop ;
using KayMcCormick.Dev ;
using KayMcCormick.Lib.Wpf ;

namespace ProjInterface.Explorer
{
    /// <summary>
    ///     Interaction logic for Window1.xaml
    /// </summary>
    public class Shell : HwndHost , IViewWithTitle , IControlView
    {
        internal const int LBN_SELCHANGE   = 0x00000001 , WM_COMMAND    = 0x00000111
                         , LB_GETCURSEL    = 0x00000188 , LB_GETTEXTLEN = 0x0000018A
                         , LB_ADDSTRING    = 0x00000180 , LB_GETTEXT    = 0x00000189
                         , LB_DELETESTRING = 0x00000182 , LB_GETCOUNT   = 0x0000018B ;

        private string _viewTitle = "ConEmu shell" ;
        private int    hostHeight , hostWidth ;


        private IntPtr hwndControl ;
        private IntPtr hwndHost ;

        #region Implementation of IView1
        public string ViewTitle { get { return _viewTitle ; } set { _viewTitle = value ; } }
        #endregion

        [ DllImport ( "user32.dll" , EntryPoint = "CreateWindowEx" , CharSet = CharSet.Unicode ) ]
        internal static extern IntPtr CreateWindowEx (
            int                                          dwExStyle
          , string                                       lpszClassName
          , string                                       lpszWindowName
          , int                                          style
          , int                                          x
          , int                                          y
          , int                                          width
          , int                                          height
          , IntPtr                                       hwndParent
          , IntPtr                                       hMenu
          , IntPtr                                       hInst
          , [ MarshalAs ( UnmanagedType.AsAny ) ] object pvParam
        ) ;

        #region Overrides of HwndHost
        protected override HandleRef BuildWindowCore ( HandleRef hwndParent )
        {
            hwndControl = IntPtr.Zero ;
            hwndHost    = IntPtr.Zero ;

            hwndHost = CreateWindowEx (
                                       0
                                     , "static"
                                     , ""
                                     , WS_CHILD | WS_VISIBLE
                                 ,     0
                                 ,     0
                                 ,     100
                                 ,     100
                                 ,     hwndParent.Handle
                                 ,     ( IntPtr ) HOST_ID
                                 ,     IntPtr.Zero
                                 ,     0
                                      ) ;
#if true
            Task.Run (
                      ( ) => {
                          var insidewndX = "-insidewnd 0x"
                                           + hwndHost.ToString ( "X" )
                                           + @" -loadcfgfile c:\temp\config.xml" ;
                          return Process.Start (
                                                @"c:\OneDrive\apps\ConEmu\app\ConEmu\conemu.exe"
                                              , insidewndX
                                               ) ;
                      }
                     ) ;
#endif
            return new HandleRef ( this , hwndHost ) ;
        }

        internal const int WS_CHILD   = 0x40000000 , WS_VISIBLE = 0x10000000
                         , LBS_NOTIFY = 0x00000001
                         , HOST_ID    = 0x00000002 , LISTBOX_ID = 0x00000001
                         , WS_VSCROLL = 0x00200000
                         , WS_BORDER  = 0x00800000 ;

        protected override void DestroyWindowCore ( HandleRef hwnd ) { }
        #endregion
    }
}