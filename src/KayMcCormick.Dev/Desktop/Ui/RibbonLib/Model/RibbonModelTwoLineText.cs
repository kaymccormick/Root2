using System.ComponentModel;
using System.Windows.Media;

namespace RibbonLib.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonModelTwoLineText : RibbonModelItem
    {
        private Geometry _pathData;
        private Brush _pathFill;
        private Brush _pathStroke = Brushes.Black;
        private string _text;
        private bool _hasTwoLines;

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(null)]
        public Geometry PathData
        {
            get { return _pathData; }
            set
            {
                if (Equals(value, _pathData)) return;
                _pathData = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(null)]
        public Brush PathFill
        {
            get { return _pathFill; }
            set
            {
                if (Equals(value, _pathFill)) return;
                _pathFill = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(null)]
        public Brush PathStroke
        {
            get { return _pathStroke; }
            set
            {
                if (Equals(value, _pathStroke)) return;
                _pathStroke = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(null)]
        public string Text
        {
            get { return _text; }
            set
            {
                if (value == _text) return;
                _text = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(false)]
        public bool HasTwoLines
        {
            get { return _hasTwoLines; }
            set
            {
                if (value == _hasTwoLines) return;
                _hasTwoLines = value;
                OnPropertyChanged();
            }
        }

        public override ControlKind Kind => ControlKind.RibbonTwoLine;
    }
}