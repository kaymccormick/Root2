﻿using System ;
using System.Reflection ;
using System.Windows ;
using System.Windows.Media ;

namespace Tests.Lib.Utils
{
    /// <summary></summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Windows.Window" />
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for WindowWrap`1
    // ReSharper disable once ClassNeverInstantiated.Global
    public class WindowWrap < T > : Window
        where T : Visual
    {
        /// <summary>Initializes a new instance of the <see cref="WindowWrap{T}"/> class.</summary>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for #ctor
        public WindowWrap ( )
        {
            try
            {
                var contentInstance = Activator.CreateInstance < T > ( ) ;
                Content = contentInstance ;
            }
            catch ( TargetInvocationException e )
            {
                if ( e.InnerException != null )
                {
                    throw e.InnerException ;
                }
            }
        }
    }
}