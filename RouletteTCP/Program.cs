// See https://aka.ms/new-console-template for more information
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace RouletteTCP_Client // Note: actual namespace depends on the project name.
{
    internal class Program {
        /* static void Main(string[] args) {
             Console.WriteLine("Hello World!");
         }*/

        static async Task Main(string[] args) {

            {
                RouletteNumberMsg msg = new RouletteNumberMsg {
                    Qualifier = "showWinningNumber",
                    Data = new Dictionary<string, int> {
                        ["showWinningNumber"] = 25 // Write Winning number here
                    }
                };

                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(msg, options);

                TcpClient client = new TcpClient("localhost", 4948);

                Console.WriteLine("Sending: " + jsonString + "\" to " + 4948);

                // Convert the JSON string to bytes
                byte[] jsonData = Encoding.UTF8.GetBytes(jsonString);

                using (NetworkStream stream = client.GetStream()) {
                    // Send the JSON data to the WPF listener
                    await stream.WriteAsync(jsonData, 0, jsonData.Length);
                }

                client.Close();
            }
        }

        public class RouletteNumberMsg {
            private string _qualifier;
            public string Qualifier { 
                get => _qualifier;
                set {
                    _qualifier = value;
                }
            }
            private Dictionary<string, int> _data;
            public Dictionary<string, int> Data {
                get { return _data; }
                set {
                    if (value != null) {
                        foreach (var item in value) {
                            if(item.Key == "showWinningNumber") {
                                if (item.Value < 0 || item.Value > 36) {
                                    throw new ArgumentOutOfRangeException(nameof(value), "Values in the Data dictionary must be between 0 and 36.");
                                }
                            }
                            
                        }
                    }
                    _data = value;
                }
            }
        }
    }
}


