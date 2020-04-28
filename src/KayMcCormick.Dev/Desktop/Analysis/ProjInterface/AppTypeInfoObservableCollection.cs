using System.Collections.Generic;
using System.Collections.ObjectModel;
using AnalysisAppLib.Syntax;
using JetBrains.Annotations;

namespace ProjInterface
{
    public class AppTypeInfoObservableCollection : ObservableCollection < AppTypeInfo >
    {
        public AppTypeInfoObservableCollection ( ) { }

        public AppTypeInfoObservableCollection ( [ NotNull ] List < AppTypeInfo > list ) :
            base ( list )
        {
        }

        public AppTypeInfoObservableCollection (
            [ NotNull ] IEnumerable < AppTypeInfo > collection
        ) : base ( collection )
        {
        }
    }
}