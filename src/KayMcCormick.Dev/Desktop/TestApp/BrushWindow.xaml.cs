using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xaml;
using System.Xml;
using AnalysisControls;
using KayMcCormick.Dev;
using KayMcCormick.Lib.Wpf;
using Microsoft.Win32;
using XamlReader = System.Windows.Markup.XamlReader;
using XamlWriter = System.Windows.Markup.XamlWriter;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for BrushWindow.xaml
    /// </summary>
    public partial class BrushWindow : Window
    {
        private object _newBrush;
        

        public BrushWindow()
        {
            InitializeComponent();
            BrushEditorState = new BrushEditorState();
            // var linearGradientBrush = new LinearGradientBrush(Colors.Pink, Colors.Coral, 30);
            // BrushListView.Items.Add(linearGradientBrush);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            if (e.Key == Key.D)
            {
                editor.DebugMode = !editor.DebugMode;
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            DebugUtils.WriteLine(editor.StartPoint.ToString());
            DebugUtils.WriteLine(editor.LinearGradientBrush.StartPoint.ToString());
        }

        private void ButtonBase_OnClick2(object sender, RoutedEventArgs e)
        {
            text1.Text = editor.StartPoint.ToString();

        }

        public ObservableCollection<LinearGradientBrush> Brushes { get; }=new ObservableCollection<LinearGradientBrush>();

        private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var listView = sender as ListView;
            if (listView == null)
                return;
            if (listView.ItemsSource == null)
            {
                _newBrush = new LinearGradientBrush();
                listView.Items.Add(_newBrush);
            }
            else
            {
                var col = listView?.ItemsSource;
                var view = CollectionViewSource.GetDefaultView(col);
                DebugUtils.WriteLine(view.GetType().ToString());
                if (view is ListCollectionView v2)
                {
                    _newBrush = v2.AddNew();
                }
            }
        }

        private void DeleteBrush(object sender, ExecutedRoutedEventArgs e)
        {
            
        }

        private void CommandBinding_OnExecuted2(object sender, ExecutedRoutedEventArgs e)
        {
            BrushEditorState brushEditorState = new BrushEditorState();
            foreach (var item in BrushListView.Items)
            {
                brushEditorState.BrushCollection.Add(item);
            }

            using (var w = XmlWriter.Create(@"C:\temp\out.xaml", new XmlWriterSettings() {Indent = true}))
            {
                XamlWriter.Save(brushEditorState, w);
            }

        }

        private void OpenExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter == null)
            {
                var cTempOutXaml = @"C:\temp\out.xaml";
                using (var r = XmlReader.Create(cTempOutXaml))
                {
                    var state = XamlReader.Load(r);
                    if (state is BrushEditorState b)
                    {
                        BrushEditorState = b;
                    }
                }

                return;
            }
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "XAML Resource Files|*.xaml";
            dialog.InitialDirectory = Environment.CurrentDirectory;
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                var f = dialog.FileName;
                XamlFuncs.ParseXaml(f);
            }
        }

        public static readonly DependencyProperty BrushEditorStateProperty = DependencyProperty.Register(
            "BrushEditorState", typeof(BrushEditorState), typeof(BrushWindow), new PropertyMetadata(default(BrushEditorState)));

        public BrushEditorState BrushEditorState
        {
            get { return (BrushEditorState) GetValue(BrushEditorStateProperty); }
            set { SetValue(BrushEditorStateProperty, value); }
        }

        private static void HandleResource(BrushEditorState brushEditorState, XamlObjectWriter x)
        {
            object resource = x.Result;

            ResourceDictionary resourceDictionary = resource as ResourceDictionary;
            if (resource is Window w)
            {
                resourceDictionary = w.Resources;
            }
            else if (resource is FrameworkElement element)
            {
                resourceDictionary = element.Resources;
            }

            if (resourceDictionary != null)
            {
                foreach (var resourcesKey in resourceDictionary.Keys)
                {
                    if (resourceDictionary[resourcesKey] is LinearGradientBrush lg)
                    {
                        brushEditorState.BrushCollection.Add(lg);
                    }

                    var col = CollectionViewSource.GetDefaultView(brushEditorState.BrushCollection);
                    col?.Refresh();
                }
            }
        }

        private void PasteCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                e.CanExecute = true;
            }

        }

        private void PasteExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var text = Clipboard.GetText();
            ProcessXaml(text);
        }

        private void ProcessXaml(string text)
        {
            XamlFuncs.ParseXaml(null, text);
        }
    }

    public class BrushEditorState : DependencyObject
    {
        public static readonly DependencyProperty BrushCollectionProperty = DependencyProperty.Register(
            "BrushCollection", typeof(BrushCollection), typeof(BrushEditorState), new PropertyMetadata(default(BrushCollection)));

        public BrushEditorState()
        {
            BrushCollection = new BrushCollection();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BrushCollection BrushCollection
        {
            get { return (BrushCollection) GetValue(BrushCollectionProperty); }
            set { SetValue(BrushCollectionProperty, value); }
        }
    }

    public class BrushEntry
    {
        public string Name { get; set; }
        public LinearGradientBrush LinearGradientBrush { get; set; }

    }

    public class BrushCollection : IList, ICollection, IEnumerable
    {
        private IList _listImplementation = new ObservableCollection<LinearGradientBrush>();
        public BrushCollection()
        {
        }

        public IEnumerator GetEnumerator()
        {
            return _listImplementation.GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            _listImplementation.CopyTo(array, index);
        }

        public int Count
        {
            get { return _listImplementation.Count; }
        }

        public object SyncRoot
        {
            get { return _listImplementation.SyncRoot; }
        }

        public bool IsSynchronized
        {
            get { return _listImplementation.IsSynchronized; }
        }

        public int Add(object value)
        {
            return _listImplementation.Add(value);
        }

        public bool Contains(object value)
        {
            return _listImplementation.Contains(value);
        }

        public void Clear()
        {
            _listImplementation.Clear();
        }

        public int IndexOf(object value)
        {
            return _listImplementation.IndexOf(value);
        }

        public void Insert(int index, object value)
        {
            _listImplementation.Insert(index, value);
        }

        public void Remove(object value)
        {
            _listImplementation.Remove(value);
        }

        public void RemoveAt(int index)
        {
            _listImplementation.RemoveAt(index);
        }

        public object this[int index]
        {
            get { return _listImplementation[index]; }
            set { _listImplementation[index] = value; }
        }

        public bool IsReadOnly
        {
            get { return _listImplementation.IsReadOnly; }
        }

        public bool IsFixedSize
        {
            get { return _listImplementation.IsFixedSize; }
        }
    }

    
}
