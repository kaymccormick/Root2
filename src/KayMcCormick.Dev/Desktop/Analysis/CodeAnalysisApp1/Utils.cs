using System ;
using Microsoft.Build.Locator ;

namespace CodeAnalysisApp1
{
    // ReSharper disable once UnusedType.Global
    internal static class Utils
    {
        // ReSharper disable once UnusedMember.Local
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private static VisualStudioInstance SelectVisualStudioInstance (
            VisualStudioInstance[] visualStudioInstances
        )
        {
            // ReSharper disable once LocalizableElement
            Console.WriteLine ( "Multiple installs of MSBuild detected please select one:" ) ;
            for ( var i = 0 ; i < visualStudioInstances.Length ; i ++ )
            {
                // ReSharper disable once LocalizableElement
                Console.WriteLine ( $"Instance {i + 1}" ) ;
                // ReSharper disable once LocalizableElement
                Console.WriteLine ( $"    Name: {visualStudioInstances[ i ].Name}" ) ;
                // ReSharper disable once LocalizableElement
                Console.WriteLine ( $"    Version: {visualStudioInstances[ i ].Version}" ) ;
                Console.WriteLine (
                                   // ReSharper disable once LocalizableElement
                                   $"    MSBuild Path: {visualStudioInstances[ i ].MSBuildPath}"
                                  ) ;
            }

            while ( true )
            {
                var userResponse = Console.ReadLine ( ) ;
                if ( int.TryParse ( userResponse , out var instanceNumber )
                     && instanceNumber > 0
                     && instanceNumber <= visualStudioInstances.Length )
                {
                    return visualStudioInstances[ instanceNumber - 1 ] ;
                }

                // ReSharper disable once LocalizableElement
                Console.WriteLine ( "Input not accepted, try again." ) ;
            }
        }
    }
}