using System.Web.Mvc;
using Boundary.Controllers.Ordinary;
using Boundary.Helper;
using Boundary.Helper.StaticValue;

namespace Boundary.Areas.Seller.Controllers
{
    [Authorize(Roles = StaticString.Role_Seller)]
    public class StoreManagmentController : BaseController
    {
        public ActionResult SellerPanel()
        {
            CheckSessionDataModel checkSession = CheckSallerSession();
            if (!checkSession.IsSuccess)
            {
                if (checkSession.Message == StaticString.Message_WrongAccess)
                    return RedirectToAction(StaticString.Action_Login, StaticString.Controller_ForLogin, new { area = "" });
                if (checkSession.Message == StaticString.Message_UnSuccessFull)
                    return RedirectToAction(StaticString.Action_Error, StaticString.Controller_ForError, new { area = "" });
                return Json(JsonResultHelper.FailedResultWithMessage(checkSession.Message), JsonRequestBehavior.AllowGet);
            }
            return View();
        }
        public ActionResult EditStore() 
        {
            CheckSessionDataModel checkSession = CheckSallerSession();
            if (!checkSession.IsSuccess)
            {
                if (checkSession.Message == StaticString.Message_WrongAccess)
                    return RedirectToAction(StaticString.Action_Login, StaticString.Controller_ForLogin, new { area = "" });
                if (checkSession.Message == StaticString.Message_UnSuccessFull)
                    return RedirectToAction(StaticString.Action_Error, StaticString.Controller_ForError, new { area = "" });
                return Json(JsonResultHelper.FailedResultWithMessage(checkSession.Message), JsonRequestBehavior.AllowGet);
            }
            return View();
        }
        public ActionResult EditStorePhoto()
        {
            CheckSessionDataModel checkSession = CheckSallerSession();
            if (!checkSession.IsSuccess)
            {
                if (checkSession.Message == StaticString.Message_WrongAccess)
                    return RedirectToAction(StaticString.Action_Login, StaticString.Controller_ForLogin, new { area = "" });
                if (checkSession.Message == StaticString.Message_UnSuccessFull)
                    return RedirectToAction(StaticString.Action_Error, StaticString.Controller_ForError, new { area = "" });
                return Json(JsonResultHelper.FailedResultWithMessage(checkSession.Message), JsonRequestBehavior.AllowGet);
            }
            return View();
        }
        public ActionResult EditStoreLogo()
        {
            CheckSessionDataModel checkSession = CheckSallerSession(true);
            if (!checkSession.IsSuccess)
            {
                if (checkSession.Message == StaticString.Message_WrongAccess)
                    return RedirectToAction(StaticString.Action_Login, StaticString.Controller_ForLogin, new { area = "" });
                if (checkSession.Message == StaticString.Message_UnSuccessFull)
                    return RedirectToAction(StaticString.Action_Error, StaticString.Controller_ForError, new { area = "" });
                return Json(JsonResultHelper.FailedResultWithMessage(checkSession.Message), JsonRequestBehavior.AllowGet);
            }
            return View();
        }
        public ActionResult EditSallerPhoto()
        {
            CheckSessionDataModel checkSession = CheckSallerSession();
            if (!checkSession.IsSuccess)
            {
                if (checkSession.Message == StaticString.Message_WrongAccess)
                    return RedirectToAction(StaticString.Action_Login, StaticString.Controller_ForLogin, new { area = "" });
                if (checkSession.Message == StaticString.Message_UnSuccessFull)
                    return RedirectToAction(StaticString.Action_Error, StaticString.Controller_ForError, new { area = "" });
                return Json(JsonResultHelper.FailedResultWithMessage(checkSession.Message), JsonRequestBehavior.AllowGet);
            }
            return View();
        }

    }
}