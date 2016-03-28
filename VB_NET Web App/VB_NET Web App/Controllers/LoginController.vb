Imports System.Web.Mvc
Imports System.Configuration

Namespace Controllers
    Public Class LoginController
        Inherits Controller

    ' GET: Login
    Function Index() As ActionResult
      Return View(New LoginVM)
    End Function

    ' POST: Login
    <HttpPost>
    Function Index(ByVal pLogin As LoginVM) As ActionResult
      Dim loginClient = New SBMemberSecuritySvc.MemberSecuritySoapClient("MemberSecuritySoap")
      Dim validatePasswordReturn As SBMemberSecuritySvc.ValidatePasswordReturn = loginClient.ValidatePassword(ConfigurationManager.AppSettings("WSToken"), pLogin.Username, -1, pLogin.Password)
      Select Case validatePasswordReturn.ReturnCode
        Case 0
          Dim memberStatusClient = New SBMemberSvc.MemberSoapClient("MemberSoap")
          Dim memberStateReturn As SBMemberSvc.FetchMemberStateReturn = memberStatusClient.FetchMemberState(ConfigurationManager.AppSettings("WSToken"), pLogin.Username, -1)
          If memberStateReturn.ReturnCode = 0 And Not memberStateReturn.MemberIsSuspended = True Then
            Session.Add("memberId", pLogin.Username)
            Return RedirectToAction("Index", "Member")
          Else
            ViewBag.ErrorMessage = "There was an error or the member account is suspended. Please contact your system administrator."
          End If
        Case 1
          ViewBag.ErrorMessage = "Undefined Error"
        Case 2
          ViewBag.ErrorMessage = "Required field missing"
        Case 100
          ViewBag.ErrorMessage = "Invalid security token"
        Case 101
          ViewBag.ErrorMessage = "Insufficient Permission"
        Case 102
          ViewBag.ErrorMessage = "Security Token Suspended"
        Case 5000
          ViewBag.ErrorMessage = "Member Not Found"
        Case 15000
          ViewBag.ErrorMessage = "Invalid Password"
        Case Else
          ViewBag.ErrorMessage = "Unknown Return Code"
      End Select
      Return View(pLogin)
    End Function

    Function Logoff() As ActionResult
      Session.Clear()
      Return RedirectToAction("Index")
    End Function
  End Class
End Namespace