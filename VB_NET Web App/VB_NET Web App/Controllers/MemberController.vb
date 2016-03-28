Imports System.Web.Mvc

Namespace Controllers
  Public Class MemberController
    Inherits Controller

    ' GET: Member/Details/5
    Function Index() As ActionResult
      If Session("memberId") Is Nothing Then
        Return RedirectToAction("Index", "Login")
      Else
        Dim member As New MemberVM
        Dim memberRewardsClient = New SBOfferSvc.OfferSoapClient("OfferSoap")
        Dim memberRewardsReturn As SBOfferSvc.MemberActiveRewardsReturn = memberRewardsClient.MemberActiveRewards(ConfigurationManager.AppSettings("WSToken"), Session("memberId"), -1, "PAW")
        If memberRewardsReturn.ReturnCode <> 0 Then
          ViewBag.ErrorMessage = "There was an error getting your rewards."
        Else
          member.Rewards = memberRewardsReturn.MemberActiveRewards.ToList()
        End If
        Return View(member)
      End If
    End Function

  End Class
End Namespace