namespace ProjLib
{
    public interface IMruItem
    {
        string FilePath { get ; }
    }

    public class MruItem : IMruItem
    {
        public string FilePath { get ; set ; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public MruItem ( string filePath ) { FilePath = filePath ; }
    }
}