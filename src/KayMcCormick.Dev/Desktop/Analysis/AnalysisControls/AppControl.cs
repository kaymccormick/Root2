using System;
using System.Windows.Controls;
using NLog;
using NLog.Fluent;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class AppControl : Control, IAppControl
    {
        /// <summary>
        /// 
        /// </summary>
        public AppControl()
        {
            ControlId = Guid.NewGuid();
            Logger = LogManager.GetLogger(GetType().Name + "." + ControlId.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public Guid ControlId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected Logger Logger { get; }

        /// <inheritdoc />
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            new LogBuilder(Logger).Message("OnInitialized").Write();
        }
    }
}