using Microsoft.EntityFrameworkCore;

namespace AnalysisControls.ViewModel
{
    public interface IUserSettingsDbContext
    {
        DbSet<UserSetting> UserSettings { get; set; }
    }
}