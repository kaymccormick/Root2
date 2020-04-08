#region header
// Kay McCormick (mccor)
// 
// Analysis
// ConsoleApp1
// ExampleSyntax.cs
// 
// 2020-04-08-5:30 AM
// 
// ---
#endregion
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Windows.Markup ;
using JetBrains.Annotations ;

namespace ConsoleApp1
{
    [ ContentProperty ( "Example" ) ]
    public sealed class ExampleSyntax
    {
        private int    _kind ;
        private string _example ;
        private string _typeName ;

        private readonly ExampleTokens _tokens = new ExampleTokens ( ) ;
        private          int           _id ;

        public ExampleSyntax (
            int                         kind
          , string                      example
          , string                      typeName
          , [ NotNull ] List < SToken > tokens
          , int                         id
        )
        {
            Kind     = kind ;
            Example  = example ;
            TypeName = typeName ;
            Id       = id ;
            foreach ( var sToken in tokens )
            {
                Tokens.Add ( sToken ) ;
            }
        }

        public int Kind { [ UsedImplicitly ] get { return _kind ; } set { _kind = value ; } }

        public string Example { [ UsedImplicitly ] get { return _example ; } set { _example = value ; } }

        public string TypeName { [ UsedImplicitly ] get { return _typeName ; } set { _typeName = value ; } }

        public int Id { [ UsedImplicitly ] get { return _id ; } set { _id = value ; } }

        [ DesignerSerializationVisibility ( DesignerSerializationVisibility.Content ) ]
        public ExampleTokens Tokens { get { return _tokens ; } }
    }
}