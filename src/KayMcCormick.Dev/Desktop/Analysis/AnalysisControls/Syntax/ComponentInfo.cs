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
namespace AnalysisControls.Syntax
{
    public class ComponentInfo
    {
        private string      _propertyName ;
        private AppTypeInfo _typeInfo ;
        private bool        _isList ;
        public  string      PropertyName { get => _propertyName ; set => _propertyName = value ; }

        public AppTypeInfo TypeInfo { get => _typeInfo ; set => _typeInfo = value ; }

        public bool IsList { get => _isList ; set => _isList = value ; }
    }
}