#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// KayMcCormick.Dev
// MyLog4JXmlEventLayoutRenderer.cs
// 
// 2020-03-19-11:57 PM
// 
// ---
#endregion
using JetBrains.Annotations ;
using NLog.Internal.Fakeables ;
using NLog.LayoutRenderers ;

namespace KayMcCormick.Dev.Logging
{
    internal class MyLog4JXmlEventLayoutRenderer : Log4JXmlEventLayoutRenderer
    {
        public MyLog4JXmlEventLayoutRenderer ( ) { SetupXmlEventLayoutRenderer ( this ) ; }

        public MyLog4JXmlEventLayoutRenderer ( IAppDomain appDomain ) : base ( appDomain )
        {
            SetupXmlEventLayoutRenderer ( this ) ;
        }

        public static void SetupXmlEventLayoutRenderer ( [ JetBrains.Annotations.NotNull ] Log4JXmlEventLayoutRenderer x )
        {
            x.IncludeAllProperties = true ;
            x.IncludeCallSite      = true ;
            //x.IncludeMdlc          = true ;
            x.IncludeSourceInfo = true ;
            x.IncludeNLogData   = true ;
            //x.IncludeNdlc          = true ;
            x.IndentXml = true ;
        }
    }
}