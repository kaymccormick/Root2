using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnalysisControls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:AnalysisControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:AnalysisControls;assembly=AnalysisControls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:DocumentView/>
    ///
    /// </summary>
    public class DocumentView : Control
    {
        public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register("Document", typeof(DocModel), typeof(DocumentView), new PropertyMetadata(default(DocModel)));

        static DocumentView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DocumentView), new FrameworkPropertyMetadata(typeof(DocumentView)));

        }

        public override void OnApplyTemplate()
        {
            var contentPresenter = (ContentPresenter) GetTemplateChild("ContentPresenter");
            var myAdornerLayer = AdornerLayer.GetAdornerLayer(contentPresenter);
            if (myAdornerLayer != null) myAdornerLayer.Add(new DragItemAdorner((UIElement)contentPresenter, Document));
            contentPresenter.Loaded += (sender, args) =>
            {
                if(VisualTreeHelper.GetChildrenCount(contentPresenter) >= 1)
                VisualTreeHelper.GetChild(contentPresenter, 0);

            };

        }

        /// <summary>
        /// 
        /// </summary>
        public DocModel Document
        {
            get { return (DocModel) GetValue(DocumentProperty); }
            set { SetValue(DocumentProperty, value); }
        }
    }
}
