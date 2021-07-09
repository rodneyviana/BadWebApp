using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BadWebClient
{
    class Program
    {
        static void Pause()
        {
            Console.Write("Press any key to contnue...");
            _ = Console.ReadKey();
            Console.WriteLine();
        }
        static void Main(string[] args)
        {
            string baseUrl = "http://localhost:44373/BadApp";
            if(args.Length == 1)
            {
                baseUrl = args[0];
            }
            Console.WriteLine($"Base Url: {baseUrl}");

            string[] breaks = { $"{baseUrl}/BreakOne", $"{baseUrl}/BreakTwo", $"{baseUrl}/BreakThree" };

            HttpUtils utils = new HttpUtils();
            ConsoleKeyInfo option;
            HttpResponseMessage response;
            int cores = Int32.Parse(Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS"));
            do
            {
                Console.WriteLine($"*** Number of cores: {cores}");
                Console.WriteLine("Options:");
                Console.WriteLine("====================================");
                Console.WriteLine("1. Unresponsive Scenario");
                Console.WriteLine("2. High CPU");
                Console.WriteLine("3. w3wp.exe crash");
                Console.WriteLine("q. Quit");

                Console.Write("Choose 1, 2, 3 or q: ");
                option = Console.ReadKey();
                Console.WriteLine();
                Console.WriteLine($"Your choice was '{option.KeyChar}'.");


                switch (option.KeyChar)
                {

                    case '1':
                        Console.WriteLine("Wait about 10 seconds after continuing and capture a dump file of w3wp.exe");
                        Pause();
                        response = utils.GetWebRequest(breaks[0]).GetAwaiter().GetResult();
                        var cookie = utils.GetASPNETSession(response);

                        var now = DateTime.Now;

                        _ = Parallel.For(0, 120, (i, state) =>
                            {
                                Console.WriteLine($"BreakOne - Request {i} Delta Start: {(DateTime.Now - now).TotalSeconds} s");

                                try
                                {
                                    var responseLocal = utils.GetWebRequest(breaks[0], cookie).GetAwaiter().GetResult();
                                    Console.WriteLine($"BreakOne - Request {i} Delta End: {(DateTime.Now - now).TotalSeconds} s");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"BreakOne - Request {i} Delta End: {(DateTime.Now - now).TotalSeconds} s");
                                    Console.WriteLine($"+---> Exeception: {ex.ToString()}");

                                }


                            });
                        break;
                    case '2':
                        Console.WriteLine("Wait about 10 seconds after continuing and capture a dump file of w3wp.exe");
                        Pause();
                        int[] limits = { 1, 10, 100, 150, 200, 500, 1000, 1200, 1400, 1500, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000,
                            1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000,
                            1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000
                        };

                        for (int i = 0; i < 5 + cores - 1; i++)
                        {
                            Console.WriteLine($"Trying limit = {limits[i]}");
                            _ = utils.GetWebRequest(breaks[2] + $"?Limit={limits[i]}");
                        }
                        break;
                    case '3':
                        Console.WriteLine("Start IntelliTrace using the provided PowerShell script in elevated mode before continuing");
                        Pause();
                        _ = utils.GetWebRequest(breaks[1]);
                        break;
                    case 'q':
                        Console.WriteLine("goodbye!");
                        break;
                    default:
                        Console.WriteLine($"Invalid option '{option.KeyChar}'");
                        break;
                }
            } while (option.KeyChar != 'q');
        }

    }
}
