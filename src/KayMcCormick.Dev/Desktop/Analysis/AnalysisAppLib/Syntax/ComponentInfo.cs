#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// AnalysisControls
// ComponentInfo.cs
// 
// 2020-03-11-7:05 PM
// 
// ---
#endregion
using System ;
using System.Text.Json.Serialization ;
using AnalysisAppLib.ViewModel ;

namespace AnalysisAppLib.Syntax
{
    public class ComponentInfo
    {
        private bool _isList ;
        private bool _isPersisted ;

        private bool _isSelfOwned ;

        private AppTypeInfo _owningTypeInfo ;
        private string      _propertyName ;
        private AppTypeInfo _typeInfo ;

        public string PropertyName
        {
            get { return _propertyName ; }
            set { _propertyName = value ; }
        }

        [ JsonIgnore ]
        public AppTypeInfo TypeInfo { get { return _typeInfo ; } set { _typeInfo = value ; } }

        public bool IsList { get { return _isList ; } set { _isList = value ; } }

        public Type ComponentType { get { return TypeInfo.Type ; } }

        [ JsonIgnore ]
        public AppTypeInfo OwningTypeInfo
        {
            get { return _owningTypeInfo ; }
            set { _owningTypeInfo = value ; }
        }

        public bool IsSelfOwned { get { return _isSelfOwned ; } set { _isSelfOwned = value ; } }

        public bool IsPersisted { get { return _isPersisted ; } set { _isPersisted = value ; } }

        public MemberBaseDocumentation XmlDoc { get ; set ; }
    }
}