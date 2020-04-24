using System ;
using System.Windows ;
using System.Windows.Data ;
using System.Windows.Markup ;

namespace ProjInterface
{
    public class SemanticModel : MarkupExtension
    {
        #region Overrides of MarkupExtension
        public override object ProvideValue ( IServiceProvider serviceProvider )
        {
            var t =(IProvideValueTarget) serviceProvider.GetService ( typeof ( IProvideValueTarget ) ) ;
            var o = t.TargetObject as BindingBase ;
            var value = o.ProvideValue ( serviceProvider ) ;
            return "" ;
        }
        #endregion
    }
}