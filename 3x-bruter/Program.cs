/*
 * Project: 3x_bruter
 * Author: Joni Fon < jf-github-communication@bastardi.net >
 * Date: 20.05.2024
 * Description: A powerful tool designed to assist in the brute force attack of 3x-UI panels, aiming to discover valid login credentials. This project is intended for educational and security testing purposes only.
 */

using CommandLine;

namespace _3x_bruter
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
               .WithParsed(options => ProcessArguments(options).GetAwaiter().GetResult());
        }

        private static async Task ProcessArguments(Options options)
        {
            if (string.IsNullOrEmpty(options.PanelTargetsFile))
            {
                Console.WriteLine("Please provide the panel targets file. Use --help for more information.");
                return;
            }

            var bruteForcer = new PanelBruteForcer();

            if (options.ExtractProxies)
            {
                await bruteForcer.BrutePanelAsync(options.PanelTargetsFile, true);
            }
            else
            {
                await bruteForcer.BrutePanelAsync(options.PanelTargetsFile);
            }
        }
    }
}
