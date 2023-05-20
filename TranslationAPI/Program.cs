using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net;
using System.Net.Http;

namespace TranslationAPI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Translation API - Created By FijiBoi";

            Console.WriteLine("Welcome to the Translation API");
            Console.WriteLine("1) Get Languages\t2) Translate Text");

            while (true)
            {
                Console.Write(">> ");
                int option = int.Parse(Console.ReadLine());
                
                switch (option)
                {
                    case 1:
                        GetLanguages().GetAwaiter().GetResult();
                        break;

                    case 2:
                        Console.WriteLine("Enter in the message you want to translate");
                        Console.Write(">> ");
                        string translate = Console.ReadLine();

                        Console.WriteLine("Enter in the desired language for the message to be translated to\n" +
                            "(Warning: If the translated message contains cyrillic characters or other, the message will be displayed as \"?\")\n");

                        Console.Write(">> ");

                        string to = Console.ReadLine();

                        Translate(translate, to).GetAwaiter().GetResult();
                        break;

                    case 3:
                        Console.WriteLine("Enter the text you want to detect the language");
                        string text = Console.ReadLine();

                        DetectLanguage(text).GetAwaiter().GetResult();
                        break;

                    default:
                        Console.WriteLine("Invalid Option");
                        break;
                }
            }
        }

        static async Task GetLanguages()
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpRequestMessage request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://google-translate1.p.rapidapi.com/language/translate/v2/languages"),

                    Headers =
                    {
                        { "X-RapidAPI-Key", "<API Key>" },
                        { "X-RapidAPI-Host", "<API Host>" },
                    },
                })
                {
                    using (HttpResponseMessage apiResponse = await client.SendAsync(request))
                    {
                        apiResponse.EnsureSuccessStatusCode();
                        string body = await apiResponse.Content.ReadAsStringAsync();

                        Response response = JsonSerializer.Deserialize<Response>(body);

                        for (int i = 0; i < response.data.languages.Length; i++)
                        {
                            Console.WriteLine($"Language: {response.data.languages[i].language}");
                        }
                    }
                }
            }
        }

        static async Task Translate(string text, string translateTo)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpRequestMessage request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://google-translate1.p.rapidapi.com/language/translate/v2"),

                    Headers =
                    {
                        { "X-RapidAPI-Key", "<API Key>" },
                        { "X-RapidAPI-Host", "<API Host>" },
                    },

                    Content = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "q", text },
                        { "target", translateTo },
                    }),
                })
                {
                    using (HttpResponseMessage apiResponse = await client.SendAsync(request))
                    {
                        apiResponse.EnsureSuccessStatusCode();
                        string body = await apiResponse.Content.ReadAsStringAsync();

                        Response response = JsonSerializer.Deserialize<Response>(body);

                        Console.WriteLine($"Translated Text: {response.data.translations[0].translatedText}");
                        Console.WriteLine($"Detected Language Source: {response.data.translations[0].detectedSourceLanguage}");

                    }
                };
            }
        }

        static async Task DetectLanguage(string text)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpRequestMessage request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://google-translate1.p.rapidapi.com/language/translate/v2/detect"),

                    Headers =
                    {
                        { "X-RapidAPI-Key", "<API Key>" },
                        { "X-RapidAPI-Host", "<API Host>" },
                    },

                    Content = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "q", text },
                    }),
                })
                {
                    using (HttpResponseMessage apiResponse = await client.SendAsync(request))
                    {
                        apiResponse.EnsureSuccessStatusCode();
                        string body = await apiResponse.Content.ReadAsStringAsync();

                        Response response = JsonSerializer.Deserialize<Response>(body);

                        Console.WriteLine($"API Detected: {response.data.detections[0][0].language}\n" +
                            $"Confidence: {response.data.detections[0][0].confidence}\n" +
                            $"Is Reliable: {response.data.detections[0][0].isReliable}");
                    }
                };
            }
        }
    }
}