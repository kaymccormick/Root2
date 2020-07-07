using System.Collections.ObjectModel;
using System.Text.Json;

namespace AnalysisControls.ViewModel
{
    class Main1ModelProxy : IMain1Model
    {
        private Main1Model _model;

        public Main1ModelProxy(Main1Model main1ModelImplementation)
        {
            _model = main1ModelImplementation;
        }

        /// <inheritdoc />
        public string Catchphrase
        {
            get { return _model.Dispatcher.Invoke(() => _model.Catchphrase); }
            set { _model.Catchphrase = value; }
        }

        /// <inheritdoc />
        public virtual object ActiveContent
        {
            get { return _model.Dispatcher.Invoke(() => _model.ActiveContent); }
            set { _model.ActiveContent = value; }
        }

        /// <inheritdoc />
        public virtual ObservableCollection<object> ContextualTabGroups
        {
            get { return _model.Dispatcher.Invoke(() => _model.ContextualTabGroups); }
            set { _model.ContextualTabGroups = value; }
        }

        /// <inheritdoc />
        public virtual AppSettingsViewModel AppSettingsViewModel
        {
            get { return _model.Dispatcher.Invoke(() => _model.AppSettingsViewModel); }
            set { _model.AppSettingsViewModel = value; }
        }

        /// <inheritdoc />
        public virtual IUserSettingsDbContext UserSettingsDbContext
        {
            get { return _model.Dispatcher.Invoke(() => _model.UserSettingsDbContext); }
        }

        /// <inheritdoc />
        public virtual bool AllDocs
        {
            get { return _model.Dispatcher.Invoke(() => _model.AllDocs); }
            set { _model.AllDocs = value; }
        }

        /// <inheritdoc />
        public virtual  CurrentOperation CurrentOperation
        {
            get { return _model.Dispatcher.Invoke(() => _model.CurrentOperation); }
            set { _model.CurrentOperation = value; }
        }

        /// <inheritdoc />
        public virtual IClientModel ClientViewModel
        {
            get { return _model.Dispatcher.Invoke(() => _model.ClientViewModel); }
            set { _model.ClientViewModel = value; }
        }

        /// <inheritdoc />
        public virtual JsonSerializerOptions JsonSerializerOptions
        {
            get { return _model.Dispatcher.Invoke(() => _model.JsonSerializerOptions); }
            set { _model.JsonSerializerOptions = value; }
        }

        /// <inheritdoc />
        public virtual object InstanceObjectId
        {
            get { return _model.Dispatcher.Invoke(() => _model.InstanceObjectId); }
            set { _model.InstanceObjectId = value; }
        }
    }
}