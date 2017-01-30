using System.Configuration;
using System.Web.Configuration;
using System.Web.Mvc;
using Boundary.Controllers.Ordinary;
using Boundary.Helper;
using Boundary.Helper.StaticValue;

namespace Boundary.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles = StaticString.Role_SuperAdmin)]
    public class AppManagmentController : BaseController
    {
        //todo:ghesmate pass parspalam code conam baz


        public ActionResult EncryptConnString()
        {
            CheckSessionDataModel checkSession = CheckAdminSession();
            if (!checkSession.IsSuccess)
            {
                if (checkSession.Message == StaticString.Message_WrongAccess)
                    return RedirectToAction(StaticString.Action_Login, StaticString.Controller_ForLogin, new { area = "" });
                if (checkSession.Message == StaticString.Message_UnSuccessFull)
                    return RedirectToAction(StaticString.Action_Error, StaticString.Controller_ForError, new { area = "" });
                return Json(JsonResultHelper.FailedResultWithMessage(checkSession.Message), JsonRequestBehavior.AllowGet);
            }

            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            ConfigurationSection section = config.GetSection("connectionStrings");
            if (!section.SectionInformation.IsProtected)
            {
                section.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
                config.Save();
            }

            section = config.GetSection("hibernate-configuration");
            if (!section.SectionInformation.IsProtected)
            {
                section.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
                config.Save();
            }
            return Json(JsonResultHelper.SuccessResult(), JsonRequestBehavior.AllowGet);
        }


        public ActionResult DecryptConnString()
        {
            CheckSessionDataModel checkSession = CheckAdminSession();
            if (!checkSession.IsSuccess)
            {
                if (checkSession.Message == StaticString.Message_WrongAccess)
                    return RedirectToAction(StaticString.Action_Login, StaticString.Controller_ForLogin, new { area = "" });
                if (checkSession.Message == StaticString.Message_UnSuccessFull)
                    return RedirectToAction(StaticString.Action_Error, StaticString.Controller_ForError, new { area = "" });
                return Json(JsonResultHelper.FailedResultWithMessage(checkSession.Message), JsonRequestBehavior.AllowGet);
            }

            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            ConfigurationSection section = config.GetSection("connectionStrings");
            if (section.SectionInformation.IsProtected)
            {
                section.SectionInformation.UnprotectSection();
                config.Save();
            }

            section = config.GetSection("hibernate-configuration");
            if (section.SectionInformation.IsProtected)
            {
                section.SectionInformation.UnprotectSection();
                config.Save();
            }
            return Json(JsonResultHelper.SuccessResult(), JsonRequestBehavior.AllowGet);
        }
    }
}