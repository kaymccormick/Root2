namespace AnalysisControls.RibbonModel
{
    /// <summary>
    /// &lt;HierarchicalDataTemplate DataType="{x:Type ribbonModel:RibbonModelItemButton}"&gt;
    /// &lt;RibbonButton Label="{Binding Label}" Command="{Binding Command}" CommandParameter="{Binding CommandParameter}" CommandTarget="{Binding CommandTarget}"&gt;&lt;/RibbonButton&gt;
    /// &lt;/HierarchicalDataTemplate&gt;
    /// </summary>
    public class RibbonModelItemButton : RibbonModelItem
    {
        /// <inheritdoc />
        public override ControlKind Kind => ControlKind.RibbonButton;

        /// <inheritdoc />
        public override string ToString()
        {
            return $"ModelItem[ {Kind}; Command={Command}; Label={Label} ]";
        }
    }
}