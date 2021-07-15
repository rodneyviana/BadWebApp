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
using System.Data.SqlClient;

namespace BadWebApp
{
    public partial class SqlAccess : System.Web.UI.Page
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
            SqlProductCategories.Filter = Filter.Text;
            SqlProductCategories.Retrieve();
            GridView1.DataSource = SqlProductCategories.Categories;
            GridView1.DataBind();
            if(SqlProductCategories.ErrorMessage != null)
            {
                ErrorLabel.Text = "Error: "+SqlProductCategories.ErrorMessage;
            } else
            {
                ErrorLabel.Text = "";
            }
        }
    }


    public class SqlProductCategories
    {
        const string error = "Category filter cannot contain ' or ;";
        public static List<ProductCategory> Retrieve()
        {
            string conn = Environment.GetEnvironmentVariable("AzureSqlConnection");

            if (null != Filter && (Filter.Contains("'") || Filter.Contains(";")))
            {
                Categories = new List<ProductCategory>();

                ErrorMessage = error;
                return Categories;
            }

            using (SqlConnection connection = new SqlConnection(conn))
            {
                string query = null;
                if (String.IsNullOrEmpty(Filter))
                {
                    query = "SELECT CAST((SELECT * FROM SalesLT.vGetAllCategories FOR JSON PATH) AS VARCHAR(MAX))";
                }
                else
                {
                    query = "SELECT CAST((SELECT * FROM SalesLT.vGetAllCategories WHERE ProductCategoryName LIKE @Cat FOR JSON PATH) AS VARCHAR(MAX))";
                }
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    if (query.Contains("@Cat"))
                    {
                        cmd.Parameters.AddWithValue("@Cat", "%" + Filter + "%");
                    }
                    connection.Open();
                    string result = null;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result = reader[0] as string;
                            break;
                        }
                    }
                    Categories = JsonConvert.DeserializeObject<List<ProductCategory>>(result);
                    ErrorMessage = null;
                    return Categories;
                }
            }

        }

        public static string ErrorMessage = null;

        public static string Filter = null;
        public static List<ProductCategory> Categories { get; set; }
    }

}