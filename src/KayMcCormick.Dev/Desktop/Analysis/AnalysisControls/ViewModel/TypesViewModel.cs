using System ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.IO ;
using System.Linq ;
using System.Runtime.CompilerServices ;
using System.Runtime.Serialization ;
using System.Text.Json.Serialization ;
using System.Xml ;
using AnalysisAppLib ;
using AnalysisAppLib.XmlDoc ;
using AnalysisAppLib.XmlDoc.Properties ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Serialization ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using NLog ;

namespace AnalysisControls.ViewModel
{
    /// <summary>
    /// </summary>
    [ NoJsonConverter ]
    public class TypesViewModel : ITypesViewModel
      , INotifyPropertyChanged
      , ISupportInitializeNotification
    {
        private DateTime _initializationDateTime ;
        private bool     _showBordersIsChecked ;

        private uint[] _hierarchyColors =
        {
            0xff9cbf60 , 0xff786482 , 0xffb89428 , 0xff9ec28c , 0xff3c6e7d , 0xff533ca3
        } ;

        private AppTypeInfo   root ;
        private List < Type > _nodeTypes ;

        private TypeMapDictionary map = new TypeMapDictionary ( ) ;

        private readonly Dictionary < Type , AppTypeInfo > otherTyps =
            new Dictionary < Type , AppTypeInfo > ( ) ;
#if true
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
#else
#endif

        private readonly Dictionary < Type , TypeDocInfo >
            _docs = new Dictionary < Type , TypeDocInfo > ( ) ;

        private DocumentCollection _documentCollection = new DocumentCollection ( ) ;

        /// <summary>
        /// </summary>
        // ReSharper disable once EmptyConstructor
        public TypesViewModel ( ) { }


        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ NotNull ]
        public static XmlDocument LoadDoc ( )
        {
            var xml = Resources.doc ;
            var docuDoc = new XmlDocument ( ) ;
            docuDoc.LoadXml ( xml ) ;
            return docuDoc ;
        }

        /// <summary>
        /// </summary>
        [ DesignerSerializationVisibility ( DesignerSerializationVisibility.Hidden ) ]
        [ JsonIgnore ]
        public AppTypeInfo Root
        {
            get { return root ; }
            set
            {
                root = value ;
                OnPropertyChanged ( ) ;
            }
        }

        /// <summary>
        /// </summary>
        public bool ShowBordersIsChecked
        {
            get { return _showBordersIsChecked ; }
            set
            {
                _showBordersIsChecked = value ;
                OnPropertyChanged ( ) ;
            }
        }

        /// <summary>
        /// </summary>
        [ DesignerSerializationVisibility ( DesignerSerializationVisibility.Hidden ) ]
        public uint[] HierarchyColors
        {
            get { return _hierarchyColors ; }
            set { _hierarchyColors = value ; }
        }

        /// <summary>
        /// </summary>
        public DocumentCollection DocumentCollection
        {
            get { return _documentCollection ; }
            set { _documentCollection = value ; }
        }

        /// <summary>
        /// </summary>
        public TypeMapDictionary Map { get { return map ; } set { map = value ; } }

        /// <summary>
        /// </summary>
        public AppTypeInfoCollection StructureRoot { get ; set ; } = new AppTypeInfoCollection ( ) ;

        public AppTypeInfo GetAppTypeInfo ( object identifier ) { return null ; }

        /// <summary>
        /// </summary>
        /// <param name="p  tTypeInfo"></param>
        /// <param name="rootR"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        [ NotNull ]
        public AppTypeInfo CollectTypeInfos (
            AppTypeInfo      parentTypeInfo
          , [ NotNull ] Type rootR
          , int              level = 0
        )
        {
            TypeDocumentation docNode = null ;

            if ( _docs.TryGetValue (
                                    rootR ?? throw new InvalidOperationException ( )
                                  , out var info
                                   ) )
            {
                if ( info.TypeDocumentation != null )
                {
                    docNode = info.TypeDocumentation ;
                }
            }

            if ( docNode != null )
            {
                CollectDoc ( docNode ) ;
            }

            var r = new AppTypeInfo
                    {
                        Type           = rootR
                      , DocInfo        = docNode
                      , ParentInfo     = parentTypeInfo
                      , HierarchyLevel = level
                      , ColorValue     = HierarchyColors[ level ]
                    } ;
            foreach ( var type1 in _nodeTypes.Where ( type => type.BaseType == rootR ) )
            {
                r.SubTypeInfos.Add ( CollectTypeInfos ( r , type1 , level + 1 ) ) ;
            }

            Map.dict[ new AppTypeInfoKey ( rootR ) ] = r ;
            return r ;
        }

