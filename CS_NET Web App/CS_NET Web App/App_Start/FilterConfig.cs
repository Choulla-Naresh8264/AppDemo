using System.Web;
using System.Web.Mvc;

namespace CS_NET_Web_App
{
  public class FilterConfig
  {
    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
      filters.Add(new HandleErrorAttribute());
    }
  }
}
