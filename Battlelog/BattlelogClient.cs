using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Battlelog
{
    public static class BattlelogClient
    {
        static GZipWebClient _webClient;

        static BattlelogClient()
        {
            _webClient = new GZipWebClient();
        }

        /// <summary>
        ///     Get the server show data
        /// </summary>
        /// <param name="guid">Server guid</param>
        /// <param name="platform">Platform, usually pc</param>
        /// <returns></returns>
        public static dynamic GetServerShow(string guid, string platform = "pc")
        {
            try
            {
                string result = _webClient.DownloadString($"https://battlelog.battlefield.com/bf4/servers/show/{platform}/{guid}/SERVER/?json=1");

                JObject response = JObject.Parse(result);
                if (!response.TryGetValue("type", out var type))
                    throw new Exception("Request failed");

                if (!response.TryGetValue("message", out var message))
                    throw new Exception("message didn't exist");

                if (!message.ToObject<JObject>().TryGetValue("SERVER_INFO", out var serverInfo))
                    throw new Exception("SERVER_INFO didn't exist");

                return serverInfo.ToObject<dynamic>();
            }
            catch (Exception e)
            {
                //Handle exceptions here however you want
                Console.WriteLine(e.ToString());
                return null;
            }
        }
    }
}
