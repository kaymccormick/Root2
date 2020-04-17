﻿#region header
// Kay McCormick (mccor)
// 
// PsProject
// WpfTestApp
// ProxyUtils.cs
// 
// 2020-02-01-6:58 AM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.IO ;
using System.Linq ;
using System.Reflection ;
using System.Threading ;
using System.Windows.Markup ;
using System.Xaml ;
using System.Xaml.Schema ;
using System.Xml ;
using Castle.DynamicProxy ;
using JetBrains.Annotations ;
using NLog ;
using XamlReader = System.Windows.Markup.XamlReader ;

/* imported form PsProject */
namespace KayMcCormick.Lib.Wpf
{
    /// <inheritdoc />
    public abstract class BaseInterceptor : IInterceptor
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxyGenerator"></param>
        protected BaseInterceptor ( ProxyGenerator proxyGenerator )
        {
            ProxyGenerator = proxyGenerator ;
        }

        /// <summary>Gets the proxy generator.</summary>
        /// <value>The proxy generator.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for ProxyGenerator
        protected ProxyGenerator ProxyGenerator { get ; }

        /// <summary>The write</summary>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for Write
        protected Action < string > Write { get ; } = null ;

        /// <summary>The write line</summary>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for WriteLine
        protected Action < string > WriteLine { get ; set ; }

        /// <summary>Intercepts the specified invocation.</summary>
        /// <param name="invocation">The invocation.</param>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for Intercept
        public abstract void Intercept ( IInvocation invocation ) ;

        /// <summary>Dumps the invocation.</summary>
        /// <param name="invocation">The invocation.</param>
        /// <param name="callDepth">The call depth.</param>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for DumpInvocation
        protected void DumpInvocation ( [ NotNull ] IInvocation invocation , int callDepth )
        {
            if ( invocation == null )
            {
                throw new ArgumentNullException ( nameof ( invocation ) ) ;
            }

            var c = Depth ( callDepth ) ;

            var f = FormatInvocation ( invocation , c ) ;
            if ( Write == null )
            {
                WriteLine ( f ) ;
            }
            else
            {
                Write ( f ) ;
            }
        }


        /// <summary>Formats the invocation.</summary>
        /// <param name="invocation">The invocation.</param>
        /// <param name="c">The c.</param>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for FormatInvocation
        [ NotNull ]
        protected string FormatInvocation ( [ NotNull ] IInvocation invocation , string c )
        {
            var args = invocation.Method.IsSpecialName && invocation.Arguments.Length == 0
                           ? ""
                           :
                           invocation.Arguments.Length == 0
                               ?
                               "( )"
                               : "("
                                 + string.Join (
                                                ", "
                                              , invocation
                                               .Arguments.Select (
                                                                  ( o , i ) => FormatArgument (
                                                                                               invocation
                                                                                             , o
                                                                                             , i
                                                                                              )
                                                                 )
                                               .ToArray ( )
                                               )
                                         .PadRight ( 40 )
                                 + " )" ;
            return c + "\t" + MethodSpec ( invocation ) + args ;
        }

        [ NotNull ]
        private string FormatArgument ( [ NotNull ] IInvocation invocation , object arg1 , int arg2 )
        {
            var q = FormatTypeAndValue ( arg1 ) ;
            var name = invocation.Method.GetParameters ( )[ arg2 ].Name ;
            return $"{name}: {q}" ;
        }

        [ NotNull ]
        private static string FormatTypeAndValue ( [ CanBeNull ] object o )
        {
            if ( o == null )
            {
                return "null" ;
            }

            var type = FormatTyp ( o.GetType ( ) ) + " " ;

            if ( o is Type )
            {
                type = "" ;
            }

            var formatValue = FormatValue ( o , out var wantType ) ;
            return wantType ? $"{type}{formatValue}" : formatValue ;
        }

        [ NotNull ]
        private static string Depth ( int callDepth )
        {
            const char c = '⟳' ;
            return $"{new string ( c , callDepth ),- 8}" ;
            //return char.ConvertFromUtf32 ( 0x2460 + callDepth - 1 ) ;
        }

