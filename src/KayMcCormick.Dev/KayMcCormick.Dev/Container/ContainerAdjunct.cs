using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KayMcCormick.Dev.Container
{
    /// <summary>
    /// 
    /// </summary>
    public class ContainerAdjunct
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List < Type > GetModulesList ( )
        {
            return new List<Type>(new []{ typeof(IdGeneratorModule) });
        }
    }
}