        private void CollectDoc ( [ CanBeNull ] CodeElementDocumentation docNode )
        {
            if ( docNode == null )
            {
                return ;
            }

            DebugUtils.WriteLine ( $"{docNode}" ) ;
            DocumentCollection.Add ( docNode ) ;
        }

        #region Implementation of ISerializable
        /// <summary>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion

        /// <summary>
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        private void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }

        #region Implementation of ISupportInitialize
        /// <summary>
        /// </summary>
        public void BeginInit ( ) { }

        /// <summary>
        /// </summary>
        public void EndInit ( )
        {
            Logger.Info ( nameof ( EndInit ) ) ;
            var mapCount = Map.Count ;
            Logger.Info ( $"Map Count {mapCount}" ) ;
            if ( mapCount != 0 )
            {
                CreateSubtypeLinkages ( ) ;
            }
            else if ( Root                       == null
                      || Root.SubTypeInfos.Count == 0 )
            {
                LoadTypeInfo ( ) ;
            }
            else
            {
                PopulateMap ( Root ) ;
            }

            DetailFields ( ) ;

            //LoadSyntaxFactoryDocs ( _docs ) ;
//            StructureRoot = new AppTypeInfoCollection { Map[ typeof ( CompilationUnitSyntax ) ] } ;
            IsInitialized          = true ;
            InitializationDateTime = DateTime.Now ;
        }

        /// <summary>
        /// </summary>
        public void DetailFields ( )
        {
            foreach ( var rField in Map.dict.SelectMany (
                                                         keyValuePair
                                                             => keyValuePair
                                                               .Value.Fields
                                                               .Cast < SyntaxFieldInfo > ( )
                                                        ) )
            {
                if ( rField.Type == null )
                {
                    DebugUtils.WriteLine ( $"type is null for {rField.TypeName}" ) ;
                }

                if ( rField.Type != null
                     && rField.Type.IsGenericType
                     && ( rField.Type.GetGenericTypeDefinition ( ) == typeof ( SyntaxList <> )
                          || rField.Type.GetGenericTypeDefinition ( )
                          == typeof ( SeparatedSyntaxList <> ) ) )
                {
                    var tz = rField.Type.GetGenericArguments ( )[ 0 ] ;
                    if ( ! Map.Contains ( tz ) )
                    {
                        continue ;
                    }

                    var ati = Map.dict[ new AppTypeInfoKey ( tz ) ] ;
                    var types = new List < AppTypeInfo > ( ) ;
                    Collect ( ati , types ) ;
                    foreach ( var appTypeInfo in types )
                    {
                        rField.Types.Add ( appTypeInfo ) ;
                    }
                }
                else
                {
                    if ( rField.Type != null
                         && Map.Contains ( rField.Type ) )
                    {
                        rField.Types.Add ( Map.dict[ new AppTypeInfoKey ( rField.Type ) ] ) ;
                    }
                }
            }
        }

        private void CreateSubtypeLinkages ( )
        {
            DebugUtils.WriteLine ( $"Performing {nameof ( CreateSubtypeLinkages )}" ) ;
            var rootR = typeof ( CSharpSyntaxNode ) ;
            _nodeTypes = rootR.Assembly.GetExportedTypes ( )
                              .Where ( t => typeof ( CSharpSyntaxNode ).IsAssignableFrom ( t ) )
                              .ToList ( ) ;
            Root = CollectTypeInfos2 ( null , rootR ) ;
        }

        [ NotNull ]
        private AppTypeInfo CollectTypeInfos2 (
            AppTypeInfo      parentTypeInfo
          , [ NotNull ] Type rootR
          , int              level = 0
        )
        {
            DebugUtils.WriteLine ( $"{rootR}" ) ;
            // if (_docs.TryGetValue(
            //                       rootR ?? throw new InvalidOperationException()
            //                     , out var info
            //                      ))
            // {
            //     docNode = info.TypeDocumentation;
            // }

//            CollectDoc(docNode);
            if ( ! Map.dict.TryGetValue ( new AppTypeInfoKey ( 
                rootR ) , out var curTypeInfo ) )
            {
                throw new InvalidOperationException ( ) ;
            }

            DebugUtils.WriteLine ( $"{curTypeInfo}" ) ;
            var r = Map.dict[ new AppTypeInfoKey ( rootR ) ] ;
            r.ParentInfo     = parentTypeInfo ;
            r.HierarchyLevel = level ;
            r.ColorValue     = HierarchyColors[ level ] ;
            foreach ( var type1 in _nodeTypes.Where ( type => type.BaseType == rootR ) )
            {
                var theTypeInfo = CollectTypeInfos2 ( r , type1 , level + 1 ) ;
                r.SubTypeInfos.Add ( theTypeInfo ) ;
            }

            PopulateFieldTypes ( r ) ;

            return r ;
        }

        /// <summary>
        /// </summary>
        /// <param name="r"></param>
        public void PopulateFieldTypes ( [ NotNull ] AppTypeInfo r )
        {
            // find way to eliminate this
            foreach ( SyntaxFieldInfo rField in r.Fields )
            {
                if ( rField.TypeName == "SyntaxList<SyntaxToken>" )
                {
                    rField.TypeName = "SyntaxTokenList" ;
                }

                TypeSyntax typs ;
                try
                {
                    typs = SyntaxFactory.ParseTypeName ( rField.TypeName ) ;
                }
                catch ( FileNotFoundException )
                {
                    continue ;
                }

                if ( typs is GenericNameSyntax gns )
                {
                    var id = gns.Identifier.ValueText ;
                    var t0 = typeof ( SyntaxNode ).Assembly.GetType (
                                                                     "Microsoft.CodeAnalysis."
                                                                     + id
                                                                     + "`1"
                                                                    ) ;
                    if ( t0 == null )
                    {
                        DebugUtils.WriteLine ( "fail" + id ) ;
                    }
                    else
                    {
                        var s = ( SimpleNameSyntax ) gns.TypeArgumentList.Arguments[ 0 ] ;
                        var t1 = typeof ( CSharpSyntaxNode ).Assembly.GetType (
                                                                               "Microsoft.CodeAnalysis.CSharp.Syntax."
                                                                               + s.Identifier
                                                                                  .ValueText
                                                                              ) ;
                        if ( t1 == null )
                        {
                            t1 = typeof ( SyntaxNode ).Assembly.GetType (
                                                                         "Microsoft.CodeAnalysis."
                                                                         + s.Identifier.ValueText
                                                                        ) ;
                        }

                        var t2 = t0.MakeGenericType ( t1 ) ;
                        rField.Type = t2 ;
                    }
                }
                else
                {
                    var t = typeof ( CSharpSyntaxNode ).Assembly.GetType (
                                                                          "Microsoft.CodeAnalysis.CSharp.Syntax."
                                                                          + rField.TypeName
                                                                         ) ;
                    if ( t == null )
                    {
                        t = typeof ( SyntaxNode ).Assembly.GetType (
                                                                    "Microsoft.CodeAnalysis."
                                                                    + rField.TypeName
                                                                   ) ;
                        if ( t == null )
                        {
                            //DebugUtils.WriteLine ( rField.TypeName ) ;
                        }
                    }

                    rField.Type = t ;
                }
            }
        }

        private static void Collect ( [ NotNull ] AppTypeInfo ati , List < AppTypeInfo > types )
        {
            if ( ! ati.Type.IsAbstract )
            {
                types.Add ( ati ) ;
            }

            foreach ( var atiSubTypeInfo in ati.SubTypeInfos )
            {
                Collect ( atiSubTypeInfo , types ) ;
            }
        }

        private void PopulateMap ( [ NotNull ] AppTypeInfo appTypeInfo )
        {
            Map[ appTypeInfo.Type ] = appTypeInfo ;
            foreach ( var subTypeInfo in appTypeInfo.SubTypeInfos )
            {
                PopulateMap ( subTypeInfo ) ;
            }
        }

        private void LoadTypeInfo ( )
        {
            var rootR = typeof ( CSharpSyntaxNode ) ;
            _nodeTypes = rootR.Assembly.GetExportedTypes ( )
                              .Where ( t => typeof ( CSharpSyntaxNode ).IsAssignableFrom ( t ) )
                              .ToList ( ) ;
            Root = CollectTypeInfos ( null , rootR ) ;
        }
        #endregion

        #region Implementation of ISupportInitializeNotification
        /// <inheritdoc />
        public bool IsInitialized { get ; set ; }

        /// <summary>
        ///     An approximate time as to when the view model was initialized and/or
        ///     populated with extended information.
        /// </summary>
        public DateTime InitializationDateTime
        {
            [ UsedImplicitly ] get { return _initializationDateTime ; }
            set { _initializationDateTime = value ; }
        }

        /// <inheritdoc />
        public event EventHandler Initialized ;
        #endregion
    }

    public class ComponentTypesViewModel : TypesViewModel
    {
        public ComponentTypesViewModel ( )
        {


            // System.Windows.Application.LoadComponent ( this , new Uri (
                                                                       // @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\v3\NewRoot\src\KayMcCormick.Dev\Desktop\Analysis\AnalysisControls\TypesViewModel.xaml"
                                                                      // ) );

        }
    }
}