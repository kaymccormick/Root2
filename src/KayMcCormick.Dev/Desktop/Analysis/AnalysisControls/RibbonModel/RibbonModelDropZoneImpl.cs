using System;
using System.Windows;
using NLog;

namespace AnalysisControls.RibbonModel
{
    class RibbonModelDropZoneImpl : RibbonModelDropZone
    {
        public override DragDropEffects OnDrop(IDataObject eData)
        {
            foreach (var format in eData.GetFormats())
            {
                var t = Type.GetType(format);
                // if (t != null)
                // {
                // if (typeof(UIElement).IsAssignableFrom(t))
                // {
                var o = eData.GetData(format);
                if (o is DocModel doc)
                {
                    var logger = LogManager.GetCurrentClassLogger();
                    logger.Info("dropped document " + doc.Title);
                    if (!(doc.Content is UIElement uie))
                    {
                        logger.Info("Content is not ui element "+ doc.Content);
                    }
                }
                        
                // }
                // }
            }
            if (eData.GetDataPresent(typeof(UIElement)))
            {
                var info = eData.GetData(typeof(UIElement));
            }

            return DragDropEffects.None;
        }
    }
}