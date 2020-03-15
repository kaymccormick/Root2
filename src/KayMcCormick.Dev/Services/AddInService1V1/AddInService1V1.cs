using System ;
using System.AddIn ;
using System.Collections.Generic ;
using System.Linq ;
using System.Net.Sockets ;
using System.Text ;
using System.Threading ;
using System.Threading.Tasks ;
using JetBrains.Annotations ;
using ServiceAddIn1 ;

namespace AddInService1V1
{
    [ AddIn (
                "Service1"
              , Description = "test imp"
              , Publisher   = "Kay McCormick"
              , Version     = "1.0"
            ) ]
    [UsedImplicitly]
    public class AddInService1V1 : IService1
    {
        private Thread _thread ;
        #region Implementation of IService1
        public bool Start ( )
        {
            _thread = new Thread ( ThreadProc ) ;

            
            return true ;
        }

        private void ThreadProc ( ) { }

        public bool Stop ( ) { return false ; }

        public bool Pause ( ) { return false ; }

        public bool Continue ( ) { return false ; }

        public bool Shutdown ( ) { return false ; }

        public void PerformFunc1 ( ) { Console.WriteLine ( "hello" ) ; }
        #endregion
    }
}