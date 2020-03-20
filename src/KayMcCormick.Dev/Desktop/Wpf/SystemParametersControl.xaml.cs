using System.Collections.Generic ;
using System.Dynamic ;
using System.Linq ;
using System.Reflection ;
using System.Text.RegularExpressions ;
using System.Windows ;
using System.Windows.Controls ;
using NLog ;
using Xceed.Wpf.Toolkit.PropertyGrid ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>Control for viewing WPF related system parameters.</summary>
    public partial class SystemParametersControl
        : UserControl
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>Parameterless constructor.</summary>
        public SystemParametersControl()
        {
            InitializeComponent();

            var sysProp = new SysProp();
            dynamic sysObj = new ExpandoObject();
            var t = typeof(SystemParameters);
            var resKeyProps = t.GetProperties().Where( info => info.PropertyType == typeof(ResourceKey) );
            var propertyDefinitionCollection = new PropertyDefinitionCollection();
            foreach ( var resKeyProp in resKeyProps )
            {
                var propertyInfo = typeof(SysProp).GetProperty( resKeyProp.Name );
                System.Diagnostics.Debug.Assert( propertyInfo != null );
                propertyInfo.SetValue( sysProp, resKeyProp.GetValue( null ) );


                var r = new Regex( @"Key$" );
                var barePropName = r.Replace( resKeyProp.Name, "" );

                var bareProp = t.GetProperty( barePropName, BindingFlags.Static | BindingFlags.Public );
                if ( bareProp == null )
                {
                    Logger.Warn( $"no prop for {resKeyProp.Name}" );
                }
                else
                {
                    var propSysProp = typeof(SysProp).GetProperty( barePropName );
                    System.Diagnostics.Debug.Assert( propSysProp != null );
                    propSysProp.SetValue( sysProp, bareProp.GetValue( null ) );

                    var p = new PropertyDefinition { TargetProperties = { barePropName } };
                    propertyDefinitionCollection.Add( p );

                    var dict = sysObj as IDictionary < string, object >;
                    dict[barePropName] = bareProp.GetValue( null );
                }
            }


            var propertyGrid = new PropertyGrid
                               {
                                   AutoGenerateProperties = false,
                                   SelectedObject         = sysProp,
                                   PropertyDefinitions    = propertyDefinitionCollection
                               };
            Content                          = propertyGrid;
            propertyGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            propertyGrid.VerticalAlignment   = VerticalAlignment.Stretch;

            //Label label = new Label() {Content = LabelStr,};
        }
    }
}
