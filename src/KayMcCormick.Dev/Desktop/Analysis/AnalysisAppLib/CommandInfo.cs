using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Autofac.Features.Metadata;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Command;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public class CommandInfo : INotifyPropertyChanged
    {
        Meta<Lazy<IBaseLibCommand>> _command;
        private string _title;
        private IBaseLibCommand _theCommand;

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, object> Metadata => _command.Metadata;
        /// <summary>
        /// 
        /// </summary>
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
                if (_command.Value.IsValueCreated)
                {
                    return _command.Value.Value;
                }

                try
                {
                    _theCommand = _command.Value.Value;
                    return _theCommand;
                }
                catch (Exception ex)
                {
                    DebugUtils.WriteLine(ex.ToString());
                    return null;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Meta<Lazy<IBaseLibCommand>> Command
        {
            get => _command; set
            {
                _command = value;
                OnPropertyChanged();
                _theCommand = _command.Value.Value;
                if (_theCommand is IDisplayable cmd)
                {
                    Title = cmd.DisplayName;
                } else
                if(_command.Metadata.TryGetValue("Title", out var title))
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return
                $"{Title} - {TheCommand.GetType().FullName} - {String.Join("; ", Metadata.Select(kv => $"{kv.Key}={kv.Value}"))}";
        }
    }
}