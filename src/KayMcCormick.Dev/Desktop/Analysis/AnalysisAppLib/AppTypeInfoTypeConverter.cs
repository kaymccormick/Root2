#region header
// Kay McCormick (mccor)
// 
// Analysis
// AnalysisAppLib
// AppTypeInfoTypeConverter.cs
// 
// 2020-04-08-4:21 PM
// 
// ---
#endregion
using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Diagnostics ;
using KayMcCormick.Dev ;
using Microsoft.CodeAnalysis.CSharp ;

namespace AnalysisAppLib
{
    public class AppTypeInfoTypeConverter : TypeConverter
    {
        #region Overrides of TypoeConverter
        #region Overrides of TypeConverter
        public override bool GetCreateInstanceSupported ( ITypeDescriptorContext context )
        {
            return true ;
        }
        #endregion
        #region Overrides of TypeConverter
        public override object CreateInstance (
            ITypeDescriptorContext context
          , IDictionary            propertyValues
        )
        {
            var r = new AppTypeInfo();
            if ( propertyValues.Contains ( "Type" ) )
            {
                var type = propertyValues[ "Type" ] ;
                if ( type is Type t )
                {
                    r.Type = t ;
                }
            }

            return r ;
        }
        #endregion

        public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType )
        {
            DebugUtils.WriteLine ($"Can convert from {context?.Instance?.GetType()} to {destinationType}?"  );
            return base.CanConvertTo ( context , destinationType ) ;
        }
        #endregion
        #region Overrides of TypeConverter
        public override StandardValuesCollection GetStandardValues (
            ITypeDescriptorContext context
        )
        {
            DebugUtils.WriteLine ( "hello from kay" ) ;
            var cSharpInfo = new AppTypeInfo { ElementName = "PredefinedNode", HierarchyLevel = 0, Title = "CSharp Node", Type = typeof(CSharpSyntaxNode) } ;
            return new StandardValuesCollection(new ArrayList {cSharpInfo});
        }
        #endregion
    }
}