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
using System.Collections.ObjectModel ;
using System.Reflection ;
using System.Text.RegularExpressions ;

namespace AnalysisControls.Syntax
{
    /// <summary>
    ///   <para>Represents a Syntax Node type in the application.</para>
    ///   <para></para>
    /// </summary>
    public class AppTypeInfo
    {
        private Type                   _type ;
        private string                 _title ;
        private List < MethodInfo >    _factoryMethods = new List < MethodInfo > ( ) ;
        private List < ComponentInfo > _components     = new List < ComponentInfo > ( ) ;

        public Type Type
        {
            get => _type ;
            set
            {
                _type = value ;
                var title = _type.Name.Replace ( "Syntax" , "" ) ;
                Title = Regex.Replace ( title , "([a-z])([A-Z])" , @"$1 $2" ) ;
            }
        }

        public string Title { get => _title ; set => _title = value ; }

        public ObservableCollection < AppTypeInfo > SubTypeInfos { get ; } =
            new ObservableCollection < AppTypeInfo > ( ) ;

        public List < MethodInfo > FactoryMethods
        {
            get => _factoryMethods ;
            set => _factoryMethods = value ;
        }

        public List < ComponentInfo > Components
        {
            get => _components ;
            set => _components = value ;
        }
    }
}