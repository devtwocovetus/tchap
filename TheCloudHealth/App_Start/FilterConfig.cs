using System.Web;
using System.Web.Mvc;

namespace TheCloudHealth
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //New Code
            filters.Add(new RequireHttpsAttribute());
        }
    }
}
