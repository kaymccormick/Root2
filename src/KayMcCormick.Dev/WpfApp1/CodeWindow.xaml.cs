using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using JetBrains.Annotations;
using KayMcCormick.Dev;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.MSBuild;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for CodeWindow.xaml
    /// </summary>
    public partial class CodeWindow : Window, INotifyPropertyChanged
    {
        private string _sourceText = ""; //"public class Test {\r\nint foo;\r\n}\r\n";

        public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register(
            "Document", typeof(Document), typeof(CodeWindow),
            new PropertyMetadata(default(Document), OnDocumentChanged));

        public Document Document
        {
            get { return (Document) GetValue(DocumentProperty); }
            set { SetValue(DocumentProperty, value); }
        }

        private static void OnDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CodeWindow) d).OnDocumentChanged((Document) e.OldValue, (Document) e.NewValue);
        }

        public static readonly DependencyProperty ProjectProperty = DependencyProperty.Register(
            "Project", typeof(Project), typeof(CodeWindow), new PropertyMetadata(default(Project), OnProjectChanged));

        private Task _task;
        private MefHostServices _host;

        public Project Project
        {
            get { return (Project) GetValue(ProjectProperty); }
            set { SetValue(ProjectProperty, value); }
        }

        private static void OnProjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CodeWindow) d).OnProjectChanged((Project) e.OldValue, (Project) e.NewValue);
        }


        protected virtual void OnProjectChanged(Project oldValue, Project newValue)
        {
            if (newValue != null)
            {
                Ellipse.Fill = Brushes.LawnGreen;
                
            }
        }


        protected virtual void OnDocumentChanged(Document oldValue, Document newValue)
        {
        }

        public CodeWindow()
        {
            InitializeComponent();
            _host = MefHostServices.Create(MefHostServices.DefaultAssemblies);
            var f = ((App) Application.Current).LoadFilename;
            if (f != null && f.EndsWith(".csproj"))
            {
                
                _task = LoadProjectAsync(f);
                return;
            }

            var w = new AdhocWorkspace(_host);
            w.AddSolution(SolutionInfo.Create(SolutionId.CreateNewId(), VersionStamp.Create()));
            var projectInfo = ProjectInfo.Create(ProjectId.CreateNewId(), VersionStamp.Create(),
                "Code Project", "code", LanguageNames.CSharp);
            var w2 = w.CurrentSolution.AddProject(projectInfo);
            w.TryApplyChanges(w2);

            DocumentInfo documentInfo = null;
            if (f != null)
                documentInfo = DocumentInfo.Create(DocumentId.CreateNewId(projectInfo.Id), "Default",
                    null, SourceCodeKind.Regular, new FileTextLoader(f, Encoding.UTF8), f);
            else
                documentInfo = DocumentInfo.Create(DocumentId.CreateNewId(projectInfo.Id), "Default",
                    null, SourceCodeKind.Regular);

            w2 = w.CurrentSolution.AddDocument(documentInfo);
            w.TryApplyChanges(w2);
          
            Project = w.CurrentSolution.GetProject(projectInfo.Id);
            Document = w.CurrentSolution.GetDocument(documentInfo.Id);
            
            Code.Focus();
            var ks=w.Services.GetLanguageServices(LanguageNames.CSharp);
            
            Action<string> d = s => DebugUtils.WriteLine(s);
            Loaded += (sender, args) =>
            {
                return;
                var c = Code.CodeControl.CodeControl;
                var lines = new string[] {"/* foo */", "public "};
                var first = true;
                foreach (var line in lines)
                {
                    if (!first)
                        c.DoInput("\r\n");
                    first = false;
                    foreach (var ch in line)
                    {
                        DebugUtils.WriteLine("Input is char '" + ch + "'");
                        c.DoInput(ch.ToString());
                        if (c.InsertionLine != null) d(c.InsertionLine.Length.ToString());
                    }
                }

                c.DoInput("c");
            };
        }

        private async Task LoadProjectAsync(string s)
        {
            MSBuildLocator.RegisterDefaults();
            var ww = MSBuildWorkspace.Create();
            var project = await ww.OpenProjectAsync(s, new Progress1()).ConfigureAwait(true);
            Project = project;
        }

        public string SourceText
        {
            get { return _sourceText; }
            set
            {
                if (value == _sourceText) return;
                _sourceText = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Combo_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            combo.SelectedIndex = 0;
        }
    }

    internal class Progress1 : IProgress<ProjectLoadProgress>
    {
        /// <inheritdoc />
        public void Report(ProjectLoadProgress value)
        {
            DebugUtils.WriteLine(value.ElapsedTime.ToString());
        }
    }
}