#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// KayMcCormick.Dev
// IConfigSource.cs
// 
// 2020-03-12-12:35 AM
// 
// ---
#endregion
namespace KayMcCormick.Dev.Logging
{
    internal interface IConfigSource
    {
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once UnusedParameter.Global
        string PathForLogFile(ILogFileSpecification spec);
    }
}