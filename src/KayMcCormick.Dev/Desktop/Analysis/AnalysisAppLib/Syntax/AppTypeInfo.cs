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
using System.ComponentModel ;
using System.Linq ;
using System.Reflection ;
using System.Runtime.CompilerServices ;
using System.Text.Json.Serialization ;
using System.Text.RegularExpressions ;
using AnalysisAppLib.ViewModel ;
using JetBrains.Annotations ;

namespace AnalysisAppLib.Syntax
{
    /// <summary>
    ///   <para>Represents a Syntax Node type in the application.</para>
    ///   <para></para>
    /// </summary>
    public sealed class AppTypeInfo : INotifyPropertyChanged

    {
        private Type                   _type ;
        private string                 _title ;
        private ObservableCollection < AppMethodInfo >    _factoryMethods = new ObservableCollection < AppMethodInfo > ( ) ;
        private ObservableCollection < ComponentInfo > _components     = new ObservableCollection < ComponentInfo > ( ) ;
        private AppTypeInfo _parentInfo ;
        private readonly ObservableCollection < AppTypeInfo > _subTypeInfos ;
        private int _hierarchyLevel ;
        private uint? _colorValue ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subTypeInfos"></param>
        public AppTypeInfo ( ObservableCollection < AppTypeInfo > subTypeInfos = null )
        {

            if ( subTypeInfos == null )
            {
                subTypeInfos = new ObservableCollection < AppTypeInfo > ( ) ;
            }

            subTypeInfos.CollectionChanged += ( sender , args ) => OnPropertyChanged ( nameof ( SubTypeInfos ) ) ;
            _components.CollectionChanged +=
                ( sender , args ) => OnPropertyChanged ( nameof ( Components ) ) ;
            _factoryMethods.CollectionChanged +=( sender , args ) => OnPropertyChanged ( nameof ( FactoryMethods ) ) ;
            _subTypeInfos = subTypeInfos ;
        }

        /// <summary>
        /// 
        /// </summary>
        public AppTypeInfo ( ) :this(null) {
        }

        /// <summary>
        /// 
        /// </summary>
        public Type Type
        {
            get { return _type ; }
            set
            {
                _type = value ;
                OnPropertyChanged();
                var title = _type.Name.Replace ( "Syntax" , "" ) ;
                Title = Regex.Replace ( title , "([a-z])([A-Z])" , @"$1 $2" ) ;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            get { return _title ; }
            set
            {
                _title = value ;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection < AppTypeInfo > SubTypeInfos { get { return _subTypeInfos ; } }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection < AppMethodInfo > FactoryMethods
        {
            get { return _factoryMethods ; }
            set
            {
                _factoryMethods = value ;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public ObservableCollection < ComponentInfo > Components
        {
            get { return _components ; }
            set
            {
                _components = value ;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ NotNull ] public IEnumerable < ComponentInfo > AllComponents
        {
            get
            {
                var type = ParentInfo ;
                IEnumerable < ComponentInfo > allComponentInfos =
                    Components?.ToList() ?? Enumerable.Empty < ComponentInfo > ( ) ;
                while ( type != null )
                {
                    allComponentInfos = allComponentInfos.Concat (
                                              type.Components.Select (
                                                                                 info
                                                                                     => new
                                                                                        ComponentInfo
                                                                                        {
                                                                                            OwningTypeInfo
                                                                                                = info
                                                                                                   .OwningTypeInfo
                                                                                          , IsList =
                                                                                                info
                                                                                                   .IsList
                                                                                          , IsSelfOwned
                                                                                                = false
                                                                                          , PropertyName
                                                                                                = info
                                                                                                   .PropertyName
                                                                                          , TypeInfo
                                                                                                = info
                                                                                                   .TypeInfo
                                                                                        }
                                                                                )
                                             ) ;
                    type = type.ParentInfo ;
                }

                return allComponentInfos ;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public AppTypeInfo ParentInfo
        {
            get
            {
                return _parentInfo ;
            }
            set
            {
                _parentInfo = value ;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int HierarchyLevel
        {
            get { return _hierarchyLevel ; }
            set { _hierarchyLevel = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public uint ? ColorValue { get { return _colorValue ; } set { _colorValue = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public TypeDocumentation DocInfo { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        private void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class AppMethodInfo
    {
        private MethodInfo _methodInfo ;

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public MethodInfo MethodInfo { get { return _methodInfo ; } set { _methodInfo = value ; } }

        /// <summary>
        /// 
        /// </summary>
        [ CanBeNull ] public Type ReflectedType => MethodInfo.ReflectedType ;

        /// <summary>
        /// 
        /// </summary>
        public Type DeclaringType => MethodInfo.DeclaringType ;

        /// <summary>
        /// 
        /// </summary>
        public string MethodName => MethodInfo.Name ;

        /// <summary>
        /// 
        /// </summary>
        public Type ReturnType => MethodInfo.ReturnType ;
        /// <summary>
        /// 
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
        /// 
        /// </summary>
        public MethodDocumentation XmlDoc { get ; set ; }
    }
}