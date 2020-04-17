using System;
using System.Windows.Forms;
using KayMcCormick.Dev.Application ;

namespace BasicWinForms
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // ReSharper disable once UnusedVariable
            using ( var instance = new ApplicationInstance (
                                                            new ApplicationInstance.
                                                                ApplicationInstanceConfiguration (
                                                                                                  LogMethod
                                                                                                , ApplicationInstanceIds
                                                                                                     .BasicWinForms
                                                                                                 )
                                                           ) )
            {


                Application.EnableVisualStyles ( ) ;
                Application.SetCompatibleTextRenderingDefault ( false ) ;
                Application.Run ( new Form1 ( ) ) ;
            }
        }

        private static void LogMethod ( string message )
        {

        }
    }
}
