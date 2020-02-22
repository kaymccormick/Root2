using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KayMcCormick.Dev.Container
{
    public class ContainerAdjunct
    {
        public static List < Type > GetModulesList ( )
        {
            return new List<Type>(new []{ typeof(IdGeneratorModule) });
        }
    }
}
