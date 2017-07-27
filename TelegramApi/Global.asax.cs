using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TelegramApi.Controllers;

namespace TelegramApi
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //TelegramBotClient bot = new TelegramBotClient(Resource.General.Bot_Token);
            //#if DEBUG
            //            bot.SetWebhookAsync("https://localhost:44325/api/bot").Wait();
            //#else
            //            bot.SetWebhookAsync("http://www.telegram.hoojibooji.com/api/bot").Wait(); 
            //#endif

            //Thread thread = new Thread(new ThreadStart(new BotController().GetUserActions));
            //thread.Start();
        }
    }
}
