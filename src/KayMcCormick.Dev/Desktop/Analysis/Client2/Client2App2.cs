using System;
using System.Collections;
using System.Windows;
using System.Windows.Input;
using KayMcCormick.Lib.Wpf;

namespace Client2
{
    internal class Client2App2 : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            bool select = false;
            var keyb = InputManager.Current.PrimaryKeyboardDevice;
            if (keyb.Modifiers == ModifierKeys.Control)
            {
                select = true;
            } else if (keyb.Modifiers == ModifierKeys.Shift)
            {
                TetExit = true;
                StartupCommand = new TestAndExitCommand();
            }

            if (keyb.IsKeyToggled(Key.LeftCtrl))
            {
                select = true;
            }
            if(select)
            {
                ShutdownMode = ShutdownMode.OnExplicitShutdown;
                var model = new Select2();
                AppHelpers.SelectAppAction(model, (model1) =>
                {

                    var item = model1.SelectedItem;
                    if (item is Type t)
                    {
                        Window w = (Window) Activator.CreateInstance(t);
                        RunWindow(w);
                        return;
                    }
                });
            }
            else
            {
                Client2Window1 main = new Client2Window1();
                RunWindow(main);
            }
        }

        public ICommand StartupCommand { get; set; }

        public bool TetExit { get; set; }

        private void RunWindow(Window window)
        {
            window.Loaded += (sender, args) =>
            {
                StartupCommand?.Execute(null);
            };
            window.Show();
        }
    }

    internal class SelectAppModel : ISelectAppModel
    {
        
        
        public void SetSelectedApp(object item)
        {
            SelectedItem = item;
        }

        public IEnumerable Items { get; set; }
        public object SelectedItem { get; set; }
        public TimeSpan Timeout { get; set; }
    }

    internal interface ISelectAppModel
    {
        void SetSelectedApp(object item);
        IEnumerable Items { get;  }
        object SelectedItem { get; set; }
        TimeSpan Timeout { get; }
    }

    class Select2 : ISelectAppModel
    {
        public void SetSelectedApp(object item)
        {
            SelectedItem = item;
        }

        public IEnumerable Items { get; }=new object[] 
            {typeof(Client2Window1), typeof(TestRibbonWindow), WpfAppCommands.QuitApplication};

        public object SelectedItem { get; set; }
        public TimeSpan Timeout { get; }

        private object _selected;
    }
}