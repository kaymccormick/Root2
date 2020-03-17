#region header
// Kay McCormick (mccor)
// 
// Deployment
// ProjInterface
// DockWindowViewModel.cs
// 
// 2020-03-16-10:04 AM
// 
// ---
#endregion
using System ;
using System.Collections ;
using System.Collections.Generic ;
using Autofac.Features.Metadata ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Interfaces ;
using KayMcCormick.Lib.Wpf ;

namespace ProjInterface
{
    public class DockWindowViewModel : IViewModel
    {
        private readonly IEnumerable < Meta < Lazy < IView1 > > > _views ;

        public DockWindowViewModel (IEnumerable<Meta <Lazy <IView1> > >  views)
        {
            _views = views ;
        }

        public IEnumerable < Meta < Lazy < IView1 > > > Views => _views ;
    }
}