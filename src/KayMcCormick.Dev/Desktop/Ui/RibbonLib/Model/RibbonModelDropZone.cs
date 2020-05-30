using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace RibbonLib.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonModelDropZone : RibbonModelItem
    {
        public override ControlKind Kind => ControlKind.DropZone;

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(null)]
        public Brush Fill { get; set; }

        public virtual DragDropEffects OnDrop(DragEventArgs eData, IInputElement inputElement)
        {
            var command = Command;
            if (command != null)
            {
                RibbonDebugUtils.WriteLine(command.GetType().ToString());
                if (command is RoutedCommand cmd)
                {
                    cmd.Execute(eData.Data, inputElement);
                }else
                command.Execute(eData.Data);

                eData.Handled = true;
                return DragDropEffects.Copy;
            }
            return DragDropEffects.None;
        }

        public void OnDragOver(DragEventArgs dragEventArgs, IInputElement inputElement)
        {
            dragEventArgs.Effects = DragDropEffects.Copy;
            dragEventArgs.Handled = true;
        }
    }
}