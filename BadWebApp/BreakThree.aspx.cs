using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Diag = System.Diagnostics;

namespace BadWebApp
{
    public partial class BreakThree : System.Web.UI.Page
    {
        const int factor = 5;
        int total = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            BreakOne.counter = 0;
            total = 0;
            if (!Page.IsPostBack)
            {
                this.LimitBox.Text = WebConfigurationManager.AppSettings["Limit"];
                var queryLimit = Request.QueryString["Limit"];
                if (!String.IsNullOrEmpty(queryLimit))
                {
                    Diag.Trace.TraceWarning("Data was passed via query string");
                    this.LimitBox.Text = queryLimit;
                    this.DoAction_Click(null, null);
                }
            }


        }

        protected void DoAction_Click(object sender, EventArgs e)
        {
            int maxValue = 0;
            Int32.TryParse(this.LimitBox.Text, out maxValue);

            while (BreakOne.counter < maxValue)
            {
                total += factor;
                BreakOne.counter++;
            }

            this.ResultBox.Text = total.ToString();
        }
    }
}