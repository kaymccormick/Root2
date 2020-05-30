namespace RibbonLib.Model
{
    public interface IRibbonModelGroup : IRibbonModelItem
    {
        /// <summary>
        /// 
        /// </summary>
        RibbonModelGroupItemCollection Items { get; }

        bool IsDropDownOpen { get; set; }
    }
}