using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Windows.Markup;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis.CSharp;

namespace AnalysisAppLib.Syntax
{
    /// <summary>
    ///     <para>Represents a Syntax Node type in the application.</para>
    ///     <para></para>
    /// </summary>
    [TypeConverter(typeof(AppTypeInfoTypeConverter))]
    public sealed class AppTypeInfo : INotifyPropertyChanged

    {
        private int _id;
        private DateTime _createdDateTime = DateTime.Now;
        private DateTime _updatedDateTime;
        private string _elementName;
        private SyntaxFieldCollection _fields = new SyntaxFieldCollection();
        private AppTypeInfoCollection _subTypeInfos;
        private uint? _colorValue;

        private ObservableCollection<AppMethodInfo> _factoryMethods =
            new ObservableCollection<AppMethodInfo>();

        private int _hierarchyLevel;
        private AppTypeInfo _parentInfo;
        private string _title;
        private Type _type;
        private readonly List<string> _kinds = new List<string>();
        private object _keyValue;
        private int _version;


        /// <summary>
        /// </summary>
        /// <param name="subTypeInfos"></param>
        public AppTypeInfo(AppTypeInfoCollection subTypeInfos = null)
        {
            if (subTypeInfos == null) subTypeInfos = new AppTypeInfoCollection();

            _factoryMethods.CollectionChanged += (sender, args)
                => OnPropertyChanged(nameof(FactoryMethods));
            _subTypeInfos = subTypeInfos;
        }

        /// <summary>
        /// </summary>
        public AppTypeInfo() : this(null)
        {
        }

        /// <summary>
        /// CLR Type
        /// </summary>
        [JsonIgnore]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public AppClrType AppClrType { get; set; }

        /// <summary>
        /// </summary>
        [NotMapped]
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
            get { return _kinds; }
        }

        /// <summary>
        /// 
        /// </summary>
        public SyntaxKindCollection SyntaxKinds { get; } = new SyntaxKindCollection();

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
        /// <returns></returns>
        public override string ToString()
        {
            return $"AppTypeInfo[{Title}]";
        }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        [NotNull]
        public List<string> SubTypeNames
        {
            get
            {
                return SubTypeInfos
                    .Select(st => st.Type.Name)
                    .ToList();
            }
        }

        /// <summary>
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
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
        [Browsable(false)]
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
        public uint? ColorValue
        {
            get { return _colorValue; }
            set { _colorValue = value; }
        }

