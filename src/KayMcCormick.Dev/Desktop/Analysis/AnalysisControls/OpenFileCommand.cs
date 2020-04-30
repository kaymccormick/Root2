using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using AnalysisAppLib;
using AnalysisAppLib.Project;
using FindLogUsages;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Command;
using KayMcCormick.Lib.Wpf.Command;
using Microsoft.Win32;

namespace AnalysisControls
{
    public class OpenFileCommand : AppCommand
    {
        private IAnalyzeCommand analyzeCommand;

        public OpenFileCommand(IAnalyzeCommand analyzeCommand) : base("Open File")
        {
            this.analyzeCommand = analyzeCommand;
        }

        public override object Argument { get; set; }

        public override async Task<IAppCommandResult> ExecuteAsync()
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
            if (Path.GetExtension(filename).ToLowerInvariant() == ".sln")
            {


                var node = new ProjectBrowserNode
                {
                    Name = "Loaded solution",
                    SolutionPath = filename
                };
                DebugUtils.WriteLine("await command");
                await analyzeCommand.AnalyzeCommandAsync(
                        node
                        , new ActionBlock<RejectedItem>(
                            x => Debug
                                .WriteLine(
                                    x.Statement
                                        .ToString()
                                )
                        )
                    )
                    .ConfigureAwait(false);
                DebugUtils.WriteLine("herecommand");
                return AppCommandResult.Success;
            }

            return AppCommandResult.Failed;
        }

        public override void OnFault(AggregateException exception)
        {
            
        }

        public override object LargeImageSourceKey { get; set; }


        public override bool CanExecute(object parameter)
        {
            return true;
        }

        
        
    }
}