        /// <summary>Dumps the return value.</summary>
        /// <param name="invocation">The invocation.</param>
        /// <param name="callDepth">The call depth.</param>
        /// <param name="continuation">if set to <see language="true" /> [continuation].</param>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for DumpReturnValue
        protected void DumpReturnValue (
            [ NotNull ] IInvocation invocation
          , int                     callDepth
          , bool                    continuation
        )
        {
            if ( invocation == null )
            {
                throw new ArgumentNullException ( nameof ( invocation ) ) ;
            }

            var c = Depth ( callDepth ) ;
            if ( invocation.ReturnValue == null )
            {
                return ;
            }

            var f = ( continuation
                          ? ""
                          : c
                            + "\t"
                            + MethodSpec ( invocation )
                            + "\t"
                            + //invocation.TargetType + "."+ invocation.Method.Name +
                            "\t\t" )
                    + " ➦ "
                    + FormatReturnValue ( invocation , invocation.ReturnValue ) ;
            WriteLine ( f ) ;
        }


        [ NotNull ]
        // ReSharper disable once UnusedParameter.Local
        private string FormatReturnValue ( IInvocation invocation , [ NotNull ] object r )
        {
            return FormatValue ( r , out _ ) ;
        }

        [ NotNull ]
        private static string FormatValue ( [ NotNull ] object r , out bool b1 )
        {
            b1 = false ;
            if ( r is bool b )
            {
                var val = b ? "⊨" : "⊭" ;
                return val ;
            }

            switch ( r )
            {
                case Type t :
                {
                    var rr = t.UnderlyingSystemType == t ? "" : "𝓡" ;
                    return $"{rr}𝒯 {FormatTyp ( t.UnderlyingSystemType )}" ;
                }
                case XamlType xt : return "𝓧" + FormatValue ( xt.UnderlyingType , out b1 ) ;
                case string ss :   return ss ;
                case ICollection < object > col :
                {
                    var q = from o in col select FormatValue ( o , out b ) ;
                    b1 = true ;
                    return string.Join ( ", " , q ) ;
                }
            }

            b1 = true ;
            return r.ToString ( ) ;
        }


        [ NotNull ]
        // ReSharper disable once UnusedMember.Local
        private static string FormatValue < T > ( [ NotNull ] IEnumerable < T > r , out bool b1 )
        {
            b1 = true ;
            return string.Join ( ", " , r.Select ( v => FormatValue ( v , out _ ) ) ) ;
        }

        /// <summary>Formats the typ.</summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for FormatTyp
        [ NotNull ]
        protected static string FormatTyp ( [ NotNull ] Type type )
        {
            if ( type == null )
            {
                throw new ArgumentNullException ( nameof ( type ) ) ;
            }

            if ( type.IsGenericType )
            {
                return FormatConstructedGenericType ( type ) ;
            }

            var typeName = type.Name ;
            return Alter ( typeName ) ;
        }

        [ NotNull ]
        private static string FormatConstructedGenericType ( [ NotNull ] Type type )
        {
            var type2 = type.GetGenericTypeDefinition ( ) ;
            return Alter (
                          $"{type2.Name}<{string.Join ( "," , type.GenericTypeArguments.Select ( ( type1 , i ) => FormatTyp ( type1 ) ) )}>"
                         ) ;
        }

        [ NotNull ]
        private static string Alter ( [ NotNull ] string s )
        {
            return s.Replace ( "<" , " 〈 " )
                    .Replace ( ">" , " 〉 " )
                    .Replace ( "[" , " 【 " )
                    .Replace ( "]" , " 】 " )
                    .Replace ( "`" , "❛" ) ;
        }

