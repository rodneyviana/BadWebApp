using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Diag = System.Diagnostics;

namespace BadWebApp
{
    public partial class BreakTwo : System.Web.UI.Page
    {

        #region static
        private static HttpClient client = new HttpClient();
        #endregion

        private string GetWeb(string url)
        {
            try
            {
                // Create a request for the URL. 		
                WebRequest request = WebRequest.Create(url);

                // Get the response.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // Display the status.
                Console.WriteLine(response.StatusDescription);
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();

                // Cleanup the streams and the response.
                reader.Close();
                dataStream.Close();
                response.Close();
                Diag.Trace.TraceInformation("Response received on GetWeb()");
                return responseFromServer;
            } catch
            {
                // it may be transient
                //  try again
                Diag.Trace.TraceError("There was a transient exception on GetWeb()");
                return GetWeb(url);
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            var response = GetWeb(BreakOne.Url);
        }
    }

}