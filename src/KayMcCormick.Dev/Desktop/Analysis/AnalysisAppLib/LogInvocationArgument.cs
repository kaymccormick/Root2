﻿using System.ComponentModel ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;

namespace AnalysisAppLib
{
    internal class LogInvocationArgument : ILogInvocationArgument
    {
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]

        private readonly ArgumentSyntax _syntax ;

        public LogInvocationArgument ( ArgumentSyntax syntax )
        {
            _syntax = syntax ;
            var jsonOut = Transforms.TransformExpr(syntax.Expression);
            Pojo = jsonOut;
        }

        
        public LogInvocationArgument ( ) {
        }

        private string jSON;

        public string GetJSON ( ILogInvocationArgument arg )
        {
            return jSON;
        }

        public void SetJSON(string value)
        {
            jSON = value;
        }

        public object Pojo { get ; set ; }

    }
}