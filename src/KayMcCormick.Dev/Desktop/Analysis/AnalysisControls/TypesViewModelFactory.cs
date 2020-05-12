using System;
using System.Windows.Markup;
using AnalysisControls.ViewModel;
using KayMcCormick.Dev;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public static class TypesViewModelFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static TypesViewModel CreateModel()
        {
            using (var stream = typeof(AnalysisControlsModule).Assembly
                .GetManifestResourceStream(
                    "AnalysisControls.TypesViewModel.xaml"
                ))
            {
                if (stream == null)
                {
                    DebugUtils.WriteLine("no stream");
                    return new TypesViewModel(
                    );
                }

                try
                {
                    var v = (TypesViewModel) XamlReader
                        .Load(stream);
                    stream.Close();
                    return v;
                }
                catch (Exception)
                {
                    return new TypesViewModel();
                }
            }
        }
    }
}