        [ CanBeNull ]
        private static string MethodSpec ( [ NotNull ] IInvocation invocation )
        {
            var declType = invocation.Method.DeclaringType ;
            if ( declType == null )
            {
                return null ;
            }

            var formatTyp = FormatTyp ( declType ) ;
            var type = invocation.TargetType ;
            var typ = FormatTyp ( type ) ;

            var m = " " + invocation.Method.Name ;
            if ( invocation.Method.IsSpecialName
                 && invocation.Method.Name.StartsWith (
                                                       "get_"
                                                     , StringComparison.InvariantCulture
                                                      ) )
            {
                m = $"𝜙 {invocation.Method.Name.Substring ( 4 )}" ;
            }

            return ( declType == type ? $"{formatTyp,33}" : $"{formatTyp,16} {typ,16}" )
                   + " "
                   + m ;

        }
    }

    /// <summary></summary>
    /// <seealso cref="BaseInterceptor" />
    /// TODO Edit XML Comment Template for BaseInterceptorImpl
    public sealed class BaseInterceptorImpl : BaseInterceptor
    {
        private readonly Stack < StackInfo > _stack = new Stack < StackInfo > ( ) ;
        private          int                 _callDepth ;

        /// <summary>
        ///     Initializes a new instance of the
        ///     <see cref="BaseInterceptorImpl" /> class.
        /// </summary>
        /// <param name="out">The out.</param>
        /// <param name="generator">The generator.</param>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for #ctor
        public BaseInterceptorImpl ( Action < string > @out , ProxyGenerator generator = null ) :
            base ( generator )
        {
            WriteLine = @out ;
        }

        /// <summary>Intercepts the specified invocation.</summary>
        /// <param name="invocation">The invocation.</param>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for Intercept
        public override void Intercept ( [ NotNull ] IInvocation invocation )
        {
            Interlocked.Increment ( ref _callDepth ) ;
            var x = _stack.Any ( ) ? _stack.Peek ( ) : null ;
            var myX = new StackInfo ( ) ;
            _stack.Push ( myX ) ;
            var writeLine = WriteLine ;
            if ( x != null )
            {
                x.Written = true ;
                writeLine ( "" ) ;
            }


            var formatted = FormatInvocation ( invocation , "" ) ;
            // ReSharper disable once UnusedVariable
            var a = new InvocationEventArgs { Formatted = formatted } ;

            DumpInvocation ( invocation , _callDepth ) ;
            invocation.Proceed ( ) ;
            try
            {
                DumpReturnValue ( invocation , _callDepth , ! myX.Written ) ;
                if ( ProxyGenerator            != null
                     && invocation.ReturnValue != null
                     && ! invocation.ReturnValue.GetType ( ).IsSealed
                     && ! ( invocation.ReturnValue is IProxyTargetAccessor ) )
                {
                    var r = invocation.ReturnValue ;
                    if ( r is XamlType typ )
                    {
                        var invoker =
                            ( XamlTypeInvoker ) ProxyGenerator.CreateClassProxyWithTarget (
                                                                                           typeof (
                                                                                               XamlTypeInvoker
                                                                                           )
                                                                                         , typ
                                                                                              .Invoker
                                                                                         , new[]
                                                                                           {
                                                                                               r
                                                                                           }
                                                                                         , this
                                                                                          ) ;
                        object[] args = { typ.UnderlyingType , typ.SchemaContext , invoker } ;


                        invocation.ReturnValue =
                            ProxyGenerator.CreateClassProxyWithTarget (
                                                                       r.GetType ( )
                                                                     , r
                                                                     , args
                                                                     , this
                                                                      ) ;
                    }
                    else if ( r.GetType ( ).IsGenericType
                              && r.GetType ( ).GetGenericTypeDefinition ( )
                              == typeof ( XamlValueConverter < object > )
                                 .GetGenericTypeDefinition ( ) )
                    {
                        object[] args = null ;
                        switch ( r )
                        {
                            case XamlValueConverter < ValueSerializer > q :
                                args = new object[] { q.ConverterType , q.TargetType , q.Name } ;
                                break ;
                            case XamlValueConverter < TypeConverter > z :
                                args = new object[] { z.ConverterType , z.TargetType , z.Name } ;
                                break ;
                        }

                        if ( args != null )
                        {
                            invocation.ReturnValue =
                                ProxyGenerator.CreateClassProxyWithTarget (
                                                                           r.GetType ( )
                                                                         , r
                                                                         , args
                                                                         , this
                                                                          ) ;
                        }
                    }
                    else if ( r.GetType ( ).IsGenericType
                              && r.GetType ( ).GetGenericTypeDefinition ( )
                              == typeof ( ReadOnlyCollection < object > )
                                 .GetGenericTypeDefinition ( ) )
                    {
                        var propInfo = r.GetType ( )
                                        .GetField (
                                                   "list"
                                                 , BindingFlags.NonPublic | BindingFlags.Instance
                                                  ) ;
                        if ( propInfo == null )
                        {
                            return ;
                        }

                        var args = new[] { propInfo.GetValue ( r ) } ;
                        invocation.ReturnValue =
                            ProxyGenerator.CreateClassProxyWithTarget (
                                                                       r.GetType ( )
                                                                     , r
                                                                     , args
                                                                     , this
                                                                      ) ;
                    }
                    else
                    {
                        switch ( r )
                        {
                            case XamlDirective d :
                            {
                                object[] args =
                                {
                                    d.GetXamlNamespaces ( ) , d.Name , d.Type , d.TypeConverter
                                  , d.AllowedLocation
                                } ;

                                invocation.ReturnValue =
                                    ProxyGenerator.CreateClassProxyWithTarget (
                                                                               r.GetType ( )
                                                                             , r
                                                                             , args
                                                                             , this
                                                                              ) ;
                                break ;
                            }
                            case NamespaceDeclaration ns :
                                invocation.ReturnValue =
                                    ProxyGenerator.CreateClassProxyWithTarget (
                                                                               r.GetType ( )
                                                                             , r
                                                                             , new object[]
                                                                               {
                                                                                   ns.Namespace
                                                                                 , ns.Prefix
                                                                               }
                                                                             , this
                                                                              ) ;
                                break ;
                            default :
                                try
                                {
                                    invocation.ReturnValue =
                                        ProxyGenerator.CreateClassProxyWithTarget (
                                                                                   r.GetType ( )
                                                                                 , r
                                                                                 , this
                                                                                  ) ;
                                }
                                catch ( InvalidProxyConstructorArgumentsException )
                                {
                                    writeLine (
                                               "Constructors for ⮜"
                                               + FormatTyp ( r.GetType ( ) )
                                               + "⮞"
                                              ) ;
                                    foreach ( var constructorInfo in r
                                                                    .GetType ( )
                                                                    .GetConstructors ( ) )
                                    {
                                        writeLine (
                                                   FormatTyp (
                                                              constructorInfo.DeclaringType
                                                              ?? throw new
                                                                  InvalidOperationException ( )
                                                             )
                                                   + " ( "
                                                   + string.Join (
                                                                  " , "
                                                                , constructorInfo
                                                                 .GetParameters ( )
                                                                 .Select (
                                                                          ( info , i )
                                                                              => FormatTyp (
                                                                                            info
                                                                                               .ParameterType
                                                                                           )
                                                                                 + " "
                                                                                 + info.Name
                                                                                 + ( info
                                                                                        .HasDefaultValue
                                                                                         ? " = "
                                                                                           + info
                                                                                              .DefaultValue
                                                                                         : "" )
                                                                         )
                                                                 )
                                                  ) ;
                                    }
                                }

                                break ;
                        }
                    }
                }
            }
            catch ( Exception ex )
            {
                writeLine ( "--Exception--" ) ;
                writeLine ( ex.ToString ( ) ) ; //.GetType ( ) ) ;
                //Console.WriteLine ( ex.Message ) ;
                writeLine ( "--End Exception--" ) ;
            }
            finally
            {
                _stack.Pop ( ) ;
                Interlocked.Decrement ( ref _callDepth ) ;
            }
        }
    }

    /// <summary>Main usage point for the <see cref="ProxyUtils" />.</summary>
    /// <seealso cref="ProxyUtilsBase" />
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for ProxyUtils
    public sealed class ProxyUtils : ProxyUtilsBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="System.Object" />
        ///     class.
        /// </summary>
        /// <param name="writeOut">Delegate or lambda to accept raw debug output.</param>
        /// <param name="interceptor">Interceptor to use.</param>
        public ProxyUtils ( Action < string > writeOut , IInterceptor interceptor ) : base (
                                                                                            writeOut
                                                                                          , interceptor
                                                                                           )
        {
        }
    }

    /// <summary>Base class for <seealso cref="ProxyUtils" />.</summary>
    public class ProxyUtilsBase : ProxyUtilsEvents
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        /// <summary>The proxy generator</summary>
        protected static readonly ProxyGenerator ProxyGenerator = new ProxyGenerator ( ) ;

        private readonly IInterceptor _interceptor ;


        // ReSharper disable once NotAccessedField.Local
        private Action < string > _writeOut ;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProxyUtilsBase" />
        ///     class.
        /// </summary>
        /// <param name="writeOut"></param>
        /// <param name="interceptor"></param>
        protected ProxyUtilsBase ( Action < string > writeOut , IInterceptor interceptor )
        {
            _writeOut    = writeOut ;
            _interceptor = interceptor ;
        }

        /// <summary>
        ///     Factory method to create a proper interceptor instance for use with
        ///     the <seealso cref="ProxyUtils" /> class.
        /// </summary>
        /// <param name="out">Handler for raw debug output.</param>
        /// <returns>The interceptor instance.</returns>
        [ NotNull ]
        public static IInterceptor CreateInterceptor ( Action < string > @out )
        {
            return new BaseInterceptorImpl ( @out ) ;
        }


        /// <summary>
        ///     Performs a XAML transform with proxied objects, to see the call
        ///     structure and parameters..
        /// </summary>
        /// <param name="instance">The instance to write as XAML XML.</param>
        /// <returns>output XML</returns>
        [ NotNull ]
        public string TransformXaml ( object instance )
        {
            var stringWriter = CreateStringWriter ( ) ;

            var xmlWriterProxy = CreateXmlWriter ( stringWriter ) ;
            var context = CreateXamlSchemaContext ( ) ;
            var settings = CreateXamlObjectReaderSettings ( ) ;
            var reader = CreateXamlObjectReader ( instance , context , settings ) ;

            var xamlWriterProxy = CreateXamlXmlWriter ( xmlWriterProxy , context ) ;


            try
            {
                XamlServices.Transform (
                                        ( System.Xaml.XamlReader ) reader
                                      , xamlWriterProxy
                                      , true
                                       ) ;
                //XamlServices.Save (xamlWriterProxy , this ) ;
            }
            catch ( Exception ex )
            {
                Console.WriteLine ( ex ) ;
            }

            return stringWriter.ToString ( ) ;
        }


        /// <summary>
        ///     Performs a XAML transform with proxied objects, to see the call
        ///     structure and parameters..
        /// </summary>
        /// <param name="inputUri"></param>
        /// <returns>output XML</returns>
        public object TransformXaml2 ( [ NotNull ] string inputUri )
        {
            var fieldInfo = typeof ( XamlReader ).GetField (
                                                            "_xamlSharedContext"
                                                          , BindingFlags.NonPublic
                                                            | BindingFlags.Static
                                                           ) ;
            if ( fieldInfo != null )
            {
                var o = fieldInfo.GetValue ( null ) ;
                Logger.Info ( "{o}" , o ) ;
            }

            // ReSharper disable once UnusedVariable
            var context = CreateXamlSchemaContext ( ) ;
            // ReSharper disable once UnusedVariable
            var settings = CreateXamlObjectReaderSettings ( ) ;
            var xmlReader = CreateXmlReader ( inputUri ) ;

            var instance = XamlReader.Load ( xmlReader ) ;

            return instance ;
        }

        /// <summary>Creates the xaml XML writer.</summary>
        /// <param name="xmlWriterProxy">The XML writer proxy.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for CreateXamlXmlWriter
        protected virtual XamlXmlWriter CreateXamlXmlWriter (
            XmlWriter         xmlWriterProxy
          , XamlSchemaContext context
        )
        {
            var args = new object[] { xmlWriterProxy , context } ;
            var xamlWriterProxy = ( XamlXmlWriter ) ProxyGenerator.CreateClassProxy (
                                                                                     typeof (
                                                                                         XamlXmlWriter
                                                                                     )
                                                                                   , args
                                                                                   , _interceptor
                                                                                    ) ;
            return xamlWriterProxy ;
        }

        /// <summary>Creates the xaml reader.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for CreateXamlReader
        protected virtual XamlReader CreateXamlReader ( XamlSchemaContext context )
        {
            // ReSharper disable once UnusedVariable
            var x = new ParserContext ( ) ;
            // x.XamlTypeMapper = new XamlTypeMapper();

            var p = ( XamlReader ) ProxyGenerator.CreateClassProxy (
                                                                    typeof ( XamlReader )
                                                                  , Array.Empty < object > ( )
                                                                  , _interceptor
                                                                   ) ;
            return p ;
        }

        /// <summary>
        ///     Creates a proxied XamlObjectReader based on the given paameters. From MS
        ///     docs:
        ///     <quoteInline>
        ///         Provides a <see cref="System.Xaml.XamlReader" />
        ///         implementation that reads object graphs and generates a XAML node
        ///         stream.
        ///     </quoteInline>
        ///     Constructor:
        ///     <quoteInline>
        ///         Initializes a new instance of the XamlObjectReader class
        ///         with the specified schema context and reader settings
        ///     </quoteInline>
        ///     .
        /// </summary>
        /// <param name="instance">The root of the object tree to read.</param>
        /// <param name="context">The schema context for the reader to use.</param>
        /// <param name="settings">A settings object.</param>
        /// <returns>Proxied XamlObjetReader instance.</returns>
        /// <seealso cref="XmlReader" />
        protected virtual object CreateXamlObjectReader (
            object                   instance
          , XamlSchemaContext        context
          , XamlObjectReaderSettings settings
        )
        {
            return ProxyGenerator.CreateClassProxy (
                                                    typeof ( XamlObjectReader )
                                                  , new[] { instance , context , settings }
                                                  , _interceptor
                                                   ) ;
        }

        /// <summary>Creates the xaml object reader settings.</summary>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for CreateXamlObjectReaderSettings
        [ NotNull ]
        protected virtual XamlObjectReaderSettings CreateXamlObjectReaderSettings ( )
        {
            return new XamlObjectReaderSettings
                   {
                       AllowProtectedMembersOnRoot      = true
                     , RequireExplicitContentVisibility = false
                     , ValuesMustBeString               = false
                   } ;
        }

        /// <summary>Creates the XML writer.</summary>
        /// <param name="stringWriter">The string writer.</param>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for CreateXmlWriter
        protected virtual XmlWriter CreateXmlWriter ( [ NotNull ] StringWriter stringWriter )
        {
            var x = XmlWriter.Create ( stringWriter ) ;
            var xmlWriterProxy = ProxyGenerator.CreateClassProxyWithTarget ( x , _interceptor ) ;
            return xmlWriterProxy ;
        }

        /// <summary>Creates the XML reader.</summary>
        /// <param name="inputUri">The input URI.</param>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for CreateXmlReader
        protected virtual XmlReader CreateXmlReader ( [ NotNull ] string inputUri )
        {
            var x = XmlReader.Create ( inputUri , new XmlReaderSettings ( ) ) ;
            var p = ProxyGenerator.CreateClassProxyWithTarget ( x , _interceptor ) ;
            return p ;
        }

        /// <summary>Creates the string writer.</summary>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for CreateStringWriter
        protected virtual StringWriter CreateStringWriter ( )
        {
            return ProxyGenerator.CreateClassProxy < StringWriter > ( _interceptor ) ;
        }

        /// <summary>Creates the xaml schema context.</summary>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for CreateXamlSchemaContext
        protected virtual XamlSchemaContext CreateXamlSchemaContext ( )
        {
            return ProxyGenerator.CreateClassProxy < XamlSchemaContext > ( _interceptor ) ;
        }

        /// <summary>Creates the proxy.</summary>
        /// <param name="writeLine">The write line.</param>
        /// <param name="interceptor">The interceptor.</param>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for CreateProxy
        public static ProxyUtils CreateProxy (
            Action < string > writeLine
          , IInterceptor      interceptor
        )
        {
            return ( ProxyUtils ) ProxyGenerator.CreateClassProxy (
                                                                   typeof ( ProxyUtils )
                                                                 , new object[]
                                                                   {
                                                                       writeLine , interceptor
                                                                   }
                                                                 , interceptor
                                                                  ) ;
        }
    }


    internal sealed class StackInfo

    {
        public bool Written ;

        /// <summary>
        ///     Initializes a new instance of the <see cref="System.Object" />
        ///     class.
        /// </summary>
        public StackInfo ( bool written = false ) { Written = written ; }
    }

    /// <summary></summary>
    /// <autogeneratedoc />
    public class ProxyUtilsEvents
    {
        /// <summary>Occurs when [begin invocation].</summary>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for BeginInvocation
        // ReSharper disable once EventNeverSubscribedTo.Global
        public event EventHandler < InvocationEventArgs > BeginInvocation ;

        /// <summary>Raises the <see cref="BeginInvocation" /> event.</summary>
        /// <param name="e">
        ///     The <see cref="InvocationEventArgs" /> instance containing
        ///     the event data.
        /// </param>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for OnBeginInvocation
        protected virtual void OnBeginInvocation ( InvocationEventArgs e )
        {
            BeginInvocation?.Invoke ( this , e ) ;
        }
    }

    /// <summary></summary>
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for InvocationEventArgs
    public sealed class InvocationEventArgs
    {
        /// <summary>The formatted</summary>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for formatted
        public string Formatted { get ; set ; }

        /// <summary>The method information</summary>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for methodInfo

        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public MethodInfo Info { get ; }
    }
}