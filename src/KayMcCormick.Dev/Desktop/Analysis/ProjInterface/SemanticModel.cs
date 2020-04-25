using System ;
using System.Windows.Data ;
using System.Windows.Markup ;
using JetBrains.Annotations ;

namespace ProjInterface
{
    public class SemanticModel : MarkupExtension
    {
        #region Overrides of MarkupExtension
        [ NotNull ]
        public override object ProvideValue ( IServiceProvider serviceProvider )
        {
            var t =(IProvideValueTarget) serviceProvider.GetService ( typeof ( IProvideValueTarget ) ) ;
            var o = t.TargetObject as BindingBase ;
            if ( o != null )
            {
                var value = o.ProvideValue ( serviceProvider ) ;
            }

            return "" ;
        }
        #endregion
    }
}