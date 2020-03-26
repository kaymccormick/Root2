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
        
        
        string PathForLogFile(ILogFileSpecification spec);
    }
}