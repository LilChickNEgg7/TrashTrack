using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using static Capstone.BO_Dashboard;
using Microsoft.AspNet.SignalR;
using System.Web.Routing;
using System;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using System.Web.Routing;


namespace Capstone
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            // Register Web API routes
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // Register SignalR hubs
            //RouteTable.Routes.MapHubs();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}