using System.Windows.Input;
using KayMcCormick.Lib.Wpf.Command;

namespace AnalysisControls.RibbonModel
{
    public interface IRibbonModelItem
    {
        /// <summary>
        /// 
        /// </summary>
// ReSharper disable once MemberCanBeProtected.Global
        ControlKind Kind { get; }

        /// <summary>
        /// 
        /// </summary>
        object Label { get; set; }

        /// <summary>
        /// 
        /// </summary>
        IAppCommand AppCommand { get; set; }

        /// <summary>
        /// s
        /// </summary>
        ICommand Command { get; set; }

        /// <summary>
        /// 
        /// </summary>
        object CommandTarget { get; set; }

        /// <summary>
        /// 
        /// </summary>
        object CommandParameter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        object LargeImageSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        object SmallImageSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        double? MaxWidth { get; set; }

        /// <summary>
        /// 
        /// </summary>
        double? MaxHeight { get; set; }

        /// <summary>
        /// 
        /// </summary>
        double? MinWidth { get; set; }

        /// <summary>
        /// 
        /// </summary>
        double? MinHeight { get; set; }

        /// <summary>
        /// 
        /// </summary>
        double? Width { get; set; }

        /// <summary>
        /// 
        /// </summary>
        double? Height { get; set; }

        /// <summary>
        /// 
        /// </summary>
// ReSharper disable once UnusedMember.Global
        string StringLabel { get; }

        object TemplateKey { get; set; }
    }
}