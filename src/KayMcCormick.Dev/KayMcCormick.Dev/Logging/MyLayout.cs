#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// KayMcCormick.Dev
// MyLayout.cs
// 
// 2020-03-19-11:57 PM
// 
// ---
#endregion
using NLog ;
using NLog.LayoutRenderers ;
using NLog.Layouts ;

namespace KayMcCormick.Dev.Logging
{
    internal class MyLayout : Layout
    {
        private readonly LayoutRenderer _layoutRenderer ;
        public MyLayout ( LayoutRenderer layoutRenderer ) { _layoutRenderer = layoutRenderer ; }
        #region Overrides of Layout
        protected override string GetFormattedMessage ( LogEventInfo logEvent )
        {
            return _layoutRenderer.Render ( logEvent ) ;
        }
        #endregion
    }
}