﻿using System.Linq ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.TestLib ;
using Xunit ;

namespace Tests.Main.Utils
{
    /// <summary></summary>
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for InfoContextTests
    [LogTestMethod ] [ BeforeAfterLogger ][Collection("GeneralPurpose")]
    public class InfoContextTests
    {
        /// <summary>Enumerators the test.</summary>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for EnumeratorTest
        [Fact ]
        public void EnumeratorTest ( )
        {
            const string name = "test" ;
            const string objectContext = "hello" ;
            var x = new InfoContext ( name , objectContext ) ;
            var objects = x.ToList ( ) ;
            Assert.Equal ( 2 ,             objects.Count ) ;
            Assert.Equal ( name ,          objects[ 0 ] ) ;
            Assert.Equal ( objectContext , objects[ 1 ] ) ;
        }
    }
}