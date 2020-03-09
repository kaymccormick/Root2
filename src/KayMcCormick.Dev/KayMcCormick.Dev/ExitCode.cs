namespace KayMcCormick.Dev
{
    /// <summary>Exit status of application.</summary>
    public enum ExitCode
    {
        /// <summary>Successful exit.</summary>
        // ReSharper disable once UnusedMember.Global
        Success = 0

       ,

        /// <summary>General error.</summary>
        GeneralError = 1

       ,

        /// <summary>Invalid arguments to application.</summary>
        // ReSharper disable once UnusedMember.Global
        ArgumentsError = 2

       , ExceptionalError
    }
}