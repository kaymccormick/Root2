using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;

namespace AnalysisAppLib
{
    public sealed class ParameterInfo
    {
        public string Name { get ; }

        public string TypeDisplayString { get ; }

        public string TypeFullName { get ; }

        // ReSharper disable once CollectionNeverQueried.Global
        public readonly List < CustomModifierInfo > CustomModifiers =
            new List < CustomModifierInfo > ( ) ;

        public ParameterInfo (
            string name
            // ReSharper disable once SuggestBaseTypeForParameter
            , [ NotNull ] ITypeSymbol            typeSymbol
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