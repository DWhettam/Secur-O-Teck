﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;

namespace SecuroteckClient
{
    #region Task 8 and beyond
    class Client
    {
        public static string apiKey = "";
        public static string userName = "";
        static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
        }       
        //static void SaveDetailsToFile()
        //{
        //    string contents = apiKey + Environment.NewLine + userName;
        //    File.WriteAllText(Environment.CurrentDirectory, contents);
        //}
        //static void LoadDetailsFromFile()
        //{
        //    using (StreamReader reader = new StreamReader(Environment.CurrentDirectory))
        //    {
        //        apiKey = reader.ReadLine();
        //        userName = reader.ReadLine();
        //    }
        //}
        //Talkback methods
        static async Task<string> GetTalkBackHello(string path)
        {
            HttpResponseMessage response = await client.GetAsync(path);
            return await response.Content.ReadAsAsync<string>();            
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
            return await response.Content.ReadAsAsync<string>();
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
            return await response.Content.ReadAsAsync<string>();
        }
        static async Task<string> DeleteUser(string apikey, string user)
        {
            if (apiKey != "")
            {
                string path = "api/user/removeuser?username=" + user;
                client.DefaultRequestHeaders.Add("ApiKey", apikey);
                HttpResponseMessage response = await client.DeleteAsync(path);
                return await response.Content.ReadAsAsync<string>();
            }
            return "You need to do a User Post or User Set first";

        }
        //Protected methods
        static async Task<string> GetProtectedHello()
        {            
            if (apiKey != "")
            {
                string path = "api/protected/hello";
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                HttpResponseMessage response = await client.GetAsync(path);
                return await response.Content.ReadAsAsync<string>();
            }
            return "You need to do a User Post or User Set first";

        }
        static async Task<string> GetProtectedSHA1(string message)
        {
            if (apiKey != "")
            {
                string path = "api/protected/sha1?message=" + message;
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                HttpResponseMessage response = await client.GetAsync(path);
                return await response.Content.ReadAsAsync<string>();
            }
            return "You need to do a User Post or User Set first";
        }
        static async Task<string> GetProtectedSHA256(string message)
        {
            if (apiKey != "")
            {
                string path = "api/protected/sha256?message=" + message;
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                HttpResponseMessage response = await client.GetAsync(path);
                return await response.Content.ReadAsAsync<string>();
            }
            return "You need to do a User Post or User Set first";
        }
        static async Task<string> GetProtectedPublicKey()
        {
            string path = "/api/protected/getpublickey";
            client.DefaultRequestHeaders.Add("ApiKey", apiKey);
            HttpResponseMessage response = await client.GetAsync(path);
            return await response.Content.ReadAsAsync<string>();
        }

        static async Task RunAsync()
        {            
            // Update port # in the following line.
            client.BaseAddress = new Uri("http://localhost:24702/");
            //LoadDetailsFromFile();
            Console.WriteLine("Hello. What would you like to do?");
            string response = Console.ReadLine();

            while (response != "Exit")
            {
                //client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
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
                                    //SaveDetailsToFile();
                                    break;
                                case "Set":
                                    apiKey = userResponse[2];
                                    userName = userResponse[3];
                                    //SaveDetailsToFile();
                                    Console.WriteLine("Stored");
                                    break;
                                case "Delete":
                                    Task<string> userDelete = DeleteUser(apiKey, userName);
                                    if (await Task.WhenAny(userDelete, Task.Delay(20000)) == userDelete)
                                    {
                                        Console.WriteLine(userDelete.Result);
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
                            switch (userResponse[1])
                            {
                                case "Hello":
                                    Task<string> protectedGetHello = GetProtectedHello();
                                    if (await Task.WhenAny(protectedGetHello, Task.Delay(20000)) == protectedGetHello)
                                    {
                                        Console.WriteLine(protectedGetHello.Result);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Request Timed Out");
                                    }
                                    break;
                                case "SHA1":
                                    Task<string> protectedGetSHA1 = GetProtectedSHA1(userResponse[2]);
                                    if (await Task.WhenAny(protectedGetSHA1, Task.Delay(20000)) == protectedGetSHA1)
                                    {
                                        Console.WriteLine(protectedGetSHA1.Result);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Request Timed Out");
                                    }
                                    break;
                                case "SHA256":
                                    Task<string> protectedGetSHA256 = GetProtectedSHA256(userResponse[2]);
                                    if (await Task.WhenAny(protectedGetSHA256, Task.Delay(20000)) == protectedGetSHA256)
                                    {
                                        Console.WriteLine(protectedGetSHA256.Result);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Request Timed Out");
                                    }
                                    break;
                                case "Get":
                                    if (userResponse[2] == "PublicKey")
                                    {
                                        Task<string> protectedGetPublicKey = GetProtectedPublicKey();
                                        if (await Task.WhenAny(protectedGetPublicKey, Task.Delay(20000)) == protectedGetPublicKey)
                                        {
                                            Console.WriteLine(protectedGetPublicKey.Result);
                                        }
                                        else
                                        {
                                            Console.WriteLine("Request Timed Out");
                                        }
                                    }                                    
                                    break;
                                default:
                                    break;
                            }
                            break;
                        default:
                            Console.WriteLine("Unrecognised Command");
                            break;
                    }
                    Console.WriteLine("What would you like to do next?");
                    response = Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }                
            }            
        }
    }
    #endregion
}
