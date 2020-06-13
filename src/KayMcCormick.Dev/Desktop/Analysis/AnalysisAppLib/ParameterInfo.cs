using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ParameterInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get ; }

        /// <summary>
        /// 
        /// </summary>
        public string TypeDisplayString { get ; }

        /// <summary>
        /// 
        /// </summary>
        public string TypeFullName { get ; }

        // ReSharper disable once CollectionNeverQueried.Global
        /// <summary>
        /// 
        /// </summary>
        public readonly List < CustomModifierInfo > CustomModifiers =
            new List < CustomModifierInfo > ( ) ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="typeSymbol"></param>
        /// <param name="select"></param>
        /// <param name="typeDisplayString"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ParameterInfo (
            string name
            // ReSharper disable once SuggestBaseTypeForParameter
            , [ JetBrains.Annotations.NotNull ] ITypeSymbol            typeSymbol
            , IEnumerable < CustomModifierInfo > select
            , string                             typeDisplayString
        )
        {
            if ( typeSymbol == null )
            {
                throw new ArgumentNullException ( nameof ( typeSymbol ) ) ;
            }

            Name              = name ;
            TypeDisplayString = typeDisplayString ;
            TypeFullName = typeSymbol.ContainingNamespace?.MetadataName
                           + '.'
                           + typeSymbol.MetadataName ;
            CustomModifiers.AddRange ( select ) ;
        }
    }
}