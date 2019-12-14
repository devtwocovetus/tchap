using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;
using ActionFilterAttribute = System.Web.Mvc.ActionFilterAttribute;

namespace TheCloudHealth.Filters
{
    public class AllowCrossSiteJsonAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Method", "*");
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Header", "*");
            base.OnActionExecuting(filterContext);
        }
    }
}