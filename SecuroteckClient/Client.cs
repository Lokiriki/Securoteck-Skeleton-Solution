using System;
using System.Collections.Generic;
using System.Globalization;
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
        static HttpClient client = new HttpClient();
        private static bool taskSet = false;
        private static HttpResponseMessage response = null;
        private static string inRequest = null;
        private static string localUserName = null;
        private static string localApiKey = null;
        private static string serverPublicKey = null;

        static void Main(string[] args)
        {
            client.BaseAddress = new Uri("http://localhost:24702/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Console.WriteLine("Hello. What would you like to do?");
            inRequest = Console.ReadLine();
            if (inRequest == "Exit")
            {
                Environment.Exit(0);
            }
            do
            {
                RunAsync().Wait();
            }
            while (inRequest != "Exit");

        }

        static async Task RunAsync()
        {
            try
            {
                Console.WriteLine("...please wait...");
                Task<string> task = null;

                #region TalkBack Hello
                if (inRequest == "TalkBack Hello")
                {
                    taskSet = true;
                    task = GetStringAsync("/api/talkback/hello");
                }
                #endregion

                #region TalkBack Sort
                else if (inRequest.Contains("TalkBack Sort ["))
                {
                    taskSet = true;
                    string[] requestNums = null;
                    string outRequest = "/api/talkback/sort?";

                    inRequest = inRequest.Remove(0, "TalkBack Sort [".Length);
                    inRequest = inRequest.TrimEnd(new char[] { ']', ' ' });
                    requestNums = inRequest.Split(',');


                    if (requestNums.Length == 1)
                    {
                        Console.WriteLine(requestNums[0]);
                        return;
                    }
                    else if (requestNums.Length > 1)
                    {
                        for (int i = 0; i < requestNums.Length; i++)
                        {
                            if (i == requestNums.Length - 1)
                            {
                                outRequest += "integers=" + requestNums[i];
                            }
                            else
                            {
                                outRequest += "integers=" + requestNums[i] + "&";
                            }
                        }
                    }
                    else if (requestNums.Length == 0)
                    {
                        outRequest += "integers=";
                    }

                    task = GetStringAsync(outRequest);
                }
                #endregion

                #region User Get
                else if (inRequest.Contains("User Get"))
                {
                    taskSet = true;
                    string outRequest = "/api/user/new?username=";

                    inRequest = inRequest.Remove(0, "User Get ".Length);
                    outRequest += inRequest;

                    task = GetStringAsync(outRequest);
                    Console.WriteLine(task.Result);
                }
                #endregion

                #region User Post
                else if (inRequest.Contains("User Post"))
                {
                    taskSet = true;
                    string userName = inRequest.Remove(0, "User Post ".Length);

                    task = PostUserASync(userName);
                }
                #endregion

                #region User Set
                else if (inRequest.Contains("User Set"))
                {
                    taskSet = true;
                    inRequest = inRequest.Remove(0, "User Set ".Length);
                    string[] newUser = inRequest.Split(new char[] { ' ' }, 2);
                    localUserName = newUser[0];
                    localApiKey = newUser[1];
                    Console.WriteLine("Stored");
                }
                #endregion

                #region User Delete
                else if (inRequest.Contains("User Delete"))
                {
                    if (localUserName == null)
                    {
                        Console.WriteLine("You need to do a User Post or User Set first");
                    }
                    else
                    {
                        taskSet = true;
                        var request = new HttpRequestMessage()
                        {
                            RequestUri = new Uri(client.BaseAddress + "api/user/removeuser?username=" + localUserName),
                            Method = HttpMethod.Delete,
                        };
                        request.Headers.Add("ApiKey", localApiKey);

                        task = DeleteUserASync(request);
                    }
                }
                #endregion

                #region Protected hello
                else if (inRequest.Contains("Protected Hello"))
                {
                    if (localApiKey == null)
                    {
                        Console.WriteLine("You need to do a User Post or User Set first");
                    }
                    else
                    {
                        taskSet = true;
                        var request = new HttpRequestMessage()
                        {
                            RequestUri = new Uri(client.BaseAddress + "api/protected/hello"),
                            Method = HttpMethod.Get,
                        };
                        request.Headers.Add("ApiKey", localApiKey);

                        task = GetRequestAsync(request);
                    }
                }
                #endregion

                #region Protected sha1
                else if (inRequest.Contains("Protected SHA1 "))
                {
                    if (localApiKey == null)
                    {
                        Console.WriteLine("You need to do a User Post or User Set first");
                    }
                    else
                    {
                        taskSet = true;
                        inRequest = inRequest.Remove(0, "Protected SHA1 ".Length);

                        var request = new HttpRequestMessage()
                        {
                            RequestUri = new Uri(client.BaseAddress + "api/protected/sha1?message=" + inRequest),
                        };
                        request.Headers.Add("ApiKey", localApiKey);

                        task = GetRequestAsync(request);
                    }
                }
                #endregion

                #region Protected sha256
                else if (inRequest.Contains("Protected SHA256 "))
                {
                    if (localApiKey == null)
                    {
                        Console.WriteLine("You need to do a User Post or User Set first");
                    }
                    else
                    {
                        taskSet = true;
                        inRequest = inRequest.Remove(0, "Protected SHA256 ".Length);

                        var request = new HttpRequestMessage()
                        {
                            RequestUri = new Uri(client.BaseAddress + "api/protected/sha256?message=" + inRequest),
                        };
                        request.Headers.Add("ApiKey", localApiKey);

                        task = GetRequestAsync(request);
                    }
                }
                #endregion

                #region Protected Get Public Key
                else if (inRequest == "Protected Get PublicKey")
                {
                    if (localApiKey == null)
                    {
                        Console.WriteLine("You need to do a User Post or User Set first");
                    }
                    else
                    {
                        taskSet = true;
                        var request = new HttpRequestMessage()
                        {
                            RequestUri = new Uri(client.BaseAddress + "api/protected/getpublickey"),
                        };
                        request.Headers.Add("ApiKey", localApiKey);

                        task = GetKeyAsync(request);
                    }
                }
                #endregion

                if (taskSet == true)
                {
                    if (await Task.WhenAny(task, Task.Delay(20000)) == task)
                    {
                        Console.WriteLine(task.Result);
                        taskSet = false;
                        Console.WriteLine("What would you like to do next?");
                        inRequest = Console.ReadLine();
                    }
                }
                else
                {
                    Console.WriteLine("What would you like to do next?");
                    inRequest = Console.ReadLine();
                }

                Console.Clear();
                Console.WriteLine(inRequest);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetBaseException().Message);
            }
        }

        static async Task<string> GetStringAsync(string path)
        {
            string responsestring = "";
            response = await client.GetAsync(path);
            responsestring = await response.Content.ReadAsStringAsync();
            return responsestring;
        }

        static async Task<string> GetRequestAsync(HttpRequestMessage request)
        {
            string responsestring = "";
            response = await client.SendAsync(request);
            responsestring = await response.Content.ReadAsStringAsync();
            return responsestring;
        }

        static async Task<string> GetKeyAsync(HttpRequestMessage request)
        {
            string responsestring = "";
            response = await client.SendAsync(request);
            responsestring = await response.Content.ReadAsStringAsync();
            if (responsestring != null)
            {
                serverPublicKey = responsestring;
                responsestring = "Got Public Key";
            }
            else
            {
                responsestring = "Couldn't Get the Public Key";
            }
            return responsestring;
        }

        static async Task<string> PostUserASync(string user)
        {
            string responsestring = "";
            response = await client.PostAsJsonAsync("api/user/new", user);
            responsestring = await response.Content.ReadAsStringAsync();
            //Check status of response
            if (response.StatusCode == HttpStatusCode.OK)
            {
                localUserName = user;
                localApiKey = responsestring;
                responsestring = "Got API Key";
            }
            else
            {
                responsestring = "Oops. Make sure your body contains a string with your username and your Content-Type is Content-Type:application/json";
            }
            return responsestring;
        }

        static async Task<string> DeleteUserASync(HttpRequestMessage Request)
        {
            string responsestring = "";
            response = await client.SendAsync(Request);
            responsestring = await response.Content.ReadAsStringAsync();
            //Check status of response
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (responsestring == "true")
                {
                    responsestring = "True";
                }
                else if (responsestring == "false")
                {
                    responsestring = "False";
                }
            }
            localApiKey = null;
            localUserName = null;
            return responsestring;
        }
        #endregion
    }
}