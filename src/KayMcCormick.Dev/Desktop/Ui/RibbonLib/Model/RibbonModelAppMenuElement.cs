using System.ComponentModel;

namespace RibbonLib.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonModelAppMenuElement
    {
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(null)]
        public string Header { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(null)]
        public string KeyTip { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(null)]
        public object ImageSource { get; set; }

        public RibbonModelAppSplitMenuItem CreateSplitMenuItem(string header)
        {
            var r = new RibbonModelAppSplitMenuItem {Header = header};
            Items.Add(r);
            return r;
        }

        public RibbonModelAppMenuItem CreateAppMenuItem(string Header)
        {
            var r = new RibbonModelAppMenuItem {Header = Header};
            Items.Add(r);
            return r;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RibbonModelAppMenuElementCollection  Items { get; } =
            new RibbonModelAppMenuElementCollection();
    }
}