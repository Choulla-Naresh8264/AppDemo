@Code
  ViewData("Title") = "Login"
  Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

@ModelType LoginVM

<h2>Login</h2>

@Using Html.BeginForm()
  If Not ViewBag.ErrorMessage Is Nothing Then
    @<div style="color:red">@ViewBag.ErrorMessage</div>
  End If
  @<fieldset>
    <div>Username :  </div>
    <div>@Html.EditorFor(Function(Model) Model.Username)</div>
    <div>Password :  </div>
    <div>@Html.PasswordFor(Function(Model) Model.Password)</div>
    <div><button type="submit">Login</button></div>
  </fieldset>
End Using

<style>
  fieldset div {
    margin-bottom:1em;
  }
</style>