﻿#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// AnalysisControls
// HandleExceptionImpl.cs
// 
// 2020-03-22-7:19 AM
// 
// ---
#endregion
using System ;
using System.IO ;
using System.Runtime.Serialization ;
using System.Runtime.Serialization.Formatters.Binary ;
using System.Windows ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// </summary>
    public sealed class HandleExceptionImpl : IHandleException
    {
        #region Implementation of IHandleException
        /// <summary>
        /// </summary>
        /// <param name="exception"></param>
        public void HandleException ( [ CanBeNull ] Exception exception )
        {
            if ( exception != null )
            {
                try
                {
                    var s1 = new StreamWriter ( @"C:\data\logs\stack.txt" ) ;
                    if ( exception.InnerException != null )
                    {
                        s1.Write ( exception.InnerException.StackTrace ) ;
                    }

                    s1.Close ( ) ;
                    IFormatter formatter = new BinaryFormatter ( ) ;
                    Stream stream = new FileStream (
                                                    @"C:\data\logs\MyFile.bin"
                                                  , FileMode.Create
                                                  , FileAccess.Write
                                                  , FileShare.None
                                                   ) ;
                    formatter.Serialize ( stream , exception ) ;
                    stream.Close ( ) ;
                    //
                    //     var x = new XmlSerializer(exception.GetType());
                    // using ( var f = File.OpenWrite ( @"c:\data\logs\exception.xml" ) )
                    // {
                    //     x.Serialize ( f , exception ) ;
                    //     f.Flush();
                    // }
                }
                catch ( Exception ex )
                {
                    DebugUtils.WriteLine ( ex.ToString ( ) ) ;
                }
            }

            var w = new Window
                    {
                        MinWidth  = 300
                      , MinHeight = 100
                      //, Content   = new ExceptionInfo { DataContext = exception }
                    } ;
            w.ShowDialog ( ) ;
        }
        #endregion
    }
}