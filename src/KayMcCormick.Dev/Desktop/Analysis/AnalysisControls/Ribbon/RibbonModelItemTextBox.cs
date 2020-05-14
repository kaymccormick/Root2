namespace AnalysisControls.RibbonModel
{
    public class RibbonModelItemTextBox : RibbonModelItem
    {
        public string Value
        {
            get;
            set;
        }

        public override ControlKind Kind => ControlKind.RibbonTextBox;
    }
}