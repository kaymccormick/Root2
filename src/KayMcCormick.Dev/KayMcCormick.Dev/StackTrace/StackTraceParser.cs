#region Copyright (c) 2011 Atif Aziz. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion


using System ;
using System.Collections.Generic ;
using System.Linq ;
using System.Text.RegularExpressions ;
using JetBrains.Annotations ;

namespace KayMcCormick.Dev.StackTrace
{
    #region Imports
    #endregion


    /// <summary>
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class StackTraceParser
    {
        private const string Space    = @"[\x20\t]" ;
        private const string NotSpace = @"[^\x20\t]" ;

        private static readonly Regex Regex = new Regex (
                                                         @"
            ^
            "
                                                         + Space
                                                         + @"*
            \w+ "
                                                         + Space
                                                         + @"+
            (?<frame>
                (?<type> "
                                                         + NotSpace
                                                         + @"+ ) \.
                (?<method> "
                                                         + NotSpace
                                                         + @"+? ) "
                                                         + Space
                                                         + @"*
                (?<params>  \( ( "
                                                         + Space
                                                         + @"* \)
                               |                    (?<pt> .+?) "
                                                         + Space
                                                         + @"+ (?<pn> .+?)
                                 (, "
                                                         + Space
                                                         + @"* (?<pt> .+?) "
                                                         + Space
                                                         + @"+ (?<pn> .+?) )* \) ) )
                ( "
                                                         + Space
                                                         + @"+
                    ( # Microsoft .NET stack traces
                    \w+ "
                                                         + Space
                                                         + @"+
                    (?<file> ( [a-z] \: # Windows rooted path starting with a drive letter
                             | / )      # *nix rooted path starting with a forward-slash
                             .+? )
                    \: \w+ "
                                                         + Space
                                                         + @"+
                    (?<line> [0-9]+ ) \p{P}?
                    | # Mono stack traces
                    \[0x[0-9a-f]+\] "
                                                         + Space
                                                         + @"+ \w+ "
                                                         + Space
                                                         + @"+
                    <(?<file> [^>]+ )>
                    :(?<line> [0-9]+ )
                    )
                )?
            )
            \s*
            $"
                                                       , RegexOptions.IgnoreCase
                                                         | RegexOptions.Multiline
                                                         | RegexOptions.ExplicitCapture
                                                         | RegexOptions.CultureInvariant
                                                         | RegexOptions.IgnorePatternWhitespace
                                                         | RegexOptions.Compiled
                                                        ,
                                                         // Cap the evaluation time to make it obvious should the expression
                                                         // fall into the "catastrophic backtracking" trap due to over
                                                         // generalization.
                                                         // https://github.com/atifaziz/StackTraceParser/issues/4
                                                         TimeSpan.FromSeconds ( 5 )
                                                        ) ;

        /// <summary>
        /// </summary>
        /// <param name="text"></param>
        /// <param name="selector"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [ JetBrains.Annotations.NotNull ]
        public static IEnumerable < T > Parse < T > (
            [ JetBrains.Annotations.NotNull ] string text
          , [ JetBrains.Annotations.NotNull ] Func < string , string , string , string ,
                IEnumerable < KeyValuePair < string , string > > , string , string , T > selector
        )
        {
            if ( selector == null )
            {
                throw new ArgumentNullException ( nameof ( selector ) ) ;
            }

            return Parse (
                          text
                        , ( idx , len , txt ) => txt
                        , ( t ,   m ) => new { Type = t , Method = m }
                        , ( pt ,  pn ) => new KeyValuePair < string , string > ( pt , pn )
                        , ( pl ,  ps ) => new { List = pl , Items = ps }
                        , ( fn ,  ln ) => new { File = fn , Line  = ln }
                        , ( f , tm , p , fl ) => selector (
                                                           f
                                                         , tm.Type
                                                         , tm.Method
                                                         , p.List
                                                         , p.Items
                                                         , fl.File
                                                         , fl.Line
                                                          )
                         ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="text"></param>
        /// <param name="tokenSelector"></param>
        /// <param name="methodSelector"></param>
        /// <param name="parameterSelector"></param>
        /// <param name="parametersSelector"></param>
        /// <param name="sourceLocationSelector"></param>
        /// <param name="selector"></param>
        /// <typeparam name="TToken"></typeparam>
        /// <typeparam name="TMethod"></typeparam>
        /// <typeparam name="TParameters"></typeparam>
        /// <typeparam name="TParameter"></typeparam>
        /// <typeparam name="TSourceLocation"></typeparam>
        /// <typeparam name="TFrame"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [ JetBrains.Annotations.NotNull ]
        public static IEnumerable < TFrame >
            Parse < TToken , TMethod , TParameters , TParameter , TSourceLocation , TFrame > (
                [ JetBrains.Annotations.NotNull ] string                                text
              , [ JetBrains.Annotations.NotNull ] Func < int , int , string , TToken >  tokenSelector
              , [ JetBrains.Annotations.NotNull ] Func < TToken , TToken , TMethod >    methodSelector
              , [ JetBrains.Annotations.NotNull ] Func < TToken , TToken , TParameter > parameterSelector
              , [ JetBrains.Annotations.NotNull ] Func < TToken , IEnumerable < TParameter > , TParameters >
                    parametersSelector
              , [ JetBrains.Annotations.NotNull ] Func < TToken , TToken , TSourceLocation >
                    sourceLocationSelector
              , [ JetBrains.Annotations.NotNull ] Func < TToken , TMethod , TParameters , TSourceLocation , TFrame > selector
            )
        {
            if ( tokenSelector == null )
            {
                throw new ArgumentNullException ( nameof ( tokenSelector ) ) ;
            }

            if ( methodSelector == null )
            {
                throw new ArgumentNullException ( nameof ( methodSelector ) ) ;
            }

            if ( parameterSelector == null )
            {
                throw new ArgumentNullException ( nameof ( parameterSelector ) ) ;
            }

            if ( parametersSelector == null )
            {
                throw new ArgumentNullException ( nameof ( parametersSelector ) ) ;
            }

            if ( sourceLocationSelector == null )
            {
                throw new ArgumentNullException ( nameof ( sourceLocationSelector ) ) ;
            }

            if ( selector == null )
            {
                throw new ArgumentNullException ( nameof ( selector ) ) ;
            }

            return
                from Match m in Regex.Matches ( text )
                select m.Groups
                into groups
                let pt = groups[ "pt" ].Captures
                let pn = groups[ "pn" ].Captures
                select selector (
                                 Token ( groups[ "frame" ] , tokenSelector )
                               , methodSelector (
                                                 Token ( groups[ "type" ] ,   tokenSelector )
                                               , Token ( groups[ "method" ] , tokenSelector )
                                                )
                               , parametersSelector (
                                                     Token ( groups[ "params" ] , tokenSelector )
                                                    ,
                                                     from i in Enumerable.Range ( 0 , pt.Count )
                                                     select parameterSelector (
                                                                               Token (
                                                                                      pt[ i ]
                                                                                    , tokenSelector
                                                                                     )
                                                                             , Token (
                                                                                      pn[ i ]
                                                                                    , tokenSelector
                                                                                     )
                                                                              )
                                                    )
                               , sourceLocationSelector (
                                                         Token ( groups[ "file" ] , tokenSelector )
                                                       , Token ( groups[ "line" ] , tokenSelector )
                                                        )
                                ) ;
        }

        private static T Token < T > (
            [ JetBrains.Annotations.NotNull ] Capture                         capture
          , [ JetBrains.Annotations.NotNull ] Func < int , int , string , T > tokenSelector
        )
        {
            return tokenSelector ( capture.Index , capture.Length , capture.Value ) ;
        }
    }
}