#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// AnalysisControls
// TypesViewModel.cs
// 
// 2020-03-11-7:05 PM
// 
// ---
#endregion
using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;
using System.Reflection ;
using System.Runtime.Serialization ;
using AnalysisControls.Syntax ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using NLog ;

namespace AnalysisControls.ViewModels
{
    internal class TypesViewModel : ITypesViewModel
    {
        private AppTypeInfo                       root ;
        private List < Type >                     _nodeTypes ;
        private Dictionary < Type , AppTypeInfo > map = new Dictionary < Type , AppTypeInfo > ( ) ;

        public TypesViewModel ( )
        {
            var rootR = typeof ( CSharpSyntaxNode ) ;
            _nodeTypes = rootR.Assembly.GetExportedTypes ( )
                              .Where ( t => typeof ( CSharpSyntaxNode ).IsAssignableFrom ( t ) )
                              .ToList ( ) ;
            Root = CollectTypeInfos ( rootR ) ;
            var methodInfos = typeof ( SyntaxFactory )
                             .GetMethods ( BindingFlags.Static | BindingFlags.Public )
                             .ToList ( ) ;

            foreach ( var methodInfo in methodInfos.Where (
                                                           info => typeof ( SyntaxNode )
                                                              .IsAssignableFrom ( info.ReturnType )
                                                          ) )
            {
                var info = map[ methodInfo.ReturnType ] ;
                info.FactoryMethods.Add ( methodInfo ) ;
                LogManager.GetCurrentClassLogger ( )
                          .Info ( "{methodName}" , methodInfo.ToString ( ) ) ;
            }

            foreach ( var pair in map.Where ( pair => pair.Key.IsAbstract == false ) )
            {
                foreach ( var propertyInfo in pair.Key.GetProperties (
                                                                      BindingFlags.DeclaredOnly
                                                                      | BindingFlags.Instance
                                                                      | BindingFlags.Public
                                                                     ) )
                {
                    var t = propertyInfo.PropertyType ;
                    if ( t == typeof ( SyntaxToken ) )
                    {
                        continue ;
                    }

                    var isList = false ;
                    AppTypeInfo typeInfo = null ;
                    if ( t.IsGenericType )
                    {
                        var targ = t.GenericTypeArguments[ 0 ] ;
                        if ( typeof ( SyntaxNode ).IsAssignableFrom ( targ )
                             && typeof ( IEnumerable ).IsAssignableFrom ( t ) )
                        {
                            LogManager.GetCurrentClassLogger ( )
                                      .Info (
                                             "{name} {prop} list of {}"
                                           , pair.Key.Name
                                           , propertyInfo.Name
                                           , targ.Name
                                            ) ;
                            isList   = true ;
                            typeInfo = map[ targ ] ;
                        }
                    }
                    else
                    {
                        map.TryGetValue ( t , out typeInfo ) ;
                    }

                    if ( typeInfo == null )
                    {
                        continue ;
                    }

                    pair.Value.Components.Add (
                                               new ComponentInfo ( )
                                               {
                                                   IsList       = isList
                                                 , TypeInfo     = typeInfo
                                                 , PropertyName = propertyInfo.Name
                                               }
                                              ) ;
                    LogManager.GetCurrentClassLogger ( ).Info ( t.ToString ( ) ) ;
                }
            }
        }

        public AppTypeInfo Root { get => root ; set => root = value ; }

        private AppTypeInfo CollectTypeInfos ( Type rootR )
        {
            var r = new AppTypeInfo ( ) { Type = rootR } ;
            foreach ( var type1 in _nodeTypes.Where ( type => type.BaseType == rootR ) )
            {
                r.SubTypeInfos.Add ( CollectTypeInfos ( type1 ) ) ;
            }

            map[ rootR ] = r ;
            return r ;
        }

        #region Implementation of ISerializable
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion
    }
}