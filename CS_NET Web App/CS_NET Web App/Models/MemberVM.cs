using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CS_NET_Web_App.Models
{
  public class MemberVM
  {
    public List<SBOfferSvc.MemberActiveRewardItem> Rewards { get; set; }
  }
}