using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CS_NET_Web_App.Models;
using System.Configuration;

namespace CS_NET_Web_App.Controllers
{
  public class LoginController : Controller
  {
    // GET: Login
    public ActionResult Index()
    {
      return View(new LoginVM());
    }

    [HttpPost]
    public ActionResult Index(LoginVM pLogin)
    {
      var loginClient = new SBMemberSecuritySvc.MemberSecuritySoapClient("MemberSecuritySoap");
      SBMemberSecuritySvc.ValidatePasswordReturn validatePasswordReturn = loginClient.ValidatePassword(ConfigurationManager.AppSettings["WSToken"], pLogin.Username, -1, pLogin.Password);

      switch(validatePasswordReturn.ReturnCode)
      {
        case 0:
          var memberStatusClient = new SBMemberSvc.MemberSoapClient("MemberSoap");
          var memberStateReturn = memberStatusClient.FetchMemberState(ConfigurationManager.AppSettings["WSToken"], pLogin.Username, -1);

          if (memberStateReturn.ReturnCode == 0 && memberStateReturn.MemberIsSuspended != -1)
          {
            Session.Add("memberId", pLogin.Username);
            return RedirectToAction("Index", "Member");
          } else
          {
            ViewBag.Errormessage = "There was an error or the member account is suspended. Please contact your system administrator.";
          }
          break;
        case 1:
          ViewBag.ErrorMessage = "Undefined Error";
          break;
        case 2:
          ViewBag.ErrorMessage = "Required field missing";
          break;
        case 100:
          ViewBag.ErrorMessage = "Invalid security token";
          break;
        case 101:
          ViewBag.ErrorMessage = "Insufficient Permission";
          break;
        case 102:
          ViewBag.ErrorMessage = "Security Token Suspended";
          break;
        case 5000:
          ViewBag.ErrorMessage = "Member Not Found";
          break;
        case 15000:
          ViewBag.ErrorMessage = "Invalid Password";
          break;
        default:
          break;
      }
      return View(pLogin);
    }

    public ActionResult Logoff()
    {
      Session.Clear();
      return RedirectToAction("Index");
    }
  }
}