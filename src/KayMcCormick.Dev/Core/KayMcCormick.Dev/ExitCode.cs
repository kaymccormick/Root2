namespace KayMcCormick.Dev
{
    /// <summary>Exit status of application.</summary>
    public enum ExitCode
    {
        /// <summary>Successful exit.</summary>
        Success = 0

       ,

        /// <summary>General error.</summary>
        GeneralError = 1

       ,

        /// <summary>Invalid arguments to application.</summary>
        ArgumentsError = 2

       ,

        /// <summary>
        /// </summary>
        ExceptionalError
    }
}