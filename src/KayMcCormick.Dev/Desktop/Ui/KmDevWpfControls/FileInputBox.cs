using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;
using Path = System.IO.Path;

namespace KmDevWpfControls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:KmDevWpfControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:KmDevWpfControls;assembly=KmDevWpfControls"
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
    ///     <MyNamespace:FileInputBox/>
    ///
    /// </summary>
    public class FileInputBox : Control
    {
        private TextBox _te;

        public static readonly DependencyProperty FileSystemInfoProperty = DependencyProperty.Register(
            "FileSystemInfo", typeof(FileSystemInfo), typeof(FileInputBox), new PropertyMetadata(default(FileSystemInfo)));

        public FileSystemInfo FileSystemInfo
        {
            get { return (FileSystemInfo) GetValue(FileSystemInfoProperty); }
            set { SetValue(FileSystemInfoProperty, value); }
        }

        public static readonly DependencyProperty IsFileSelectedProperty = DependencyProperty.Register(
            "IsFileSelected", typeof(bool), typeof(FileInputBox), new PropertyMetadata(default(bool)));

        public bool IsFileSelected
        {
            get { return (bool) GetValue(IsFileSelectedProperty); }
            set { SetValue(IsFileSelectedProperty, value); }
        }
        static FileInputBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FileInputBox),
                new FrameworkPropertyMetadata(typeof(FileInputBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, Executed));
            _te = (TextBox) GetTemplateChild("TextBox");
        }

        private void Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            var r = openFileDialog.ShowDialog().GetValueOrDefault();

            if (!r)
                return;
            var fileName = openFileDialog.FileName;
            _te.Text = fileName;

            IsFileSelected = true;
            var fileAttributes = File.GetAttributes(fileName);
            FileSystemInfo = ((fileAttributes & FileAttributes.Directory) == FileAttributes.Directory) ? (FileSystemInfo) new DirectoryInfo(fileName) : new FileInfo(fileName);
            
        } 

    }

    public class EnumFlagsSelector :Control
    {
        
        static EnumFlagsSelector()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EnumFlagsSelector),
                new FrameworkPropertyMetadata(typeof(EnumFlagsSelector)));
        }

    }
}
