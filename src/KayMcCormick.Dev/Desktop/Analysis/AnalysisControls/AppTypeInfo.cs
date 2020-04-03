using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.Linq ;
using System.Runtime.CompilerServices ;
using System.Text.Json.Serialization ;
using System.Text.RegularExpressions ;
using System.Windows.Markup ;
using AnalysisAppLib.Syntax ;
using AnalysisControls.ViewModel ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis.CSharp ;

namespace AnalysisControls
{
    /// <summary>
    ///     <para>Represents a Syntax Node type in the application.</para>
    ///     <para></para>
    /// </summary>
    public sealed class AppTypeInfo : INotifyPropertyChanged

    {
        private string _elementName ;
        private SyntaxFieldCollection  _fields = new SyntaxFieldCollection ();
        private AppTypeInfoCollection _subTypeInfos ;
        private          uint ?                               _colorValue ;

        private SyntaxComponentCollection _components =
            new SyntaxComponentCollection ( ) ;

        private ObservableCollection < AppMethodInfo > _factoryMethods =
            new ObservableCollection < AppMethodInfo > ( ) ;

        private int         _hierarchyLevel ;
        private AppTypeInfo _parentInfo ;
        private string      _title ;
        private Type        _type ;


        /// <summary>
        /// </summary>
        /// <param name="subTypeInfos"></param>
        public AppTypeInfo ( AppTypeInfoCollection subTypeInfos = null )
        {
            if ( subTypeInfos == null )
            {
                subTypeInfos = new AppTypeInfoCollection ( ) ;
            }

            _components.CollectionChanged += ( sender , args )
                => OnPropertyChanged ( nameof ( Components ) ) ;
            _factoryMethods.CollectionChanged += ( sender , args )
                => OnPropertyChanged ( nameof ( FactoryMethods ) ) ;
            _subTypeInfos = subTypeInfos ;
        }

        /// <summary>
        /// </summary>
        public AppTypeInfo ( ) : this ( null ) { }

        /// <summary>
        /// </summary>
        public Type Type
        {
            get { return _type ; }
            set
            {
                _type = value ;
                OnPropertyChanged ( ) ;
                var title = _type.Name.Replace ( "Syntax" , "" ) ;
                Title = Regex.Replace ( title , "([a-z])([A-Z])" , @"$1 $2" ) ;
            }
        }

