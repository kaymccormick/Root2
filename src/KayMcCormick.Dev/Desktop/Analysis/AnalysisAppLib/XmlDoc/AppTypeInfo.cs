using JetBrains.Annotations;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Windows.Markup;

namespace AnalysisAppLib.XmlDoc
{
    /// <summary>
    ///     <para>Represents a Syntax Node type in the application.</para>
    ///     <para></para>
    /// </summary>
    [TypeDescriptionProvider(typeof(AppTypeInfoTypeDescriptionProvider))]
    [TypeConverter(typeof(AppTypeInfoTypeConverter))]
    public sealed class AppTypeInfo : INotifyPropertyChanged

    {
        private DateTime _createdDateTime= DateTime.Now;
        private DateTime _updatedDateTime;
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
        private readonly List<string> _kinds= new List < string > ();
        private object _keyValue ;


        /// <summary>
        /// </summary>
        /// <param name="subTypeInfos"></param>
        public AppTypeInfo(  AppTypeInfoCollection subTypeInfos = null)
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
        public AppTypeInfo( ) : this(null) { }

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
        // ReSharper disable once UnusedMember.Global
        [ NotNull ] public List<string> SubTypeNames
        {
            get
            {
                return SubTypeInfos
                                   .Select ( st => st.Type.Name )
                                   .ToList ( ) ;
            }
        }

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
            // ReSharper disable once UnusedMember.Global
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
        // ReSharper disable once UnusedMember.Global
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
        /// 
        /// </summary>
        public DateTime UpdatedDateTime
        {
            // ReSharper disable once UnusedMember.Global
            get { return _updatedDateTime ; }
            set { _updatedDateTime = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public DateTime CreatedDateTime
        {
            get { return _createdDateTime ; }
            set { _createdDateTime = value ; }
        }

        /// <summary>
        /// Key for the Type that isn't the object itself.
        /// </summary>
        public object KeyValue { get { return _keyValue ; } set { _keyValue = value ; } }

        /// <summary>
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            UpdatedDateTime = DateTime.Now;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class SyntaxFieldCollection : IList , ICollection, IEnumerable
    {
        private readonly IList _listImplementation = new List < SyntaxFieldInfo > ( ) ;
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
    [TypeConverter(typeof(SyntaxFieldInfoTypeConverter))]
    public sealed class SyntaxFieldInfo
    {
        private string _clrTypeName;
        private bool _override;
        /// <summary>
        /// 
        /// </summary>
        public SyntaxFieldInfo()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="typeName"></param>
        /// <param name="kinds"></param>
        public SyntaxFieldInfo(string name, string typeName, [ NotNull ] params string[] kinds)
        {
            _name = name;
            _typeName = typeName;

            foreach (var kind in kinds)
            {
                var k = (SyntaxKind)Enum.Parse(typeof(SyntaxKind), kind);
                _kinds.Add(k);
            }

        }

        private string _name;
        private string _typeName;
        private readonly SyntaxKindCollection _kinds = new SyntaxKindCollection();
        private bool _optional;
        private Type _type;
        private string _elementTypeMetadataName ;
        private bool _isCollection ;

        /// <summary>
        /// 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public string TypeName { get { return _typeName; } set { _typeName = value; } }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get { return _name; } set { _name = value; } }

        /// <summary>
        /// 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public SyntaxKindCollection Kinds
        {
            get { return _kinds; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ValueSerializer(typeof(SyntaxFieldTypeValueSerializer))]
        [TypeConverter(typeof(SyntaxFieldTypeTypeConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Type Type
        {
            get { return _type ; }
            set
            {
                _type = value ;
                _clrTypeName = _type?.AssemblyQualifiedName ;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<AppTypeInfo> Types { get; } = new List<AppTypeInfo>();

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        [NotNull]
        public IEnumerable<Type> ClrTypes
        {
            get { return Types.Select(typ => typ.Type); }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Override { get { return _override; } set { _override = value; } }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public bool Optional { get { return _optional; } set { _optional = value; } }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public string ClrTypeName { get { return _clrTypeName; } set { _clrTypeName = value; } }

        /// <summary>
        /// 
        /// </summary>
        public string ElementTypeMetadataName { get { return _elementTypeMetadataName ; } set { _elementTypeMetadataName = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public bool IsCollection { get { return _isCollection ; } set { _isCollection = value ; } }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"TypeName: {TypeName}, Name: {Name}, Kinds: {Kinds}, Type: {Type}, Types: {Types}, ClrTypes: {ClrTypes}, Override: {Override}, Optional: {Optional}";
        }
    }

    /// <inheritdoc />
    public sealed class SyntaxFieldTypeTypeConverter : TypeConverter
    {
        #region Overrides of TypeConverter
        /// <inheritdoc />
        public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType )
        {
            if(destinationType == typeof(string))
            {
                return true ;
            }
            return base.CanConvertTo ( context , destinationType ) ;
        }

        /// <inheritdoc />
        public override object ConvertTo (
            ITypeDescriptorContext context
          , CultureInfo            culture
          , object                 value
          , Type                   destinationType
        )
        {
            if ( value is Type t )
            {
                return "boo" ;
            }
            return base.ConvertTo ( context , culture , value , destinationType ) ;
        }
        #endregion
    }

    /// <inheritdoc />
    public sealed class SyntaxFieldTypeValueSerializer : ValueSerializer
    {
        #region Overrides of ValueSerializer
        /// <inheritdoc />
        public override bool CanConvertToString ( object value , IValueSerializerContext context )
        {
            if ( value is Type )
            {
                return true ;
            }
            return base.CanConvertToString ( value , context ) ;
        }

        /// <inheritdoc />
        public override string ConvertToString ( object value , IValueSerializerContext context )
        {
            if ( value is Type t )
            {
                if ( t.IsGenericType )
                {
                    var t2 = t.GetGenericTypeDefinition ( ) ;
                    // ReSharper disable once PossibleNullReferenceException
                    var nt = t2.FullName.Replace ( "`1" , "" ) ;
                    var x = t.GetGenericArguments ( )[ 0 ].FullName ;
                    return $"{nt}<{x}>" ;
                }

                return t.FullName ;
            }
            return base.ConvertToString ( value , context ) ;
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class SyntaxKindCollection : IList, ICollection, IEnumerable
    {
        private readonly IList _list = new List < SyntaxKind > ( ) ;

        /// <inheritdoc />
        public IEnumerator GetEnumerator ( ) => _list.GetEnumerator ( ) ;
        /// <inheritdoc />
        public void CopyTo ( Array array , int index ) => _list.CopyTo ( array , index ) ;

        /// <inheritdoc />
        public int Count => _list.Count ;

        /// <inheritdoc />
        public object SyncRoot => _list.SyncRoot ;

        /// <inheritdoc />
        public bool IsSynchronized => _list.IsSynchronized ;

        /// <inheritdoc />
        public int Add ( object value ) => _list.Add ( value ) ;
        /// <inheritdoc />
        public bool Contains ( object value ) => _list.Contains ( value ) ;
        /// <inheritdoc />
        public void Clear ( ) => _list.Clear ( ) ;
        /// <inheritdoc />
        public int IndexOf ( object value ) => _list.IndexOf ( value ) ;
        /// <inheritdoc />
        public void Insert ( int index , object value ) => _list.Insert ( index , value ) ;
        /// <inheritdoc />
        public void Remove ( object value ) => _list.Remove ( value ) ;
        /// <inheritdoc />
        public void RemoveAt ( int index ) => _list.RemoveAt ( index ) ;
        
        /// <inheritdoc />
        public object this [ int index ]
        {
            get => _list[ index ] ;
            set => _list[ index ] = value ;
        }


        /// <inheritdoc />
        public bool IsReadOnly => false ;

        /// <inheritdoc />
        public bool IsFixedSize => _list.IsFixedSize ;
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class AppTypeInfoCollection : IList < AppTypeInfo >
      , ICollection < AppTypeInfo >
      , IEnumerable < AppTypeInfo >, ICollection
    {
        private readonly IList < AppTypeInfo > _listImplementation = new List < AppTypeInfo > ();
        private object _syncRoot ;
        private bool _isSynchronized ;
        #region Implementation of IEnumerable
        /// <inheritdoc />
        public IEnumerator < AppTypeInfo > GetEnumerator ( ) { return _listImplementation.GetEnumerator ( ) ; }

        IEnumerator IEnumerable.GetEnumerator ( ) { return ( ( IEnumerable ) _listImplementation ).GetEnumerator ( ) ; }
        #endregion
        #region Implementation of ICollection<AppTypeInfo>
        /// <inheritdoc />
        public void Add ( AppTypeInfo item ) { _listImplementation.Add ( item ) ; }

        /// <inheritdoc />
        public void Clear ( ) { _listImplementation.Clear ( ) ; }

        /// <inheritdoc />
        public bool Contains ( AppTypeInfo item ) { return _listImplementation.Contains ( item ) ; }

        /// <inheritdoc />
        public void CopyTo ( AppTypeInfo[] array , int arrayIndex ) { _listImplementation.CopyTo ( array , arrayIndex ) ; }

        /// <inheritdoc />
        public bool Remove ( AppTypeInfo item ) { return _listImplementation.Remove ( item ) ; }

        /// <inheritdoc />
        public void CopyTo ( Array array , int index ) { }

        /// <inheritdoc />
        public int Count
        {
            get { return _listImplementation.Count ; }
        }

        /// <inheritdoc />
        public object SyncRoot { get { return _syncRoot ; } }

        /// <inheritdoc />
        public bool IsSynchronized { get { return _isSynchronized ; } }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get { return _listImplementation.IsReadOnly ; }
        }
        #endregion
        #region Implementation of IList<AppTypeInfo>
        /// <inheritdoc />
        public int IndexOf ( AppTypeInfo item ) { return _listImplementation.IndexOf ( item ) ; }

        /// <inheritdoc />
        public void Insert ( int index , AppTypeInfo item ) { _listImplementation.Insert ( index , item ) ; }

        /// <inheritdoc />
        public void RemoveAt ( int index ) { _listImplementation.RemoveAt ( index ) ; }

        /// <inheritdoc />
        public AppTypeInfo this [ int index ]
        {
            get { return _listImplementation[ index ] ; }
            set { _listImplementation[ index ] = value ; }
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class SyntaxComponentCollection : ObservableCollection < ComponentInfo >
    {
    }

    /// <summary>
    /// Wrapper class around <see cref="MethodInfo"/>. Supplies extra information if necessary and other potentially =
    /// useful information and facilities. Method parameters are individually wrapped in <see cref="AppParameterInfo"/>
    /// </summary>
    public sealed class AppMethodInfo
    {
        private MethodInfo _methodInfo;

        /// <summary>
        /// </summary>
        [JsonIgnore]
        public MethodInfo MethodInfo { get { return _methodInfo; } set { _methodInfo = value; } }

        /// <summary>
        /// </summary>
        [CanBeNull] [ UsedImplicitly ] public Type ReflectedType { get { return MethodInfo.ReflectedType; } }

        /// <summary>
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        [ CanBeNull ] public Type DeclaringType { get { return MethodInfo.DeclaringType; } }

        /// <summary>
        /// </summary>
        [ NotNull ] [ UsedImplicitly ] public string MethodName { get { return MethodInfo.Name; } }

        /// <summary>
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        [ NotNull ] public Type ReturnType { get { return MethodInfo.ReturnType; } }

        /// <summary>
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        [ NotNull ] public IEnumerable<AppParameterInfo> Parameters
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
        /// </summary>
        [JsonIgnore]
        public MethodDocumentation XmlDoc { get; set; }
    }

    /// <summary>
    /// See <see cref="AppMethodInfo"/>.
    /// </summary>
    public sealed class AppParameterInfo
    {
        private int    _index;
        private bool   _isOptional;
        private string _name;
        private Type   _parameterType;

        /// <summary>
        /// Type of parameter.
        /// </summary>
        public Type ParameterType
        {
            [ UsedImplicitly ] get { return _parameterType; }
            set { _parameterType = value; }
        }

        /// <summary>
        /// Is parameter optional?
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public bool IsOptional { get { return _isOptional; } set { _isOptional = value; } }

        /// <summary>
        /// Name of parameter
        /// </summary>
        public string Name { [ UsedImplicitly ] get { return _name; } set { _name = value; } }

        /// <summary>
        /// Zero-based index of parameter.
        /// </summary>
        public int Index { [ UsedImplicitly ] get { return _index; } set { _index = value; } }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AppTypeInfoKey : IComparable < AppTypeInfoKey >
    {
        private string _unqualifiedTypeName ;

        /// <summary>
        /// 
        /// </summary>
        public string StringValue
        {
            get { return _unqualifiedTypeName ; }
            set { _unqualifiedTypeName = value ; }
        }

        #region Relational members
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo ( AppTypeInfoKey other )
        {
            if ( ReferenceEquals ( this , other ) )
            {
                return 0 ;
            }

            if ( ReferenceEquals ( null , other ) )
            {
                return 1 ;
            }

            return string.Compare ( StringValue , other.StringValue , StringComparison.Ordinal ) ;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unqualifiedTypeName"></param>
        public AppTypeInfoKey ( string unqualifiedTypeName )
        {
            StringValue = unqualifiedTypeName ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public AppTypeInfoKey ( Type type )
        {
            StringValue = type.Name ;

        }

        #region Equality members
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals ( AppTypeInfoKey other )
        {
            return StringValue == other.StringValue ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals ( object obj )
        {
            return obj is AppTypeInfoKey other && Equals ( other ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode ( )
        {
            return ( StringValue != null ? StringValue.GetHashCode ( ) : 0 ) ;
        }
        #endregion
    }
}