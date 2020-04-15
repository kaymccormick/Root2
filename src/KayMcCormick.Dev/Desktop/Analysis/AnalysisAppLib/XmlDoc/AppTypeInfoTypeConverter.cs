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
using System.ComponentModel ;
using AnalysisAppLib.Syntax ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using Microsoft.CodeAnalysis.CSharp ;

namespace AnalysisAppLib.XmlDoc
{
    /// <summary>
    /// Prototypical Type converter for <see cref="AppTypeInfo"/>
    /// </summary>
    public sealed class AppTypeInfoTypeConverter : TypeConverter
    {
        #region Overrides of TypoeConverter
        #region Overrides of TypeConverter
        /// <inheritdoc />
        public override bool GetCreateInstanceSupported ( ITypeDescriptorContext context )
        {
            return true ;
        }
        #endregion
        #region Overrides of TypeConverter
        /// <inheritdoc />
        [ NotNull ]
        public override object CreateInstance (
            ITypeDescriptorContext context
          , IDictionary            propertyValues
        )
        {
            var r = new AppTypeInfo();
            if ( ! propertyValues.Contains ( "Type" ) )
            {
                return r ;
            }

            var type = propertyValues[ "Type" ] ;
            if ( type is Type t )
            {
                r.Type = t ;
            }

            return r ;
        }
        #endregion

        /// <inheritdoc />
        public override bool CanConvertTo ( [ CanBeNull ] ITypeDescriptorContext context , Type destinationType )
        {
            //DebugUtils.WriteLine ($"Can convert from {context?.Instance?.GetType()} to {destinationType}?"  );
            return base.CanConvertTo ( context , destinationType ) ;
        }
        #endregion
        #region Overrides of TypeConverter
        /// <inheritdoc />
        [ NotNull ]
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