        /// <summary>
        /// </summary>
        public string Title
        {
            get { return _title ; }
            set
            {
                _title = value ;
                OnPropertyChanged ( ) ;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AppTypeInfoCollection SubTypeInfos
        {
            get { return _subTypeInfos ; }
            set { _subTypeInfos = value ; }
        }

        /// <summary>
        /// </summary>
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
        public ObservableCollection < AppMethodInfo > FactoryMethods
        {
            get { return _factoryMethods ; }
            set
            {
                _factoryMethods = value ;
                OnPropertyChanged ( ) ;
            }
        }

        /// <summary>
        /// </summary>
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
        [ JsonIgnore ]
        public SyntaxComponentCollection Components
        {
            get { return _components ; }
            set
            {
                _components = value ;
                OnPropertyChanged ( ) ;
            }
        }

        /// <summary>
        /// </summary>
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
        [ NotNull ] public IEnumerable < ComponentInfo > AllComponents
        {
            get
            {
                var type = ParentInfo ;
                var allComponentInfos =
                    Components?.ToList ( ) ?? Enumerable.Empty < ComponentInfo > ( ) ;
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
                                                                                                   , IsList
                                                                                                         = info
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
        /// </summary>
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
        [ JsonIgnore ]
        public AppTypeInfo ParentInfo
        {
            get { return _parentInfo ; }
            set
            {
                _parentInfo = value ;
                OnPropertyChanged ( ) ;
            }
        }

        /// <summary>
        /// </summary>
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
        public int HierarchyLevel
        {
            get { return _hierarchyLevel ; }
            set { _hierarchyLevel = value ; }
        }

        /// <summary>
        /// </summary>
        public uint ? ColorValue { get { return _colorValue ; } set { _colorValue = value ; } }

        /// <summary>
        /// </summary>
        public TypeDocumentation DocInfo { get ; set ; }

        public string ElementName { get { return _elementName ; } set { _elementName = value ; } }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public SyntaxFieldCollection Fields
        {
            get { return _fields ; }
        }

        /// <summary>
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
    public class SyntaxFieldCollection : IList , ICollection, IEnumerable
    {
        private IList _listImplementation = new List < SyntaxFieldInfo > ( ) ;
        #region Implementation of IEnumerable
        public IEnumerator GetEnumerator ( ) { return _listImplementation.GetEnumerator ( ) ; }
        #endregion
        #region Implementation of ICollection
        public void CopyTo ( Array array , int index ) { _listImplementation.CopyTo ( array , index ) ; }

        public int Count
        {
            get { return _listImplementation.Count ; }
        }

        public object SyncRoot
        {
            get { return _listImplementation.SyncRoot ; }
        }

        public bool IsSynchronized
        {
            get { return _listImplementation.IsSynchronized ; }
        }
        #endregion
        #region Implementation of IList
        public int Add ( object value ) { return _listImplementation.Add ( value ) ; }

        public bool Contains ( object value ) { return _listImplementation.Contains ( value ) ; }

        public void Clear ( ) { _listImplementation.Clear ( ) ; }

        public int IndexOf ( object value ) { return _listImplementation.IndexOf ( value ) ; }

        public void Insert ( int index , object value ) { _listImplementation.Insert ( index , value ) ; }

        public void Remove ( object value ) { _listImplementation.Remove ( value ) ; }

        public void RemoveAt ( int index ) { _listImplementation.RemoveAt ( index ) ; }

        public object this [ int index ]
        {
            get { return _listImplementation[ index ] ; }
            set { _listImplementation[ index ] = value ; }
        }

        public bool IsReadOnly
        {
            get { return _listImplementation.IsReadOnly ; }
        }

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
        public SyntaxFieldInfo ( string name , string typeName, params string[] kinds )
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
        private SyntaxKindCollection _kinds = new SyntaxKindCollection ();

        [DesignerSerializationVisibility( DesignerSerializationVisibility.Content)]
        public string TypeName  { get { return _typeName ; } set { _typeName = value ; } }

        public string Name { get { return _name ; } set { _name = value ; } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public SyntaxKindCollection Kinds
        {
            get { return _kinds ; }
        }

        public Type Type { get ; set ; }

        public List<AppTypeInfo> Types { get ;  } = new List < AppTypeInfo > ();
    }

    public class SyntaxKindCollection : IList, ICollection, IEnumerable
    {
        private IList _listImplementation = new List < SyntaxKind > ( ) ;
        #region Implementation of IEnumerable
        public IEnumerator GetEnumerator ( ) { return _listImplementation.GetEnumerator ( ) ; }
        #endregion
        #region Implementation of ICollection
        public void CopyTo ( Array array , int index ) { _listImplementation.CopyTo ( array , index ) ; }

        public int Count
        {
            get { return _listImplementation.Count ; }
        }

        public object SyncRoot
        {
            get { return _listImplementation.SyncRoot ; }
        }

        public bool IsSynchronized
        {
            get { return _listImplementation.IsSynchronized ; }
        }
        #endregion
        #region Implementation of IList
        public int Add ( object value ) { return _listImplementation.Add ( value ) ; }

        public bool Contains ( object value ) { return _listImplementation.Contains ( value ) ; }

        public void Clear ( ) { _listImplementation.Clear ( ) ; }

        public int IndexOf ( object value ) { return _listImplementation.IndexOf ( value ) ; }

        public void Insert ( int index , object value ) { _listImplementation.Insert ( index , value ) ; }

        public void Remove ( object value ) { _listImplementation.Remove ( value ) ; }

        public void RemoveAt ( int index ) { _listImplementation.RemoveAt ( index ) ; }

        public object this [ int index ]
        {
            get { return _listImplementation[ index ] ; }
            set { _listImplementation[ index ] = value ; }
        }

        public bool IsReadOnly
        {
            get { return _listImplementation.IsReadOnly ; }
        }

        public bool IsFixedSize
        {
            get { return _listImplementation.IsFixedSize ; }
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class AppTypeInfoCollection : IList, ICollection, IEnumerable
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

        private IList _list ;
        #region Implementation of IEnumerable
        public IEnumerator GetEnumerator ( ) { return _list.GetEnumerator ( ) ; }
        #endregion
        #region Implementation of ICollection
        public void CopyTo ( Array array , int index ) { _list.CopyTo ( array , index ) ; }

        public int Count
        {
            get { return _list.Count ; }
        }

        public object SyncRoot
        {
            get { return _list.SyncRoot ; }
        }

        public bool IsSynchronized
        {
            get { return _list.IsSynchronized ; }
        }
        #endregion
        #region Implementation of IList
        public int Add ( object value ) { return _list.Add ( value ) ; }

        public bool Contains ( object value ) { return _list.Contains ( value ) ; }

        public void Clear ( ) { _list.Clear ( ) ; }

        public int IndexOf ( object value ) { return _list.IndexOf ( value ) ; }

        public void Insert ( int index , object value ) { _list.Insert ( index , value ) ; }

        public void Remove ( object value ) { _list.Remove ( value ) ; }

        public void RemoveAt ( int index ) { _list.RemoveAt ( index ) ; }

        public object this [ int index ]
        {
            get { return _list[ index ] ; }
            set { _list[ index ] = value ; }
        }

        public bool IsReadOnly
        {
            get { return _list.IsReadOnly ; }
        }

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
}