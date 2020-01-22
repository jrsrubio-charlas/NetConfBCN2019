using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NetConfBCN2019Demo2.ConsoleAppClient
{
    class Program
    {
        static void Main(string[] args)
        {
            WaitingForMessage(args).GetAwaiter().GetResult();
        }

        private static async Task WaitingForMessage (string[] args)
        {
            //HubConnection connection;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("https://netconfbcn2019demo2publishtosignalr.azurewebsites.net");
            HttpResponseMessage response = await client.GetAsync("/api/negotiatevotingresults");
            response.EnsureSuccessStatusCode();
            var content = JsonConvert.DeserializeObject<NegotiateResponse>(await response.Content.ReadAsStringAsync());

            string token = content.Accesstoken;
            string url = content.Url;

            var connection = new HubConnectionBuilder()
                .WithUrl(url, options => {
                    options.AccessTokenProvider = () => Task.FromResult(token);
                })
                .Build();

            connection.StartAsync().ContinueWith(task => {
                if (task.IsFaulted)
                {
                    Console.WriteLine("Se ha producido un error al establecer la conexión: {0}",
                                      task.Exception.GetBaseException());
                }
                else
                {
                    Console.WriteLine("Connected");

                    connection.On<string>("VotingResults", (resultados) => {
                        Console.WriteLine($"{resultados}");
                    });

                    while (true)
                    {
                        string message = Console.ReadLine();

                        if (string.IsNullOrEmpty(message))
                        {
                            break;
                        }

                    }
                }

            }).Wait();

            await connection.StopAsync();
        }
    }
}
