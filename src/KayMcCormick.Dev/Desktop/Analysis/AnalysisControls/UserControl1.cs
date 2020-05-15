using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnalysisControls
{
    public partial class UserControl1 : UserControl
    {
        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            base.OnDragDrop(drgevent);
            propertyGrid1.SelectedObject= drgevent.Data.GetData(drgevent.Data.GetFormats()[0]);
        }

        public UserControl1()
        {
            InitializeComponent();
            
        }

        public override bool AllowDrop => true;
    }
}
