using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Boundary.Helper;
using Boundary.Helper.StaticValue;
using BusinessLogic.BussinesLogics;
using DataModel.Entities;
using DataModel.Enums;
using DataModel.Models.DataModel;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Boundary.Controllers.Ordinary
{
    public class SignupController : Controller
    {
        public ShopFinderUserManager ShopFinderUserManager { get; private set; }

        public SignupController() : this(new ShopFinderUserManager()) { }

        public SignupController(ShopFinderUserManager shopFinderUserManager)
        {
            ShopFinderUserManager = shopFinderUserManager;
        }


        [HttpGet]
        public ActionResult Telegram(string hashCode)
        {
            ViewBag.HashCode = hashCode;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Telegram(RegisterMemberDataModel model,string hashCode)
        {
            //todo: remove this
            model.Password = Helper.HelperFunction.GetMd5Hash(model.Password);
            model.ConfirmPassword = Helper.HelperFunction.GetMd5Hash(model.ConfirmPassword);

            var user = new UserBL().GetByHashCode(hashCode);
            if (user == null)
            {
                return Json(JsonResultHelper.FailedResultWithMessage("کاربر مورد نظر یافت نشد"), JsonRequestBehavior.AllowGet);
            }

            user.RoleCode = ERole.Member;
            user.IsActive = true;
            new UserBL().Update(user);


            await SignInAsync(new AppUser()
            {
                Id = user.Id,
                Email = user.Email,
                RoleCode = ERole.Member,
                Role = StaticRole.Member,
                Password = user.Password,
                IsActive = user.IsActive,
                UserName = user.UserName,
            }, isPersistent: false);

            Member member=new MemberBL().GetSummaryForSession(user.Id);
            return Json(JsonResultHelper.SuccessResult(member.Id), JsonRequestBehavior.AllowGet);
        }

        #region Helpers
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        [System.Web.Mvc.NonAction]
        [System.Web.Mvc.Authorize]
        public async Task SignInAsync(AppUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            var identity = await ShopFinderUserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            #region add information to user's role

            identity.AddClaim(new Claim(ClaimTypes.Role, user.Role.Name ?? string.Empty));

            #endregion


            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        #endregion
    }
}