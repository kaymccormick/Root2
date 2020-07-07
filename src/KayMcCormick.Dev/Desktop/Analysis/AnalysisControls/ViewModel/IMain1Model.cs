using System.Collections.ObjectModel;
using System.Text.Json;

namespace AnalysisControls.ViewModel
{
    public interface IMain1Model
    {
        string Catchphrase { get; set; }
        object ActiveContent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        ///
        /// 
        /// <summary>
        /// 
        /// </summary>
        ObservableCollection<object> ContextualTabGroups { get; set; }

        AppSettingsViewModel AppSettingsViewModel { get; set; }
        IUserSettingsDbContext UserSettingsDbContext { get; }
        bool AllDocs { get; set; }

        /// </summary>
        CurrentOperation CurrentOperation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        IClientModel ClientViewModel { get; set; }

        JsonSerializerOptions JsonSerializerOptions { get; set; }
        object InstanceObjectId { get; set; }
    }
}