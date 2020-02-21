﻿#region header
// Kay McCormick (mccor)
// 
// Common
// CommonTests
// TestConverter2.cs
// 
// 2020-01-30-5:54 AM
// 
// ---
#endregion
using System.Collections ;
using System.Globalization ;
using System.Linq ;
using Autofac ;
using Autofac.Core ;
using KayMcCormick.Test.Common ;
using Tests.Lib.Fixtures ;
using Tests.Lib.Misc ;
using WpfApp.Core.Converters ;
using WpfApp.Core.Interfaces ;
using Xunit ;
using Xunit.Abstractions ;

namespace Tests.Main.Converters
{
    /// <summary></summary>
    /// <seealso cref="Xunit.IClassFixture{T}" />
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for RegistrationConverterTests
    [ LogTestMethod ] [ BeforeAfterLogger ] [Collection("GeneralPurpose")]
    public class RegistrationConverterTests : IClassFixture < TestContainerFixture >
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="System.Object" />
        ///     class.
        /// </summary>
        public RegistrationConverterTests (
            TestContainerFixture fixture
          , ITestOutputHelper    output
        )
        {
            _fixture = fixture ;
            _output  = output ;
        }

        // ReSharper disable once InternalOrPrivateMemberNotDocumented
        private readonly TestContainerFixture _fixture ;

        // ReSharper disable once InternalOrPrivateMemberNotDocumented
        private readonly ITestOutputHelper _output ;

        /// <summary>Tests the conversion1.</summary>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for TestConversion1
        [ Fact ]
        public void TestConversion1 ( )
        {
            // takes IComponentLifetime

            _fixture.Container.Resolve < IRandom > ( ) ;
            var objIdProv = _fixture.Container.Resolve < IObjectIdProvider > ( ) ;
            var converter = new RegistrationConverter ( _fixture.Container , objIdProv ) ;

            var regs = _fixture.Container.ComponentRegistry.Registrations ;
            var value = regs.Where (
                                    ( registration , i )
                                        => registration.Services.Any (
                                                                      service
                                                                          => service is TypedService
                                                                                 t
                                                                             && t.ServiceType
                                                                             == typeof ( IRandom )
                                                                     )
                                   )
                            .First ( ) ;
            var result = converter.Convert (
                                            value
                                          , typeof ( IEnumerable )
                                          , null
                                          , CultureInfo.CurrentCulture
                                           ) ;
            Assert.NotNull ( result ) ;
            var enumerable = ( IEnumerable ) result ;
            var collection = enumerable as object[] ?? enumerable.Cast < object > ( ).ToArray ( ) ;
            Assert.NotEmpty ( collection ) ;
            foreach ( var o in collection )
            {
                _output.WriteLine ( $"{o}" ) ;
            }
        }
    }
}