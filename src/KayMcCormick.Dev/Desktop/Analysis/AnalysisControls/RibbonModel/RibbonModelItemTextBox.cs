using System.ComponentModel;

namespace AnalysisControls.RibbonModel
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonModelItemTextBox : RibbonModelItem, INotifyPropertyChanged
    {
        private string _value;

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(null)]
        public string Value
        {
            get { return _value; }
            set
            {
                if (value == _value) return;
                _value = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public override ControlKind Kind
        {
            get { return ControlKind.RibbonTextBox; }
        }
    }
}