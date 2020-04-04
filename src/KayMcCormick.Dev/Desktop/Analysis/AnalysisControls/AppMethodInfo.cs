#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// AnalysisControls
// AppTypeInfo.cs
// 
// 2020-03-11-7:01 PM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using System.Linq ;
using System.Reflection ;
using System.Text.Json.Serialization ;
using AnalysisAppLib ;
using AnalysisAppLib.Syntax ;
using AnalysisControls.ViewModel ;
using JetBrains.Annotations ;

namespace AnalysisControls
{
    /// <summary>
    /// </summary>
    public sealed class AppMethodInfo
    {
        private MethodInfo _methodInfo ;

        /// <summary>
        /// </summary>
        [ JsonIgnore ]
        public MethodInfo MethodInfo { get { return _methodInfo ; } set { _methodInfo = value ; } }

        /// <summary>
        /// </summary>
        [ CanBeNull ] public Type ReflectedType { get { return MethodInfo.ReflectedType ; } }

        /// <summary>
        /// </summary>
        public Type DeclaringType { get { return MethodInfo.DeclaringType ; } }

        /// <summary>
        /// </summary>
        public string MethodName { get { return MethodInfo.Name ; } }

        /// <summary>
        /// </summary>
        public Type ReturnType { get { return MethodInfo.ReturnType ; } }

        /// <summary>
        /// </summary>
        public IEnumerable < AppParameterInfo > Parameters
        {
            get
            {
                return MethodInfo.GetParameters ( )
                                 .Select (
                                          ( info , i ) => new AppParameterInfo
                                                          {
                                                              Index         = i
                                                            , ParameterType = info.ParameterType
                                                            , Name          = info.Name
                                                            , IsOptional    = info.IsOptional
                                                          }
                                         ) ;
            }
        }

        /// <summary>
        /// </summary>
        public MethodDocumentation XmlDoc { get ; set ; }
    }
}