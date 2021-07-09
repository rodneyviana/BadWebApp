using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Diag = System.Diagnostics;

namespace BadWebApp
{
    public partial class BreakOne : System.Web.UI.Page
    {
        public static Random rnd = new Random();
        public const string Url = "https://www.microsoft.com/apc";
        public static byte counter = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["Session"] == null)
            {
                Session["Session"] = (int)0;
            } else
            {
                Session["Session"] = (int)(Session["Session"]) + 1;
            }
            var pause = rnd.Next(5000);
            Diag.Trace.TraceInformation($"The delay is {pause} ms");
            Thread.Sleep(pause);
            this.SessionBox.Text = Session["Session"].ToString();
        }
    }
}