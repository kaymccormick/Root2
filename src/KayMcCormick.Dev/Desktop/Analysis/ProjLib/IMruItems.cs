using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.IO ;
using Microsoft.VisualStudio.Settings ;
using NLog.LayoutRenderers.Wrappers ;

namespace ProjLib
{
    public interface IMruItemProvider
    {
        List < IMruItem > GetMruItemListFor ( IVsInstance vsInstance ) ;
    }

    public class MruItemProvider : IMruItemProvider
    {

        public List < IMruItem > GetMruItemListFor ( IVsInstance vsInstance )
        {
            var path = Path.Combine ( vsInstance.InstallationPath , vsInstance.ProductPath ) ;
            try
            {
                var ext = ExternalSettingsManager.CreateForApplication ( path ) ;
                var store = ext.GetReadOnlySettingsStore ( SettingsScope.UserSettings ) ;
                var mru = @"MRUItems\{a9c4a31f-f9cb-47a9-abc0-49ce82d0b3ac}\Items" ;
                foreach ( var name in store.GetPropertyNames ( mru ) )
                {
                    var value = store.GetString ( mru , name ) ;
                    Debug.WriteLine ( "Property name: {0}, value: {1}" , name , value ) ;
                    var strings = value.Split ( '|' ) ;
                }
            }
            catch ( Exception ex )
            {
                Debug.WriteLine ( ex ) ;
            }

            return new List < IMruItem > ( ) ;
        }
    }

    public interface IMruItems
    {
        List < IMruItem > GetMruItemList ( ) ;
    }
}