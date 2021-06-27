using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FinalFantasu.Models;
using FinalFantasu.Utils;

namespace FinalFantasu.Controllers
{
    public class BaseController : Controller
    {
		// GET: Base
		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var user = (User)Session[SessionUtils.SESSION.UserInfo];
			if (user == null) {
				filterContext.Result = new RedirectToRouteResult(
					new RouteValueDictionary(new {controller="Home", action="Index"}));
			}
			else {
				if (user.ID != 1) {
					filterContext.Result = new RedirectToRouteResult(
						new RouteValueDictionary(new { controller = "Home", action = "Index" }));
				}
			}
			base.OnActionExecuting(filterContext);
		}
	}
}