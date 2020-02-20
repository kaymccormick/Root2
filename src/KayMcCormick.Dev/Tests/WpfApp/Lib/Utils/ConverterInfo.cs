using System ;
using System.Windows.Data ;

namespace Tests.Lib.Utils
{
    internal class ConverterInfo
    {
        public ValueConversionAttribute Attr { get ; set ; }

        public Type ConverterType { get ; set ; }
    }
}