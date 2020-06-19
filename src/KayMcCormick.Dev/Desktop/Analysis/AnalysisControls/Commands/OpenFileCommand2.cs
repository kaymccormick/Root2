
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using AnalysisAppLib;
using AnalysisAppLib.Project;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Command;
using KayMcCormick.Lib.Wpf.Command;
using Microsoft.Win32;

namespace AnalysisControls.Commands
{
    /// <summary>
    /// Open file command
    /// </summary>
    [UsedImplicitly]
    public class OpenFileCommand2 : AppCommand
    {
        private readonly AppCommand _command;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="analyzeCommand"></param>
        public OpenFileCommand2(AppCommand command) : base("Open File")
        {
            _command = command;
        }

        /// <inheritdoc />
        public override object Argument { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename2"></param>
        /// <returns></returns>
        public override async Task<IAppCommandResult> ExecuteAsync(object filename2)
        {
            var filters = new List<Filter>
            {
                new Filter {Extension = ".sln", Description = "Solution Files"},
                new Filter {Extension = ".xml", Description = "XML files"}, new Filter
                {
                    Extension = ".cs", Description = "CSharp source files",
                    // ReSharper disable once UnusedAnonymousMethodSignature
#pragma warning disable 1998
                    Handler = async delegate(string filename1)
                    {
#pragma warning restore 1998
                    }
                }
            };
            var dlg = new OpenFileDialog
            {
                DefaultExt = ".cs",
                Filter = string.Join(
                    "|"
                    , filters.Select(
                        f => $"{f.Description} (*{f.Extension})|*{f.Extension}"
                    )
                )
            };

            var result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result != true)
            {
                return AppCommandResult.Cancelled;
            }

            

            // Open document
            var filename = dlg.FileName;
            var resul2 = await _command.ExecuteAsync(filename).ConfigureAwait(false);

            return resul2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        public override void OnFault(AggregateException exception)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public override object LargeImageSourceKey { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
