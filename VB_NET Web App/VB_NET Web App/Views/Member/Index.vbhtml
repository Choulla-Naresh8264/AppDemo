@ModelType MemberVM
@Code
  ViewData("Title") = "Index"
  Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<h2>Account Details</h2>

<h3>Rewards</h3>

<table border="1" cellpadding="10">
  <thead>
    <tr>
      <td>Name</td>
      <td>Barcode</td>
      <td>Value</td>
    </tr>
  </thead>
  <tbody>
    @For Each Reward In Model.Rewards
      @<tr>
        <td>@Reward.RewardName</td>
        <td>@Reward.RewardBarcode</td>
        <td>@Reward.RewardValue</td>
      </tr>
    Next Reward
  </tbody>
</table>