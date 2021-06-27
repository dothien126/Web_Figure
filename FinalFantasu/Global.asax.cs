using FinalFantasu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FinalFantasu.Utils;


namespace FinalFantasu
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}
		void Application_PreRequestHandlerExecute(object sender, EventArgs e)
		{
			try
			{
				User user = (User)Session[SessionUtils.SESSION.UserInfo];
				if (user != null)
					Session[SessionUtils.SESSION.Cart] = new Models.Interactives.AniFigDB().GetListCarts(user.ID).Count;					
			}
			catch (Exception) { }
		}
	}
}
