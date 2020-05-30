using System;

namespace RibbonLib.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class ContextualTabGroupActivatedHandlerArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public object Document{ get; set; }

        /// <inheritdoc />
        public ContextualTabGroupActivatedHandlerArgs(object document)
        {
            Document = document;
        }
    }
}