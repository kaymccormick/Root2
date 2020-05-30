namespace RibbonLib.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class OnSelectedChangedEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isSelected"></param>
        public OnSelectedChangedEventArgs(bool isSelected)
        {
            IsSelected = isSelected;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSelected { get; set; }
    }
}