using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using KayMcCormick.Dev.Application ;

namespace BasicWinForms
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
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
