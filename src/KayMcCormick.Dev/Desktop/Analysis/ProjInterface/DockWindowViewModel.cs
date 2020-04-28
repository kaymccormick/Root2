using System ;
using System.Collections.Generic ;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization ;
using AnalysisAppLib;
using Autofac ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Lib.Wpf ;
using NLog ;
using Logger = NLog.Logger ;

namespace ProjInterface
{
    /// <summary>
    /// 
    /// </summary>
    [ UsedImplicitly ]
    public sealed class DockWindowViewModel : IViewModel, INotifyPropertyChanged
    {
        [NotNull] private readonly ILifetimeScope _scope;

        // ReSharper disable once UnusedMember.Local
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        private Dictionary<Category, Info1> _dict;


        public DockWindowViewModel ([ NotNull ] ILifetimeScope scope, Dictionary<Category, Info1> dict )
        {
            _scope = scope;
            var views = scope.Resolve < IEnumerable < Lazy<IControlView> > > ( ) ;
            foreach ( var controlView in views )
            {
                DebugUtils.WriteLine(controlView.ToString());

            }
            Dict = dict;

        }
        public ObservableCollection<TabInfo> Tabs { get; } = new ObservableCollection<TabInfo>();

        public Dictionary<Category, Info1> Dict
        {
            get => _dict;
            set
            {
                if (Equals(value, _dict)) return;
                _dict = value;
                OnPropertyChanged();
                Tabs.Clear();
                foreach (var l1 in _dict.Values)
                {
                    var tabinfo = new TabInfo { Category = l1.Category };
                    foreach (var l2 in l1.Infos.Values)
                    {
                        var groupinfo = new GroupInfo { Group = l2.Group, Category = l1.Category };
                        foreach (var commandInfo in l2.Infos)
                        {
                            groupinfo.Commands.Add(commandInfo);
                        }

                        //  
                        //scope.Resolve<Func<TabInfo, IEnumerable<GroupInfo>>>()

                        tabinfo.Groups.Add(groupinfo);
                    }

                    var groups = _scope.Resolve<IEnumerable<GroupInfo>>(new TypedParameter(typeof(TabInfo), tabinfo));
                    foreach (var groupInfo in groups)
                    {
                        tabinfo.Groups.Add(groupInfo);
                    }
                    Tabs.Add(tabinfo);
                }
            }
        }

        #region Implementation of ISerializable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class GroupInfo : INotifyPropertyChanged
    {
        private string _group;
        private Category _category;
        private CommandObservableCollection _commands = new CommandObservableCollection();

        public string Group
        {
            get => _group;
            set
            {
                if (value == _group) return;
                _group = value;
                OnPropertyChanged();
            }
        }

        public Category Category
        {
            get => _category;
            set
            {
                if (value == _category) return;
                _category = value;
                OnPropertyChanged();
            }
        }

        public CommandObservableCollection Commands
        {
            get => _commands;
            internal set
            {
                if (Equals(value, _commands)) return;
                _commands = value;
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

    public class GroupInfoImpl : GroupInfo
    {
        
    }

    public class TabInfo :INotifyPropertyChanged
    {
        private Category _category;
        private ObservableCollection<GroupInfo> _groups = new ObservableCollection<GroupInfo>();

        public TabInfo()
        {
        }

        public Category Category
        {
            get => _category;
            set
            {
                if (value == _category) return;
                _category = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<GroupInfo> Groups
        {
            get => _groups;
            internal set
            {
                if (Equals(value, _groups)) return;
                _groups = value;
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