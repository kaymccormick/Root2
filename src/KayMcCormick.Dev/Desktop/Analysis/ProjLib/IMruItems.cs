using System.Collections.Generic ;
using NLog.LayoutRenderers.Wrappers ;

namespace ProjLib
{
    public interface IMruItemProvider
    {
        List < IMruItem > GetMruItemListFor ( IVsInstance vsInstance ) ;
    }

    public interface IMruItems
    {
        List < IMruItem > GetMruItemList ( ) ;
    }
}