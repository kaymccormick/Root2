using CommandLine;

namespace Client2
{
    internal class ProjInterfaceOptions
    {
        [Option('w', "window", Default = "Client2Window1")]
        public string window { get; set; }
        [Option('c', "command")]
        public string Command { get; set; }
        [Option('a', "arg")]
        public string Argument { get; set; }

    }
}