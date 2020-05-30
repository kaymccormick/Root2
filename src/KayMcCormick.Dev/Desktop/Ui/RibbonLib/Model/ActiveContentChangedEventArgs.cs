using System;

namespace RibbonLib.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class ActiveContentChangedEventArgs : EventArgs
    {
        /// <inheritdoc />
        public ActiveContentChangedEventArgs(object activeContent)
        {
            ActiveContent = activeContent;
        }

        /// <summary>
        /// 
        /// </summary>
        public object ActiveContent { get; set; }

    }
}