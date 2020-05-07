using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Autofac.Features.Metadata;
using JetBrains.Annotations;
using KayMcCormick.Dev;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    [TypeConverter(typeof(CommandInfoTypeConverter))]
    public class CommandInfo : INotifyPropertyChanged
    {
        Meta<Lazy<IBaseLibCommand>> command;
        private string _title;
        private IBaseLibCommand _theCommand;

        public IDictionary<string, object?> Metadata => command.Metadata;
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

        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
        public Meta<Lazy<IBaseLibCommand>> Command
        {
            get => command; set
            {
                command = value;
                OnPropertyChanged();
                _theCommand = command.Value.Value;
                if (_theCommand is IDisplayable cmd)
                {
                    Title = cmd.DisplayName;
                } else
                if(command.Metadata.TryGetValue("Title", out var title))
                {
                    Title = (string)title;
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return
                $"{Title} - {TheCommand.GetType().FullName} - {String.Join("; ", Metadata.Select(kv => $"{kv.Key}={kv.Value}"))}";
        }
    }
}