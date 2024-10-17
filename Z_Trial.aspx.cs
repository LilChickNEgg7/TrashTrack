using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Capstone
{
    public partial class Z_Trial : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set the default active tab
                hfActiveTab.Value = "#tab1"; // Set Tab 1 as the default
            }
        }

        protected void btnTab1_Click(object sender, EventArgs e)
        {

        }
    }
}