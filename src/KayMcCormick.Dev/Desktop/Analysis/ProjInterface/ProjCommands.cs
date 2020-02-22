using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input ;

namespace ProjInterface
{
    public static class ProjCommands
    {
     public static RoutedUICommand LoadSolution = new RoutedUICommand("Load Solution", nameof(LoadSolution), typeof ( ProjCommands ));   
    }
}
