#region header
// Kay McCormick (mccor)
// 
// WpfApp2
// WpfApp2
// PropertyCellTemplateSelector.cs
// 
// 2020-02-10-7:44 PM
// 
// ---
#endregion
using System ;
using System.Windows ;
using System.Windows.Controls ;
using NLog ;

namespace WpfApp2
{
    public class PropertyCellTemplateSelector : DataTemplateSelector
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public PropertyCellTemplateSelector (
            LogProperty              property
          , Func < string , object > findResource
        )
        {
            this.Property = property ;
            _findResource = findResource ;
        }

        public           LogProperty              Property ;
        private readonly Func < string , object > _findResource ;

        public override DataTemplate SelectTemplate ( object item , DependencyObject container )
        {
            var item2 = ( LogEntry ) item ;
            DataTemplate theTemplate = null ;
            try
            {
                var propertyInfo = typeof ( LogEntry ).GetProperty ( Property.Name ) ;
                if ( propertyInfo != null )
                {
                    propertyInfo.GetValue ( item ) ;
                }
                else
                {
                    if ( item2 != null && item2.ContainsKey ( Property.Name ) )
                    {
                    }
                    else
                    {
                        theTemplate = ( DataTemplate ) _findResource ( "PropertyNoValueTemplate" ) ;
                    }
                }
            }
            catch ( Exception ex )
            {
                Logger.Warn ( ex , ex.Message ) ;
                return ( DataTemplate ) _findResource ( "PropertyValueExceptionTemplate" ) ;
            }

            if ( theTemplate != null )
            {
                return theTemplate ;
            }

            var template = _findResource ( $"{Property.Name}PropertyValueTemplate" ) ;
            if ( template != null )
            {
                return ( DataTemplate ) template ;
            }

            template = _findResource ( "PropertyValueTemplate" ) ;
            var dataTemplate = ( DataTemplate ) template ;
            return dataTemplate ;

            //return base.SelectTemplate ( item , container ) ;
        }
    }
}