using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BadWebClient
{
    
    public class HttpUtils
    {
        private const string CookieHeaderString = "Set-Cookie";

        /// <summary>
        /// Get a web request response
        /// </summary>
        /// <param name="Uri">Url to the Http request</param>
        /// <param name="ASPNetSession">Optional ASP.NET session</param>
        /// <returns>Http Response</returns>
        public async Task<HttpResponseMessage> GetWebRequest(string Uri, string ASPNetSession=null)
        {
            CookieContainer cookieContainer = new CookieContainer();
            Uri baseUrl = new Uri(new Uri(Uri).GetLeftPart(UriPartial.Authority));
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            {
                handler.MaxConnectionsPerServer = 100;
                if(ASPNetSession != null)
                {
                    cookieContainer.Add(baseUrl, new Cookie("ASP.NET_SessionId", ASPNetSession));
                }
                using (var client = new HttpClient(handler) { BaseAddress = baseUrl })
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("text/html"));
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                    //client.DefaultRequestHeaders.Remove("X-RequestDigest");
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.114 Safari/537.36 Edg/89.0.774.68");


                    HttpResponseMessage response = await client.GetAsync(Uri);
                    return response;
                }
            }
        }

        /// <summary>
        /// Returns ASP.NET Session ID
        /// </summary>
        /// <param name="Response">The Http Response Message</param>
        /// <returns>The Session ID or null if none is present</returns>
        public string GetASPNETSession(HttpResponseMessage Response)
        {
            if(Response.Headers.Contains(CookieHeaderString))
            {
                string cookie = Response.Headers.SingleOrDefault(header => header.Key == CookieHeaderString).Value.First()?.Split(';')[0].Split('=')[1];
                return cookie;

            }
            return null;
        }

    }
}
