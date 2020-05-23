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
    /// <summary>
    /// 
    /// </summary>
    public partial class UserControl1 : UserControl
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="drgevent"></param>
        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            base.OnDragDrop(drgevent);
            propertyGrid1.SelectedObject= drgevent.Data.GetData(drgevent.Data.GetFormats()[0]);
        }

        /// <summary>
        /// 
        /// </summary>
        public UserControl1()
        {
            InitializeComponent();
            
        }

        /// <inheritdoc />
        public override bool AllowDrop => true;
    }
}
