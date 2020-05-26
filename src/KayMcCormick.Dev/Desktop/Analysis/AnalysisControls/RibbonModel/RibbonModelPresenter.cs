namespace AnalysisControls.RibbonModel
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonModelPresenter : RibbonModelItem
    {
        private object _content;

        public object Content
        {
            get { return _content; }
            set
            {
                if (Equals(value, _content)) return;
                _content = value;
                OnPropertyChanged();
            }
        }

        public override ControlKind Kind
        {
            get { return ControlKind.RibbonContentPresenter; }
        }
    }
}