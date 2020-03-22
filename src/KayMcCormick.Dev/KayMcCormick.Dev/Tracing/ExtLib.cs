using System.Runtime.InteropServices ;

namespace KayMcCormick.Dev.Tracing
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExtLib
    {

        [ DllImport ( "Compatlib1.dll" ) ]
        public static extern ulong
            fnEventWriteSETUP_LOGGING_EVENT_AssumeEnabled ([MarshalAs( UnmanagedType.LPWStr)] string message ) ;
    }
}