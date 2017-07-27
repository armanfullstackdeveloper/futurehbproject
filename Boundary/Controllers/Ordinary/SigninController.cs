using System.Threading.Tasks;
using System.Web.Mvc;
using Boundary.Helper;
using BusinessLogic.BussinesLogics.RelatedToOrder;
using DataModel.Models.ViewModel;

namespace Boundary.Controllers.Ordinary
{
    public class SigninController : Controller
    {
        public ShopFinderUserManager ShopFinderUserManager { get; private set; }

        public SigninController() : this(new ShopFinderUserManager()) { }

        public SigninController(ShopFinderUserManager shopFinderUserManager)
        {
            ShopFinderUserManager = shopFinderUserManager;
        }


        [HttpGet]
        public ActionResult Telegram(string hashCode)
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Telegram(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(JsonResultHelper.FailedResultWithMessage(), JsonRequestBehavior.AllowGet);

            //todo: remove this
            model.Password = Helper.HelperFunction.GetMd5Hash(model.Password);

            //todo: ebteda check konam in user az ghabl sabt shode ya na
            AppUser user = await ShopFinderUserManager.FindAsync(model.UserName, model.Password);

            string telegramLink = "";

            //age sabt nashode, user jadid misazam va ye hash behesh ekhtesas midam va link bot dorost mikonam barash
            if (user == null)
            {
                
            }
            else
            {
                if (user.IsActive == false)
                    return Json(JsonResultHelper.FailedResultWithMessage("وضعیت کاربری شما غیر فعال است"), JsonRequestBehavior.AllowGet);

            }

            //age sabt shode bashe, va chat-id ro ham dashte bashim, behesh code faal sazi midim



            //age sabt shode bashe, va chat-id ro nadashte bashim, ye hash behesh ekhtesas midam va link bot dorost mikonam barash

            
            return Json(JsonResultHelper.SuccessResult(telegramLink), JsonRequestBehavior.AllowGet);
        }
    }
}