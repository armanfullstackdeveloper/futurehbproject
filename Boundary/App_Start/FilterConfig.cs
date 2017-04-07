using System.Web.Mvc;
using Boundary.Helper.AttributeFilters;

namespace Boundary
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            //for exception handeling
            filters.Add(new HoojiBoojiExceptionHandler());
        }
    }
}
