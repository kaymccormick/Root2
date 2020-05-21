using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AvalonDock.Layout;
using ControlsDemo.Annotations;
using Microsoft.Win32;

namespace ControlsDemo
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            System.Drawing.Bitmap b = new Bitmap(@"C:\temp\example.bmp");
            InitializeComponent();
            Anchorables.Add("test");
            Debug.WriteLine(DM.LayoutUpdateStrategy?.ToString());
        }


        public ObservableCollection<object> DockItems { get; set; } = new ObservableCollection<object>();

        public ObservableCollection<object> Anchorables { get; set; } = new ObservableCollection<object>();

        public Window2ViewModel ViewModel { get; set; } = new Window2ViewModel();

        private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            var result = open.ShowDialog();
            if ((bool) result)
            {
                var ext = System.IO.Path.GetExtension(open.FileName);
                if (ext == ".dll")
                {
                    try
                    {
                        var assembly = Assembly.LoadFile(open.FileName);
                    }
                    catch (Exception ex)
                    {
                        Debug.Write(ex.ToString());
                    }
                }

            }
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            HashSet<Type> editorTypes = new HashSet<Type>();
            var a = ViewModel.Assemblies.ToList();
            foreach (var viewModelAssembly in a)
            {
                Debug.WriteLine(a);
                try
                {
                    foreach (var exportedType in viewModelAssembly.ExportedTypes)
                    {
//                        Debug.WriteLine(exportedType);
                        try
                        {
                            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(exportedType))
                            {
                                {
                                    var e3 = property.GetEditor(typeof(UITypeEditor));
                                    if(e3 != null)
                                    editorTypes.Add(e3.GetType());
                                    if (e3 != null && e3.GetType() != typeof(CollectionEditor))
                                    {
                                        
                                        var paint = ((UITypeEditor) e3).GetPaintValueSupported();
                                        var kind = ((UITypeEditor) e3).GetEditStyle();
                                        Debug.WriteLine(
                                            $"{exportedType?.FullName} {paint} {kind} {property.Name} {property.Category} {e3}");
                                    }
                                }
                                {
                                    var e3 = property.GetEditor(typeof(InstanceCreationEditor));
                                    if (e3 != null)
                                        editorTypes.Add(e3.GetType());
                                    if (e3 != null && e3.GetType() != typeof(CollectionEditor))
                                    {
                                        // var paint = ((UITypeEditor)e3).GetPaintValueSupported();
                                        // var kind = ((UITypeEditor)e3).GetEditStyle();
                                        Debug.WriteLine($"!!!! {exportedType?.FullName}  {property.Name} {property.Category} {e3}");
                                    }

                                }

                            }
                            // var e2 = TypeDescriptor.GetEditor(exportedType, typeof(UITypeEditor));
                            // if (e2 != null)
                            // {
                                // Debug.WriteLine($"{exportedType} {e2}");
                            // }

                            var e1 = TypeDescriptor.GetEditor(exportedType, typeof(InstanceCreationEditor));
                            if (e1 != null)
                            {

                                    editorTypes.Add(e1.GetType());

                                Debug.WriteLine("!!!" +e1);
                            }
                        } catch{}
                    }
                } catch{}
            }
            foreach (var editorType in editorTypes)
            {
                Debug.WriteLine(editorType);
            }
        }
    }

    public class Window2ViewModel :INotifyPropertyChanged
    {

        public ObservableCollection<Assembly> Assemblies { get; set; }=new ObservableCollection<Assembly>();
        private Type _selectedType;

        public Window2ViewModel()
        {
            AppDomain.CurrentDomain.AssemblyLoad += CurrentDomainOnAssemblyLoad;

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Assemblies.Add(assembly);
            }
        }

        private void CurrentDomainOnAssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            Debug.Write("Loaded " + args.LoadedAssembly.FullName);
            Assemblies.Add(args.LoadedAssembly);
        }
        public Type SelectedType
        {
            get { return _selectedType; }
            set
            {
                if (Equals(value, _selectedType)) return;
                _selectedType = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
