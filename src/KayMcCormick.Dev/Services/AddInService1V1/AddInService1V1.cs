using System;
using System.AddIn ;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceAddIn1 ;

namespace AddInService1V1
{
    [AddIn("Service1", Description = "test imp", Publisher = "Kay McCormick", Version = "1.0")]
    public class AddInService1V1 : IService1
    {
        #region Implementation of` IService1
        public void PerformFunc1 ( ) { Console.WriteLine ( "hello" ) ; }
        #endregion
    }
}
