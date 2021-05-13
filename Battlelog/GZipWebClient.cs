using System;
using System.Net;

namespace Battlelog
{
    public class GZipWebClient : WebClient
    {
        private string _userAgent;

        public GZipWebClient(string ua = "Mozilla/5.0 (compatible; BFUDPClient)")
        {
            _userAgent = ua;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.UserAgent = _userAgent;
            return request;
        }
    }
}
