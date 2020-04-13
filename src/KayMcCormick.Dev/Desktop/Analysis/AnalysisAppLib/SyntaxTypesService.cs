#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// AnalysisAppLib
// SyntaxTypesService.cs
// 
// 2020-04-13-3:57 AM
// 
// ---
#endregion
using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Linq ;
using System.Reflection ;
using AnalysisAppLib.XmlDoc ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using ComponentInfo = AnalysisAppLib.XmlDoc.ComponentInfo ;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public class SyntaxTypesService : ISyntaxTypesService , ISupportInitializeNotification
    {
        private static          string _pocoPrefix       = "Poco";
        private static readonly string _collectionSuffix = "Collection";

        private Type          _cSharpSyntaxNodeClrType ;
        private List < Type > _nodeTypes ;

        #region Implementation of ISyntaxTypesService
        /// <summary>
        /// Get the <see cref="AppTypeInfo"/> instance for a particular identifier.
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public AppTypeInfo GetAppTypeInfo ( [ NotNull ] object identifier )
        {
            AppTypeInfoKey key = null ;
            string unqualifiedTypeName = null ;
            if ( identifier is Type type )
            {
                unqualifiedTypeName = type.Name ;
            }
            else if ( identifier is string s1 )
            {
                unqualifiedTypeName = s1 ;
            }
            else if ( identifier is AppTypeInfoKey k1 )
            {
                key = k1 ;
            }
            else
            {
                throw new InvalidOperationException ( "Bad key" ) ;
            }

            if ( unqualifiedTypeName != null
                 && key              == null )
            {
                key = new AppTypeInfoKey ( unqualifiedTypeName ) ;
            }

            if ( Map.dict.TryGetValue ( key , out var typeInfo ) )
            {
                return typeInfo ;
            }

            throw new InvalidOperationException ( "No such type" ) ;
        }
        #endregion

        private void CreateSubtypeLinkages ( )
        {
            DebugUtils.WriteLine ( $"Performing {nameof ( CreateSubtypeLinkages )}" ) ;
            _cSharpSyntaxNodeClrType = typeof ( CSharpSyntaxNode ) ;
            _nodeTypes = _cSharpSyntaxNodeClrType.Assembly.GetExportedTypes ( )
                                                 .Where (
                                                         t => typeof ( CSharpSyntaxNode )
                                                            .IsAssignableFrom ( t )
                                                        )
                                                 .ToList ( ) ;
            AppTypeCSharpSyntaxNode = CollectTypeInfos2 ( null , _cSharpSyntaxNodeClrType ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        public AppTypeInfo AppTypeCSharpSyntaxNode
        {
            get { return _appTypeCSharpSyntaxNode ; }
            set { _appTypeCSharpSyntaxNode = value ; }
        }

        [ NotNull ]
        private AppTypeInfo CollectTypeInfos2 (
            AppTypeInfo      parentTypeInfo
          , [ NotNull ] Type rootR
          , int              level = 0
        )
        {
            DebugUtils.WriteLine ( $"{rootR}" ) ;

            if ( ! Map.dict.TryGetValue ( new AppTypeInfoKey(rootR) , out var curTypeInfo ) )
            {
                throw new InvalidOperationException ( ) ;
            }

            DebugUtils.WriteLine ( $"{curTypeInfo}" ) ;
            var r = Map.dict[ new AppTypeInfoKey ( rootR ) ] ;
            r.ParentInfo     = parentTypeInfo ;
            r.HierarchyLevel = level ;
            //r.ColorValue     = HierarchyColors[level];
            foreach ( var type1 in _nodeTypes.Where ( type => type.BaseType == rootR ) )
            {
                var theTypeInfo = CollectTypeInfos2 ( r , type1 , level + 1 ) ;
                r.SubTypeInfos.Add ( theTypeInfo ) ;
            }

            return r ;
        }

        /// <summary>
        /// -
        /// </summary>
        public  TypeMapDictionary Map { get { return _map ; } set { _map = value ; } }
        private TypeMapDictionary _map = new TypeMapDictionary ( ) ;
        private bool              _isInitialized ;
        private AppTypeInfo       _appTypeCSharpSyntaxNode ;
        #region Implementation of ISupportInitialize
        /// <summary>
        /// 
        /// </summary>
        public void BeginInit ( ) { }

        /// <summary>
        /// 
        /// </summary>
        public void EndInit ( ) { }
        #endregion

        #region Implementation of ISupportInitializeNotification
        /// <summary>
        /// 
        /// </summary>
        public bool IsInitialized { get { return _isInitialized ; } }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler Initialized ;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeSyntax"></param>
        /// <param name="collectionMap"></param>
        /// <param name="appTypeInfo"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TypeSyntax FieldPocoCollectionType (
            [ NotNull ] TypeSyntax                              typeSyntax
          , [ NotNull ] IReadOnlyDictionary < string , object > collectionMap
          , [ NotNull ] AppTypeInfo                             appTypeInfo
        )
        {
            if ( typeSyntax == null )
            {
                throw new ArgumentNullException ( nameof ( typeSyntax ) ) ;
            }

            if ( appTypeInfo == null )
            {
                throw new ArgumentNullException ( nameof ( appTypeInfo ) ) ;
            }

            var q =
                from SyntaxFieldInfo field in appTypeInfo.Fields
                let s = typeSyntax as SimpleNameSyntax
                where field.Name == s?.Identifier.Text
                select field.Types ;

            var key = q.FirstOrDefault ( )?.FirstOrDefault ( ) ;
            if ( key != null
                 && collectionMap.TryGetValue (
                                               key.KeyValue.ToString ( )
                                             , out var fieldTypeName
                                              ) )
            {
                return SyntaxFactory.ParseTypeName ( ( string ) fieldTypeName ) ;
            }

            return typeSyntax ;
        }

        internal void LoadFactoryMethodSignaturesAndDocumentation
            ( ISyntaxTypesService sts, IDocInterface docface )
        {

            var si = docface.GetTypeDocumentation(typeof ( SyntaxFactory ));
            var methodInfos = typeof ( SyntaxFactory )
                             .GetMethods ( BindingFlags.Static | BindingFlags.Public )
                             .ToList ( ) ;

            foreach ( var methodInfo in methodInfos.Where (
                                                           info => typeof ( SyntaxNode )
                                                              .IsAssignableFrom ( info.ReturnType )
                                                          ) )
            {
                AppTypeInfoKey key = new AppTypeInfoKey(methodInfo.ReturnType);
                var info = sts.GetAppTypeInfo ( key ) ;
                var appMethodInfo = new AppMethodInfo { MethodInfo = methodInfo } ;
                if ( si != null
                     && si.MethodDocumentation.TryGetValue ( methodInfo.Name , out var mdoc ) )
                {
                    var p = string.Join (
                                         ","
                                       , methodInfo
                                        .GetParameters ( )
                                        .Select (
                                                 parameterInfo
                                                     => parameterInfo.ParameterType.FullName
                                                )
                                        ) ;
                    //DebugUtils.WriteLine ( $"xx: {p}" ) ;
                    foreach ( var methodDocumentation in mdoc )
                    {
                        //  DebugUtils.WriteLine ( methodDocumentation.Parameters ) ;
                        if ( methodDocumentation.Parameters == p )
                        {
                            //    DebugUtils.WriteLine ( $"Docs for {methodInfo}" ) ;
                            appMethodInfo.XmlDoc = methodDocumentation ;
                            docface.CollectDoc ( methodDocumentation ) ;
                        }
                    }
                }

                info.FactoryMethods.Add ( appMethodInfo ) ;
                //Logger.Info ( "{methodName}" , methodInfo.ToString ( ) ) ;
            }

            foreach ( var pair in Map.dict.Where(v => v.Key != new AppTypeInfoKey(typeof(CSharpSyntaxNode)))
            )
            {
                //}.Where ( pair => pair.Key.IsAbstract == false ) )
                {
                    var type = GetTypeInfo ( pair.Value ) ;
                    foreach ( var propertyInfo in type.GetProperties (
                                                                          BindingFlags.DeclaredOnly
                                                                          | BindingFlags.Instance
                                                                          | BindingFlags.Public
                                                                         ) )
                    {
                        if ( propertyInfo.DeclaringType != type )
                        {
                            continue ;
                        }

                        var t = propertyInfo.PropertyType ;
                       
                        var isList = false ;
                        AppTypeInfo typeInfo = null ;
                        AppTypeInfo otherTypeInfo = null ;
                        if ( t.IsGenericType )
                        {
                            DebugUtils.WriteLine ( $"{t} is Generic" ) ;
                            var targ = t.GenericTypeArguments[ 0 ] ;
                            DebugUtils.WriteLine ( $"{targ}" ) ;
                            if ( typeof ( SyntaxNode ).IsAssignableFrom ( targ )
                                 && typeof ( IEnumerable ).IsAssignableFrom ( t ) )
                            {
                                isList   = true ;
                                typeInfo = ( AppTypeInfo ) Map[ targ ] ;
                            }
                        }
                        else
                        {
                            if ( ! Map.dict.TryGetValue ( new AppTypeInfoKey ( t ) , out typeInfo ) )
                            {
                                if ( ! otherTyps.TryGetValue ( t , out otherTypeInfo ) )
                                {
                                    otherTypeInfo = otherTyps[ t ] = new AppTypeInfo { Type = t } ;
                                }
                            }
                        }

                        if ( typeInfo         == null
                             && otherTypeInfo == null )
                        {
                            continue ;
                        }

                        PropertyDocumentation propDoc = null ;
                        var info = docface.GetTypeDocumentation ( type ) ;
                        if ( type != null)
                        {
                            if ( info.PropertyDocumentation.TryGetValue (
                                                                         propertyInfo.Name
                                                                       , out propDoc
                                                                        ) )
                            {
                            }
                        }

                        if ( propDoc != null )
                        {
                            docface.CollectDoc ( propDoc ) ;
                            // pair.Value.Components.Add (
                                                       // new ComponentInfo
                                                       // {
                                                           // XmlDoc         = propDoc
                                                         // , IsSelfOwned    = true
                                                         // , OwningTypeInfo = pair.Value
                                                         // , IsList         = isList
                                                         // , TypeInfo =
                                                               // typeInfo ?? otherTypeInfo
                                                         // , PropertyName = propertyInfo.Name
                                                       // }
                                                      // ) ;
                        }

                        //Logger.Info ( t.ToString ( ) ) ;
                    }
                }
            }
        }

        private Type GetTypeInfo ( [ NotNull ] AppTypeInfo pairValue )
        {
            return pairValue.Type ;
        }

        /// <summary>
        /// 
        /// </summary>
        public DocumentCollection DocumentCollection { get; set; } = new DocumentCollection();
        private readonly Dictionary<Type, AppTypeInfo> otherTyps =
            new Dictionary<Type, AppTypeInfo>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model1"></param>
        /// <param name="DebugOut"></param>
        /// <returns></returns>
        [NotNull]
        public IReadOnlyDictionary<string, object> CollectionMap(
        )
        {
            DebugUtils.WriteLine("Populating collectionMap");
            var collectionMap = new Dictionary<string, object>();
            foreach (var kvp in Map.dict)
            {
                var mapKey = kvp.Key;
                var t = (AppTypeInfo)Map[mapKey];
                var colType = $"{_pocoPrefix}{t.Type.Name}{_collectionSuffix}";
                collectionMap[(string)t.KeyValue] = colType;
            }

            return collectionMap;
        }
    }
}