#region header
// Kay McCormick (mccor)
// 
// Proj
// ProjLib
// MruItemProvider.cs
// 
// 2020-02-20-6:19 PM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.IO ;
using JetBrains.Annotations ;
using Microsoft.VisualStudio.Settings ;

namespace ProjLib
{
    public class MruItemProvider : IMruItemProvider
    {

        public List < IMruItem > GetMruItemListFor ( [ NotNull ] IVsInstance vsInstance )
        {
            if ( vsInstance == null )
            {
                throw new ArgumentNullException ( nameof ( vsInstance ) ) ;
            }

            if (vsInstance.InstallationPath == null)
            {
                throw new ArgumentException("installation path is null");
            }
            if (vsInstance.ProductPath == null)
            {
                throw new ArgumentException("product path is null");
            }

            var mruItemListFor = new List<IMruItem>();;
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
                    var mruitem = new MruItem ( strings[ 0 ], strings[3] ) ;
                    mruItemListFor.Add ( mruitem ) ;
                }
            }
            catch ( Exception ex )
            {
                Debug.WriteLine ( ex ) ;
            }

            
            return mruItemListFor ;
        }
    }
}