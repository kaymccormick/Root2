#region header
// Kay McCormick (mccor)
// 
// Analysis
// ProjInterface
// Test1.cs
// 
// 2020-04-25-11:46 AM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using Autofac.Features.AttributeFilters ;
using KayMcCormick.Dev ;

namespace ProjInterface
{
    public class Test1
    {
        private readonly IEnumerable < Type > _types ;

        public Test1 (
            [ MetadataFilter ( "UiConversion" , true ) ]
            IEnumerable < Type > types,
            [ MetadataFilter ( "" , true ) ]
            IEnumerable < Type > types
        )
        {
            _types = types ;
            foreach ( var type in _types )
            {
                DebugUtils.WriteLine ( $"** {type.FullName}" ) ;
            }
        }
    }
}