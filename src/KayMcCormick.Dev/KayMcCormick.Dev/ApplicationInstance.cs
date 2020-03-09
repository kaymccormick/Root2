using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Core ;

namespace KayMcCormick.Dev
{
    public class ApplicationInstance
    {
        private ILifetimeScope lifetimeScope ;
        private List <IModule> _modules = new List < IModule > ();
        private ContainerBuilder builder ;

        public ApplicationInstance ( ) {
        }

        public void Initialize ( )
        {

        }

        public void AddModule ( IModule appModule ) { _modules.Add ( appModule ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ILifetimeScope GetLifetimeScope ( )
        {
            if(lifetimeScope != null)
            {
                return lifetimeScope ;

            }

            builder = new ContainerBuilder ( ) ;
            foreach ( var module in _modules ) { builder.RegisterModule ( module ) ; }

            var container = builder.Build ( ) ;
            return container.BeginLifetimeScope ( ) ;
        }
    }

}
