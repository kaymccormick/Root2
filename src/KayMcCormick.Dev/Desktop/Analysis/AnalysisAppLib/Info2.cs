using Autofac.Features.Metadata;
using KayMcCormick.Dev;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace AnalysisAppLib
{
    public class Info2
    {
        string group;
        Category category;
        public string Group { get => group; set => group = value; }
        public List<CommandInfo> Infos { get; set; } = new List<CommandInfo>();
        public Category Category { get => category; set => category = value; }
    }

    /// <summary>
    /// 
    /// </summary>
    [TypeConverter(typeof(CommandInfoTypeConverter))]
    public class CommandInfo : INotifyPropertyChanged
    {
        Meta<Lazy<IBaseLibCommand>> command;
        private string _title;
        private IBaseLibCommand _theCommand;

        public string Title
        {
            get => _title;
            set
            {
                if (value == _title) return;
                _title = value;
                OnPropertyChanged();
            }
        }

        public IBaseLibCommand TheCommand
        {
            get
            {
                if (command.Value.IsValueCreated)
                {
                    return command.Value.Value;
                }

                try
                {
                    _theCommand = command.Value.Value;
                    return _theCommand;
                }
                catch (Exception ex)
                {
                    return null;
                }

                return null;
            }
        }
        public Meta<Lazy<IBaseLibCommand>> Command
        {
            get => command; set
            {
                command = value;
                OnPropertyChanged();
                if(command.Metadata.TryGetValue("Title", out var title))
                {
                    Title = (string)title;
                }

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CommandInfoTypeConverter :TypeConverter
    {
        public CommandInfoTypeConverter()
        {
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return base.CanConvertTo(context, destinationType);
        }
    }
}