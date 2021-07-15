using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using System.Text;

namespace BadWebApp
{
    public partial class FunctionAccess : System.Web.UI.Page
    {
        public ProductCategories products;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {

            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ProductCategories.Filter = Filter.Text;
            ProductCategories.Retrieve();
            GridView1.DataSource = ProductCategories.Categories;
            GridView1.DataBind();
            if(ProductCategories.ErrorMessage != null)
            {
                ErrorLabel.Text = "Error: "+ProductCategories.ErrorMessage;
            } else
            {
                ErrorLabel.Text = "";
            }
        }
    }


    public class ProductCategories
    {
        protected static HttpWebResponse GetResponse(WebRequest Request)
        {
            try
            {
                HttpWebResponse response = (HttpWebResponse)Request.GetResponse();
                return response;
            }
            catch (WebException ex)
            {
                if (ex.Response == null || ex.Status != WebExceptionStatus.ProtocolError)
                    throw;

                return (HttpWebResponse)ex.Response;
            }
        }
        public static List<ProductCategory> Retrieve()
        {
#if DEBUG
            Categories = new List<ProductCategory>()
            {
                new ProductCategory()
                {
                    ParentProductCategoryName = "Bikes",
                    ProductCategoryID = 100,
                    ProductCategoryName = "Mountain Bikes"
                },
                new ProductCategory()
                {
                    ParentProductCategoryName = "Bikes",
                    ProductCategoryID = 100,
                    ProductCategoryName = "snow Bikes"
                }

            };
#else
            // Create a request for the URL. 	
            string url = Environment.GetEnvironmentVariable("FunctionEndpoint") + (Filter.Length > 0 ? "&cat="+HttpUtility.UrlEncode(Filter) : "");	
            WebRequest request = WebRequest.Create(url);
            // Get the response.
            HttpWebResponse response = GetResponse(request);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            
            string responseFromServer = reader.ReadToEnd();
            dataStream.Close();
            reader.Close();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Categories = JsonConvert.DeserializeObject<List<ProductCategory>>(responseFromServer);
                ErrorMessage = null;
            } else
            {
                Categories = new List<ProductCategory>();
                try
                {
                    Dictionary<string, string> error = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseFromServer);
                    ErrorMessage = error["error"];
                } catch
                {
                    ErrorMessage = $"({response.StatusCode}) {response.StatusDescription} payload: {responseFromServer}";
                }
                
            }
#endif

            return Categories;
        }

        public static string ErrorMessage = null;

        public static string Filter = null;
        public static List<ProductCategory> Categories { get; set; }
    }

    public class ProductCategory
    {
        public string ParentProductCategoryName { get; set; }
        public string ProductCategoryName { get; set; }
        public int ProductCategoryID { get; set; }
    }
}