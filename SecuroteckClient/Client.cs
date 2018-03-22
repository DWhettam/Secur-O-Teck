using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SecuroteckClient
{
    #region Task 8 and beyond
    class Client
    {
        public static string apiKey;
        public static string userName;
        static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
        }       
        //Talkback methods
        static async Task<string> GetTalkBackHello(string path)
        {
            HttpResponseMessage response = await client.GetAsync(path);
            return await response.Content.ReadAsStringAsync();            
        }
        static async Task<string> GetTalkBackSort(string intArrayString)
        {
            string path = "/api/talkback/sort?";
            intArrayString = intArrayString.Substring(1, intArrayString.Length - 2);
            int[] intArray = intArrayString.Split(',').Select(int.Parse).ToArray();

            foreach (var item in intArray)
            {
                path += "integers=" + item + "&";
            }
            path = path.Remove(path.Length - 1, 1);
            HttpResponseMessage response = await client.GetAsync(path);
            return await response.Content.ReadAsStringAsync();
        }
        //User methods
        static async Task<string> GetUser(string user)
        {
            string path = "api/user/new?username=" + user;
            HttpResponseMessage response = await client.GetAsync(path);
            return await response.Content.ReadAsStringAsync();
        }
        static async Task<string> PostUser(string user)
        {
            string path = "api/user/new";
            HttpResponseMessage response = await client.PostAsJsonAsync(path, user);
            if (response.IsSuccessStatusCode)
            {
                apiKey = response.Content.ReadAsAsync<string>().Result;
                userName = user;
                return "Got API Key";
            }           
            return await response.Content.ReadAsStringAsync();
        }
        static async Task<string> DeleteUser(string apikey, string user)
        {
            string path = "api/user/removeuser?username=" + user;
            client.DefaultRequestHeaders.Add("ApiKey", apikey);           
            HttpResponseMessage response = await client.DeleteAsync(path);            
            return await response.Content.ReadAsStringAsync();
        }

        static async Task RunAsync()
        {            
            // Update port # in the following line.
            client.BaseAddress = new Uri("http://localhost:24702/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                Console.WriteLine("Hello. What would you like to do?");
                string response = Console.ReadLine();
                
                while (response != "Exit")
                {
                    string[] userResponse = response.Split(' ');
                    Console.WriteLine("...please wait...");
                    switch (userResponse[0])
                    {
                        case "TalkBack":
                            switch (userResponse[1])
                            {
                                case "Hello":
                                    Task<string> talkBackHello = GetTalkBackHello("/api/talkback/hello");
                                    if (await Task.WhenAny(talkBackHello, Task.Delay(20000)) == talkBackHello)
                                    {
                                        Console.WriteLine(talkBackHello.Result);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Request Timed Out");
                                    }

                                    break;
                                case "Sort":
                                    Task<string> talkBackSort = GetTalkBackSort(userResponse[2]);
                                    if (await Task.WhenAny(talkBackSort, Task.Delay(20000)) == talkBackSort)
                                    {
                                        Console.WriteLine(talkBackSort.Result);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Request Timed Out");
                                    }
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "User":
                            switch (userResponse[1])
                            {
                                case "Get":
                                    Task<string> userGet = GetUser(userResponse[2]);
                                    if (await Task.WhenAny(userGet, Task.Delay(20000)) == userGet)
                                    {
                                        Console.WriteLine(userGet.Result);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Request Timed Out");
                                    }
                                    break;
                                case "Post":
                                    Task<string> userPost = PostUser(userResponse[2]);
                                    if (await Task.WhenAny(userPost, Task.Delay(20000)) == userPost)
                                    {
                                        Console.WriteLine(userPost.Result);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Request Timed Out");
                                    }
                                    break;
                                case "Set":
                                    apiKey = userResponse[2];
                                    userName = userResponse[3];
                                    Console.WriteLine("Stored");
                                    break;
                                case "Delete":
                                    Task<string> userDelte = DeleteUser(apiKey, userName);
                                    if (await Task.WhenAny(userDelte, Task.Delay(20000)) == userDelte)
                                    {
                                        Console.WriteLine(userDelte.Result);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Request Timed Out");
                                    }
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "Protected":
                            //Protected(userResponse[1]);
                            break;
                        default:
                            Console.WriteLine("Unrecognised Command");
                            break;
                    }
                    Console.WriteLine("What would you like to do next?");
                    response = Console.ReadLine();
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
    #endregion
}
