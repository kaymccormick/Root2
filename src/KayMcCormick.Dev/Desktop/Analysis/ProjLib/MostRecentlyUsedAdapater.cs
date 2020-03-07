#if VSSETTINGS
using System;
using System.Collections.Generic ;
using System.Diagnostics;
using System.IO;

using Microsoft.VisualStudio.Settings;

namespace ProjLib
{
    public class MostRecentlyUsedAdapater : IMruItems
    {
        private readonly IVsInstance _vsInstance ;
        private readonly IMruItemProvider _provider ;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public MostRecentlyUsedAdapater (IVsInstance vsInstance, IMruItemProvider provider )
        {
            _vsInstance = vsInstance ;
            Debug.Assert ( _vsInstance.InstanceId != null ) ;
            _provider = provider ;
        }

        public List < IMruItem > GetMruItemList ( )
        {
            var yy = _vsInstance ;
            return _provider.GetMruItemListFor ( _vsInstance ) ;
        }


    }
}
#endif