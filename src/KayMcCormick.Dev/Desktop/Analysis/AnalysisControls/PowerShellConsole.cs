using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PowerShellShared;
using Terminal1;
using TextEntryCompleteArgs = Terminal1.TextEntryCompleteArgs;

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
    ///     <MyNamespace:PowerShellConsole/>
    ///
    /// </summary>
    public class PowerShellConsole : Control
    {
        private WpfTerminalControl _terminal;
        private WpfInputLine _input;
        private TextBlock _mainstatus;
        private WrappedPowerShell _shell;
        private ProgressBar _progressBar
            ;

        static PowerShellConsole()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PowerShellConsole), new FrameworkPropertyMetadata(typeof(PowerShellConsole)));
        }

        /// <inheritdoc />
        public PowerShellConsole()
        {
            
        }

        public WrappedPowerShell Shell
        {
            get { return _shell; }
            set { _shell = value; }
        }

        /// <inheritdoc />
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _terminal = (WpfTerminalControl)GetTemplateChild("Terminal");
            Shell = (WrappedPowerShell) GetTemplateChild("Shell");
            _input = (WpfInputLine)GetTemplateChild("Input");
            _mainstatus = (TextBlock) GetTemplateChild("MainStatus");
            _progressBar = (ProgressBar) GetTemplateChild("ProgressBar");

            if (_input != null)
            {

                Shell.Terminal = _terminal;
                _input.Focus();
                _terminal.ProgressEvent += (sender, args) =>
                {
                    if (_progressBar.Visibility != Visibility.Visible)
                    {
                        _progressBar.Visibility = Visibility.Visible;
                    }

                    _progressBar.Value = args.Record.PercentComplete;
                    if (args.Record.RecordType == 1)
                    {
                        _mainstatus.Text = "";
                    }
                    else
                    {
                        _mainstatus.Text = args.Record.StatusDescription;
                    }
                };
                _input.TextEntryComplete += OnInput_OnTextEntryComplete;
            }
        }

        private async void OnInput_OnTextEntryComplete(object sender, Terminal1.TextEntryCompleteArgs args)
        {
            Debug.WriteLine(args.Text);
            await Shell.Execute(args.Text);
            Debug.WriteLine("Back from execute");
        }
    }
}
