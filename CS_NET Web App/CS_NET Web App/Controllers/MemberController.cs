using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CS_NET_Web_App.Models;

namespace CS_NET_Web_App.Controllers
{
  public class MemberController : Controller
  {
    // GET: Member
    public ActionResult Index()
    {
      if (Session["memberId"] == null)
      {
        return RedirectToAction("Index", "Login");
      }

      var member = new MemberVM();
      var memberRewardsClient = new SBOfferSvc.OfferSoapClient("OfferSoap");
      SBOfferSvc.MemberActiveRewardsReturn memberRewardsReturn =
        memberRewardsClient.MemberActiveRewards(System.Configuration.ConfigurationManager.AppSettings["WSToken"],
        Session["memberId"].ToString(), -1, "PAW");
      if(memberRewardsReturn.ReturnCode != 0)
      {
        ViewBag.ErrorMessage = "There was an error getting your rewards";
      } else
      {
        member.Rewards = memberRewardsReturn.MemberActiveRewards.ToList();
      }
      return View(member);
    }
  }
}