        /// <summary>
        /// </summary>
        [NotMapped]
        [Browsable(false)]
        public ICodeElementDocumentation DocInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public string ElementName
        {
            get { return _elementName; }
            set { _elementName = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public SyntaxFieldCollection Fields
        {
            get { return _fields; }
            set { _fields = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime UpdatedDateTime
        {
            // ReSharper disable once UnusedMember.Global
            get { return _updatedDateTime; }
            set { _updatedDateTime = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public DateTime CreatedDateTime
        {
            get { return _createdDateTime; }
            set { _createdDateTime = value; }
        }

        /// <summary>
        /// Key for the Type that isn't the object itself.
        /// </summary>
        [NotMapped]
        public object KeyValue
        {
            get { return _keyValue; }
            set { _keyValue = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Version
        {
            get { return _version; }
            set { _version = value; }
        }

        /// <summary>
        /// Primary key
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEnumerable<AppTypeInfo> AllTypes { get; set; }

        /// <summary>
        /// 
        /// </summary>
                [Browsable(false)]
        [JsonIgnore]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public ITypesViewModel Model { get; set; }

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
    /// Representation of CLR type.
    /// </summary>
    public class AppClrType
    {
        private int _id;

        /// <summary>
        /// Primary key
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Type full name
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Assembly qualified name
        /// </summary>
        public string AssemblyQualifiedName { get; set; }

        /// <summary>
        /// Assembly
        /// </summary>
        public AppAssembly Assembly { get; set; }

        /// <summary>
        /// Base type
        /// </summary>
        public AppClrType BaseType { get; set; }

        /// <summary>
        /// Is Abstract
        /// </summary>
        public bool IsAbstract { get; set; }

        /// <summary>
        /// Is Class
        /// </summary>
        public bool IsClass { get; set; }

        /// <summary>
        /// Is constructed generic type
        /// </summary>
        public bool IsConstructedGenericType { get; set; }

        /// <summary>
        /// Is generic type definition
        /// </summary>
        public bool IsGenericTypeDefinition { get; set; }

        /// <summary>
        /// Is generic type
        /// </summary>
        public bool IsGenericType { get; set; }
    }

    /// <summary>
    /// Assembly
    /// </summary>
    public class AppAssembly
    {
        private int _id;


        /// <summary>
        /// Primary key
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class SyntaxFieldCollection : IList, ICollection, IEnumerable, IList<SyntaxFieldInfo>,
        ICollection<SyntaxFieldInfo>, IEnumerable<SyntaxFieldInfo>
    {
        private readonly IList _listImplementation;

        /// <summary>
        /// 
        /// </summary>
        public SyntaxFieldCollection()
        {
            _listImplementation = (IList) _generic;
        }

        private readonly IList<SyntaxFieldInfo> _generic = new List<SyntaxFieldInfo>();

        #region Implementation of IEnumerable

        IEnumerator<SyntaxFieldInfo> IEnumerable<SyntaxFieldInfo>.GetEnumerator()
        {
            yield break;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return _listImplementation.GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        void ICollection.CopyTo(Array array, int index)
        {
            _listImplementation.CopyTo(array, index);
        }

        /// <inheritdoc />
        public bool Remove(SyntaxFieldInfo item)
        {
            return _generic.Remove(item);
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get { return _listImplementation.Count; }
        }

        /// <summary>
        /// 
        /// </summary>
        public object SyncRoot
        {
            get { return _listImplementation.SyncRoot; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSynchronized
        {
            get { return _listImplementation.IsSynchronized; }
        }

        #endregion

        #region Implementation of IList

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int Add(object value)
        {
            return _listImplementation.Add(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(object value)
        {
            return _listImplementation.Contains(value);
        }

        /// <inheritdoc />
        public void Add(SyntaxFieldInfo item)
        {
            _generic.Add(item);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            _listImplementation.Clear();
        }

        /// <inheritdoc />
        public bool Contains(SyntaxFieldInfo item)
        {
            return _generic.Contains(item);
        }

        /// <inheritdoc />
        public void CopyTo(SyntaxFieldInfo[] array, int arrayIndex)
        {
            _generic.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int IndexOf(object value)
        {
            return _listImplementation.IndexOf(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Insert(int index, object value)
        {
            _listImplementation.Insert(index, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void Remove(object value)
        {
            _listImplementation.Remove(value);
        }

        /// <inheritdoc />
        public int IndexOf(SyntaxFieldInfo item)
        {
            return _generic.IndexOf(item);
        }

        /// <inheritdoc />
        public void Insert(int index, SyntaxFieldInfo item)
        {
            _generic.Insert(index, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            _listImplementation.RemoveAt(index);
        }

        SyntaxFieldInfo IList<SyntaxFieldInfo>.this[int index]
        {
            get { return _generic[index]; }
            set { _generic[index] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public object this[int index]
        {
            get { return _listImplementation[index]; }
            set { _listImplementation[index] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly
        {
            get { return _listImplementation.IsReadOnly; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsFixedSize
        {
            get { return _listImplementation.IsFixedSize; }
        }

        #endregion
    }


    /// <summary>
    /// </summary>
    
    public sealed class SyntaxFieldInfo
    {
        private int _id;
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
        public SyntaxFieldInfo(string name, string typeName, [NotNull] params string[] kinds)
        {
            _name = name;
            _typeName = typeName;

            foreach (var k in kinds.Select(kind => (SyntaxKind) Enum.Parse(typeof(SyntaxKind), kind))) _kinds.Add(k);
        }

        private string _name;
        private string _typeName;
        private readonly SyntaxKindCollection _kinds = new SyntaxKindCollection();
        private bool _optional;
        private Type _type;
        private string _elementTypeMetadataName;
        private bool _isCollection;
        private AppTypeInfo _appTypeInfo;

        /// <summary>
        /// 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public string TypeName
        {
            get { return _typeName; }
            set { _typeName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

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
        [TypeConverter(typeof(SyntaxFieldTypeTypeConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [NotMapped]
        public Type Type
        {
            get { return _type; }
            set
            {
                _type = value;
                _clrTypeName = _type?.AssemblyQualifiedName;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public List<AppTypeInfo> Types { get; } = new List<AppTypeInfo>();

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        [NotNull]
        [NotMapped]
        public IEnumerable<Type> ClrTypes
        {
            get { return Types.Select(typ => typ.Type); }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Override
        {
            get { return _override; }
            set { _override = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public bool Optional
        {
            get { return _optional; }
            set { _optional = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public string ClrTypeName
        {
            get { return _clrTypeName; }
            set { _clrTypeName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ElementTypeMetadataName
        {
            get { return _elementTypeMetadataName; }
            set { _elementTypeMetadataName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsCollection
        {
            get { return _isCollection; }
            set { _isCollection = value; }
        }

        /// <summary>
        /// Primary key
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [JsonIgnore]
        [Browsable(false)]
        public AppTypeInfo AppTypeInfo
        {
            get { return _appTypeInfo; }
            set { _appTypeInfo = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public ITypesViewModel Model { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return
                $"TypeName: {TypeName}, Name: {Name}, Kinds: {Kinds}, Type: {Type}, Types: {Types}, ClrTypes: {ClrTypes}, Override: {Override}, Optional: {Optional}";
        }
    }

    /// <inheritdoc />
    public sealed class SyntaxFieldTypeTypeConverter : TypeConverter
    {
        #region Overrides of TypeConverter

        /// <inheritdoc />
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string)) return true;
            return base.CanConvertTo(context, destinationType);
        }

        /// <inheritdoc />
        public override object ConvertTo(
            ITypeDescriptorContext context
            , CultureInfo culture
            , object value
            , Type destinationType
        )
        {
            // ReSharper disable once UnusedVariable
            if (value is Type t) return "boo";
            return base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class SyntaxKindCollection : IList, ICollection, IEnumerable
    {
        private readonly IList _list = new List<SyntaxKind>();

        /// <inheritdoc />
        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        /// <inheritdoc />
        public void CopyTo(Array array, int index)
        {
            _list.CopyTo(array, index);
        }

        /// <inheritdoc />
        public int Count
        {
            get { return _list.Count; }
        }

        /// <inheritdoc />
        public object SyncRoot
        {
            get { return _list.SyncRoot; }
        }

        /// <inheritdoc />
        public bool IsSynchronized
        {
            get { return _list.IsSynchronized; }
        }

        /// <inheritdoc />
        public int Add(object value)
        {
            return _list.Add(value);
        }

        /// <inheritdoc />
        public bool Contains(object value)
        {
            return _list.Contains(value);
        }

        /// <inheritdoc />
        public void Clear()
        {
            _list.Clear();
        }

        /// <inheritdoc />
        public int IndexOf(object value)
        {
            return _list.IndexOf(value);
        }

        /// <inheritdoc />
        public void Insert(int index, object value)
        {
            _list.Insert(index, value);
        }

        /// <inheritdoc />
        public void Remove(object value)
        {
            _list.Remove(value);
        }

        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        /// <inheritdoc />
        public object this[int index]
        {
            get { return _list[index]; }
            set { _list[index] = value; }
        }


        /// <inheritdoc />
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <inheritdoc />
        public bool IsFixedSize
        {
            get { return _list.IsFixedSize; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class AppTypeInfoCollection : IList<AppTypeInfo>
        , ICollection<AppTypeInfo>
        , IEnumerable<AppTypeInfo>, ICollection
    {
        private readonly IList<AppTypeInfo> _listImplementation = new List<AppTypeInfo>();
#pragma warning disable 649
        private object _syncRoot;
#pragma warning restore 649
#pragma warning disable 649
        private bool _isSynchronized;
#pragma warning restore 649

        #region Implementation of IEnumerable

        /// <inheritdoc />
        public IEnumerator<AppTypeInfo> GetEnumerator()
        {
            return _listImplementation.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _listImplementation).GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<AppTypeInfo>

        /// <inheritdoc />
        public void Add(AppTypeInfo item)
        {
            _listImplementation.Add(item);
        }

        /// <inheritdoc />
        public void Clear()
        {
            _listImplementation.Clear();
        }

        /// <inheritdoc />
        public bool Contains(AppTypeInfo item)
        {
            return _listImplementation.Contains(item);
        }

        /// <inheritdoc />
        public void CopyTo(AppTypeInfo[] array, int arrayIndex)
        {
            _listImplementation.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public bool Remove(AppTypeInfo item)
        {
            return _listImplementation.Remove(item);
        }

        /// <inheritdoc />
        public void CopyTo(Array array, int index)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get { return _listImplementation.Count; }
        }

        /// <inheritdoc />
        public object SyncRoot
        {
            get { return _syncRoot; }
        }

        /// <inheritdoc />
        public bool IsSynchronized
        {
            get { return _isSynchronized; }
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get { return _listImplementation.IsReadOnly; }
        }

        #endregion

        #region Implementation of IList<AppTypeInfo>

        /// <inheritdoc />
        public int IndexOf(AppTypeInfo item)
        {
            return _listImplementation.IndexOf(item);
        }

        /// <inheritdoc />
        public void Insert(int index, AppTypeInfo item)
        {
            _listImplementation.Insert(index, item);
        }

        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            _listImplementation.RemoveAt(index);
        }

        /// <inheritdoc />
        public AppTypeInfo this[int index]
        {
            get { return _listImplementation[index]; }
            set { _listImplementation[index] = value; }
        }

        #endregion
    }

    /// <summary>
    /// Wrapper class around <see cref="MethodInfo"/>. Supplies extra information if necessary and other potentially =
    /// useful information and facilities. Method parameters are individually wrapped in <see cref="AppParameterInfo"/>
    /// </summary>
    public sealed class AppMethodInfo
    {
        private System.Reflection.MethodInfo _methodInfo;
        private int _id;

        /// <summary>
        /// </summary>
        [JsonIgnore]
        [NotMapped]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public System.Reflection.MethodInfo MethodInfo
        {
            get { return _methodInfo; }
            set
            {
                _methodInfo = value;
                MethodName = value.Name;
                foreach (var p in MethodInfo.GetParameters()
                    .Select(
                        (info, i) => new AppParameterInfo
                        {
                            Index = i,
                            ParameterType = info.ParameterType,
                            Name = info.Name,
                            IsOptional = info.IsOptional
                        }
                    ))
                    Parameters.Add(p);
                ReturnType = value.ReturnType;
                DeclaringType = value.DeclaringType;
            }
        }

        /// <summary>
        /// </summary>
        /// 
        [NotMapped]
        [CanBeNull]
        [UsedImplicitly]
        public Type ReflectedType { get; set; }

        /// <summary>
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        [NotMapped]
        [CanBeNull]
        public Type DeclaringType { get; set; }

        /// <summary>
        /// </summary>
        [CanBeNull]
        [UsedImplicitly]
        public string MethodName { get; set; }

        /// <summary>
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        [CanBeNull]
        public Type ReturnType { get; set; }

        /// <summary>
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotNull]
        public AppParameterCollection Parameters { get; } = new AppParameterCollection();

        /// <summary>
        /// </summary>
        [JsonIgnore]
        [NotMapped]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ICodeElementDocumentation XmlDoc { get; set; }

        /// <summary>
        /// Primary key
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AppParameterCollection : IList, ICollection, IEnumerable
    {
        private readonly IList _listImplementation = new List<AppParameterInfo>();

        /// <inheritdoc />
        public IEnumerator GetEnumerator()
        {
            return _listImplementation.GetEnumerator();
        }

        /// <inheritdoc />
        public void CopyTo(Array array, int index)
        {
            _listImplementation.CopyTo(array, index);
        }

        /// <inheritdoc />
        public int Count
        {
            get { return _listImplementation.Count; }
        }

        /// <inheritdoc />
        public object SyncRoot
        {
            get { return _listImplementation.SyncRoot; }
        }

        /// <inheritdoc />
        public bool IsSynchronized
        {
            get { return _listImplementation.IsSynchronized; }
        }

        /// <inheritdoc />
        public int Add(object value)
        {
            return _listImplementation.Add(value);
        }

        /// <inheritdoc />
        public bool Contains(object value)
        {
            return _listImplementation.Contains(value);
        }

        /// <inheritdoc />
        public void Clear()
        {
            _listImplementation.Clear();
        }

        /// <inheritdoc />
        public int IndexOf(object value)
        {
            return _listImplementation.IndexOf(value);
        }

        /// <inheritdoc />
        public void Insert(int index, object value)
        {
            _listImplementation.Insert(index, value);
        }

        /// <inheritdoc />
        public void Remove(object value)
        {
            _listImplementation.Remove(value);
        }

        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            _listImplementation.RemoveAt(index);
        }

        /// <inheritdoc />
        public object this[int index]
        {
            get { return _listImplementation[index]; }
            set { _listImplementation[index] = value; }
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get { return _listImplementation.IsReadOnly; }
        }

        /// <inheritdoc />
        public bool IsFixedSize
        {
            get { return _listImplementation.IsFixedSize; }
        }
    }

    /// <summary>
    /// See <see cref="AppMethodInfo"/>.
    /// </summary>
    public sealed class AppParameterInfo
    {
        private int _id;
        private int _index;
        private bool _isOptional;
        private string _name;
        private Type _parameterType;

        /// <summary>
        /// Type of parameter.
        /// </summary>
        [NotMapped]
        public Type ParameterType
        {
            [UsedImplicitly] get { return _parameterType; }
            set { _parameterType = value; }
        }

        /// <summary>
        /// Is parameter optional?
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public bool IsOptional
        {
            get { return _isOptional; }
            set { _isOptional = value; }
        }

        /// <summary>
        /// Name of parameter
        /// </summary>
        public string Name
        {
            [UsedImplicitly] get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Zero-based index of parameter.
        /// </summary>
        public int Index
        {
            [UsedImplicitly] get { return _index; }
            set { _index = value; }
        }

        /// <summary>
        /// Public key
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class AppTypeInfoKey : IComparable<AppTypeInfoKey>
    {
        private string _unqualifiedTypeName;

        /// <summary>
        /// 
        /// </summary>
        public string StringValue
        {
            get { return _unqualifiedTypeName; }
            set { _unqualifiedTypeName = value; }
        }

        #region Relational members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(AppTypeInfoKey other)
        {
            if (ReferenceEquals(this, other)) return 0;

            if (ReferenceEquals(null, other)) return 1;

            return string.Compare(StringValue, other.StringValue, StringComparison.Ordinal);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unqualifiedTypeName"></param>
        public AppTypeInfoKey(string unqualifiedTypeName)
        {
            StringValue = unqualifiedTypeName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public AppTypeInfoKey([NotNull] Type type)
        {
            StringValue = type.Name;
        }

        public override string ToString()
        {
            return $"AppTypeInfoKey{{{StringValue}}}";
        }
        #region Equality members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals([NotNull] AppTypeInfoKey other)
        {
            return StringValue == other.StringValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is AppTypeInfoKey other && Equals(other);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return StringValue != null ? StringValue.GetHashCode() : 0;
        }

        #endregion
    }
}