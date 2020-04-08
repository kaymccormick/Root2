using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.Linq ;
using System.Reflection ;
using System.Runtime.CompilerServices ;
using System.Text.Json.Serialization ;
using System.Text.RegularExpressions ;
using System.Windows.Markup ;
using AnalysisAppLib.Syntax ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis.CSharp ;

namespace AnalysisAppLib
{
    /// <summary>
    ///     <para>Represents a Syntax Node type in the application.</para>
    ///     <para></para>
    /// </summary>
    public sealed class AppTypeInfo : INotifyPropertyChanged

    {
        private string _elementName;
        private readonly SyntaxFieldCollection _fields = new SyntaxFieldCollection();
        private AppTypeInfoCollection _subTypeInfos;
        private uint? _colorValue;

        private SyntaxComponentCollection _components =
            new SyntaxComponentCollection();

        private ObservableCollection<AppMethodInfo> _factoryMethods =
            new ObservableCollection<AppMethodInfo>();

        private int _hierarchyLevel;
        private AppTypeInfo _parentInfo;
        private string _title;
        private Type _type;
        private List<string> _kinds= new List < string > ();


        /// <summary>
        /// </summary>
        /// <param name="subTypeInfos"></param>
        public AppTypeInfo(AppTypeInfoCollection subTypeInfos = null)
        {
            if (subTypeInfos == null)
            {
                subTypeInfos = new AppTypeInfoCollection();
            }

            _components.CollectionChanged += (sender, args)
                => OnPropertyChanged(nameof(Components));
            _factoryMethods.CollectionChanged += (sender, args)
                => OnPropertyChanged(nameof(FactoryMethods));
            _subTypeInfos = subTypeInfos;
        }

        /// <summary>
        /// </summary>
        public AppTypeInfo() : this(null) { }

        /// <summary>
        /// </summary>
        public Type Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged();
                var title = _type.Name.Replace("Syntax", "");
                Title = Regex.Replace(title, "([a-z])([A-Z])", @"$1 $2");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<string> Kinds
        {
            get { return _kinds ; }
        }

