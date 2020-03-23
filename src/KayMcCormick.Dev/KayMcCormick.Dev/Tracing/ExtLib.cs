using System.Runtime.InteropServices ;
#if false
namespace KayMcCormick.Dev.Tracing
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExtLib
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [ DllImport ( "Compatlib1.dll" ) ]
        public static extern ulong
            fnEventWriteSETUP_LOGGING_EVENT_AssumeEnabled ([MarshalAs( UnmanagedType.LPWStr)] string message ) ;
    }
}
#endif