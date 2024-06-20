using CommandLine;

namespace _3x_bruter
{
    public class Options
    {
        [Option('f', "file", Required = true, HelpText = "File containing panel targets.")]
        public string PanelTargetsFile { get; set; }

        [Option('e', "extract", Required = false, HelpText = "Extract proxies from the panels. (socks and http)")]
        public bool ExtractProxies { get; set; }
    }
}
