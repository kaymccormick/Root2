using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac ;

namespace KayMcCormick.Dev
{
    /// <summary>
    /// 
    /// </summary>
    public class InfraModule : Module
    {
        #region Overrides of Module
        protected override void Load ( ContainerBuilder builder )
        {
            builder.RegisterModule < IdGeneratorModule > ( ) ;
            base.Load ( builder ) ;
        }
        #endregion
    }
}