        /// <summary>
        /// </summary>
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [JsonIgnore]
        public AppTypeInfoCollection SubTypeInfos
        {
            get { return _subTypeInfos; }
            set { _subTypeInfos = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<string> SubTypeNames => SubTypeInfos.Cast<AppTypeInfo>().Select(st => st.Type.Name).ToList();

        /// <summary>
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ObservableCollection<AppMethodInfo> FactoryMethods
        {
            get { return _factoryMethods; }
            set
            {
                _factoryMethods = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [JsonIgnore]
        public SyntaxComponentCollection Components
        {
            get { return _components; }
            set
            {
                _components = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [NotNull]
        public IEnumerable<ComponentInfo> AllComponents
        {
            get
            {
                var type = ParentInfo;
                var allComponentInfos =
                    Components?.ToList() ?? Enumerable.Empty<ComponentInfo>();
                while (type != null)
                {
                    allComponentInfos = allComponentInfos.Concat(
                                                                  type.Components.Select(
                                                                                          info
                                                                                              => new
                                                                                                 ComponentInfo
                                                                                              {
                                                                                                  OwningTypeInfo
                                                                                                         = info
                                                                                                            .OwningTypeInfo
                                                                                                   ,
                                                                                                  IsList
                                                                                                         = info
                                                                                                            .IsList
                                                                                                   ,
                                                                                                  IsSelfOwned
                                                                                                         = false
                                                                                                   ,
                                                                                                  PropertyName
                                                                                                         = info
                                                                                                            .PropertyName
                                                                                                   ,
                                                                                                  TypeInfo
                                                                                                         = info
                                                                                                            .TypeInfo
                                                                                              }
                                                                                         )
                                                                 );
                    type = type.ParentInfo;
                }

                return allComponentInfos;
            }
        }

        /// <summary>
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [JsonIgnore]
        public AppTypeInfo ParentInfo
        {
            get { return _parentInfo; }
            set
            {
                _parentInfo = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int HierarchyLevel
        {
            get { return _hierarchyLevel; }
            set { _hierarchyLevel = value; }
        }

        /// <summary>
        /// </summary>
        public uint? ColorValue { get { return _colorValue; } set { _colorValue = value; } }

        /// <summary>
        /// </summary>
        public TypeDocumentation DocInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ElementName { get { return _elementName; } set { _elementName = value; } }


        /// <summary>
        /// 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public SyntaxFieldCollection Fields
        {
            get { return _fields; }
        }

        /// <summary>
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SyntaxFieldCollection : IList , ICollection, IEnumerable
    {
        private IList _listImplementation = new List < SyntaxFieldInfo > ( ) ;
        #region Implementation of IEnumerable
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator ( ) { return _listImplementation.GetEnumerator ( ) ; }
        #endregion
        #region Implementation of ICollection
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        void ICollection.CopyTo ( Array array , int index ) { _listImplementation.CopyTo ( array , index ) ; }

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get { return _listImplementation.Count ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public object SyncRoot
        {
            get { return _listImplementation.SyncRoot ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSynchronized
        {
            get { return _listImplementation.IsSynchronized ; }
        }
        #endregion
        #region Implementation of IList
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int Add ( object value ) { return _listImplementation.Add ( value ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains ( object value ) { return _listImplementation.Contains ( value ) ; }

        /// <summary>
        /// 
        /// </summary>
        public void Clear ( ) { _listImplementation.Clear ( ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int IndexOf ( object value ) { return _listImplementation.IndexOf ( value ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Insert ( int index , object value ) { _listImplementation.Insert ( index , value ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void Remove ( object value ) { _listImplementation.Remove ( value ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt ( int index ) { _listImplementation.RemoveAt ( index ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public object this [ int index ]
        {
            get { return _listImplementation[ index ] ; }
            set { _listImplementation[ index ] = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly
        {
            get { return _listImplementation.IsReadOnly ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsFixedSize
        {
            get { return _listImplementation.IsFixedSize ; }
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    [ContentProperty("Kinds")]
    public class SyntaxFieldInfo
    {
        private bool _override ;
        /// <summary>
        /// 
        /// </summary>
        public SyntaxFieldInfo ( ) {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="typeName"></param>
        /// <param name="kinds"></param>
        public SyntaxFieldInfo ( string name , string typeName, params string[] kinds  )
        {
            _name = name ;
            _typeName = typeName ;

            foreach ( var kind in kinds )
            {
                var k = (SyntaxKind)Enum.Parse(typeof(SyntaxKind), kind);
                _kinds.Add ( k) ;
            }
            
        }

        private string _name ;
        private string _typeName ;
        private readonly SyntaxKindCollection _kinds = new SyntaxKindCollection ();
        private bool _optional ;

        /// <summary>
        /// 
        /// </summary>
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Content)]
        public string TypeName  { get { return _typeName ; } set { _typeName = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get { return _name ; } set { _name = value ; } }

        /// <summary>
        /// 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public SyntaxKindCollection Kinds
        {
            get { return _kinds ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Type Type { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<AppTypeInfo> Types { get ;  } = new List < AppTypeInfo > ();

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        [ NotNull ] public IEnumerable< Type > ClrTypes
        {
            get { return Types.Select ( typ => typ.Type ) ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Override { get { return _override ; } set { _override = value ; } }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public bool Optional { get { return _optional ; } set { _optional = value ; } }

        public override string ToString ( )
        {
            return $"TypeName: {TypeName}, Name: {Name}, Kinds: {Kinds}, Type: {Type}, Types: {Types}, ClrTypes: {ClrTypes}, Override: {Override}, Optional: {Optional}" ;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class SyntaxKindCollection : IList, ICollection, IEnumerable
    {
        private readonly IList _listImplementation = new List < SyntaxKind > ( ) ;
        #region Implementation of IEnumerable
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator ( ) { return _listImplementation.GetEnumerator ( ) ; }
        #endregion
        #region Implementation of ICollection
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo ( Array array , int index ) { _listImplementation.CopyTo ( array , index ) ; }

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get { return _listImplementation.Count ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public object SyncRoot
        {
            get { return _listImplementation.SyncRoot ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSynchronized
        {
            get { return _listImplementation.IsSynchronized ; }
        }
        #endregion
        #region Implementation of IList
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int Add ( object value ) { return _listImplementation.Add ( value ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains ( object value ) { return _listImplementation.Contains ( value ) ; }

        /// <summary>
        /// 
        /// </summary>
        public void Clear ( ) { _listImplementation.Clear ( ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int IndexOf ( object value ) { return _listImplementation.IndexOf ( value ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Insert ( int index , object value ) { _listImplementation.Insert ( index , value ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void Remove ( object value ) { _listImplementation.Remove ( value ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt ( int index ) { _listImplementation.RemoveAt ( index ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public object this [ int index ]
        {
            get { return _listImplementation[ index ] ; }
            set { _listImplementation[ index ] = value ; }
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get { return _listImplementation.IsReadOnly ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsFixedSize
        {
            get { return _listImplementation.IsFixedSize ; }
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class AppTypeInfoCollection : IList, ICollection, IEnumerable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        public AppTypeInfoCollection ( IList list ) { _list = list ; }

        /// <summary>
        /// 
        /// </summary>
        public AppTypeInfoCollection ( )
        {
            _list = new List < AppTypeInfo > ( ) ;
        }

        private readonly IList _list ;
        #region Implementation of IEnumerable
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator ( ) { return _list.GetEnumerator ( ) ; }
        #endregion
        #region Implementation of ICollection
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo ( Array array , int index ) { _list.CopyTo ( array , index ) ; }

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get { return _list.Count ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public object SyncRoot
        {
            get { return _list.SyncRoot ; }
        }

        /// <inheritdoc />
        public bool IsSynchronized
        {
            get { return _list.IsSynchronized ; }
        }
        #endregion
        #region Implementation of IList
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int Add ( object value ) { return _list.Add ( value ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains ( object value ) { return _list.Contains ( value ) ; }

        /// <summary>
        /// 
        /// </summary>
        public void Clear ( ) { _list.Clear ( ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int IndexOf ( object value ) { return _list.IndexOf ( value ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Insert ( int index , object value ) { _list.Insert ( index , value ) ; }

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void Remove ( object value ) { _list.Remove ( value ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt ( int index ) { _list.RemoveAt ( index ) ; }

        /// <inheritdoc />
        public object this [ int index ]
        {
            get { return _list[ index ] ; }
            set { _list[ index ] = value ; }
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get { return _list.IsReadOnly ; }
        }

        /// <inheritdoc />
        public bool IsFixedSize
        {
            get { return _list.IsFixedSize ; }
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class SyntaxComponentCollection : ObservableCollection < ComponentInfo >
    {
    }

    public sealed class AppMethodInfo
    {
        private MethodInfo _methodInfo;

        /// <summary>
        /// </summary>
        [JsonIgnore]
        public MethodInfo MethodInfo { get { return _methodInfo; } set { _methodInfo = value; } }

        /// <summary>
        /// </summary>
        [CanBeNull] public Type ReflectedType { get { return MethodInfo.ReflectedType; } }

        /// <summary>
        /// </summary>
        public Type DeclaringType { get { return MethodInfo.DeclaringType; } }

        /// <summary>
        /// </summary>
        public string MethodName { get { return MethodInfo.Name; } }

        /// <summary>
        /// </summary>
        public Type ReturnType { get { return MethodInfo.ReturnType; } }

        /// <summary>
        /// </summary>
        public IEnumerable<AppParameterInfo> Parameters
        {
            get
            {
                return MethodInfo.GetParameters()
                                 .Select(
                                         (info, i) => new AppParameterInfo
                                                      {
                                                          Index = i
                                                         ,
                                                          ParameterType = info.ParameterType
                                                         ,
                                                          Name = info.Name
                                                         ,
                                                          IsOptional = info.IsOptional
                                                      }
                                        );
            }
        }

        /// <summary>
        /// </summary
        [JsonIgnore]
        public MethodDocumentation XmlDoc { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AppParameterInfo
    {
        private int    _index;
        private bool   _isOptional;
        private string _name;
        private Type   _parameterType;

        /// <summary>
        /// 
        /// </summary>
        public Type ParameterType
        {
            get { return _parameterType; }
            set { _parameterType = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsOptional { get { return _isOptional; } set { _isOptional = value; } }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get { return _name; } set { _name = value; } }

        /// <summary>
        /// 
        /// </summary>
        public int Index { get { return _index; } set { _index = value; } }
    }
}