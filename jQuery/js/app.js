var appSettings = {
	WebServiceToken: "YourWebServiceTokenHere",
	serviceUrls: {
		base:"https://preview.smartbutton.com/WS/",
		memberSecurity:"MemberSecurity.asmx",
		member:"Member.asmx",
		offer:"Offer.asmx"
	},
	currentAccountId:null,
	locationExternalReference:"PAW",
	getServiceUrl: function(serviceCategory, serviceFunction) {
		return appSettings.serviceUrls.base + 
		appSettings.serviceUrls[serviceCategory] +
		"/" + serviceFunction;
	}
};

$(function(){
	$("#submit_login").unbind().click(function(){
		login();
	});
});

function login() {
	var user = {
		username:$("#uname").val(),
		password:$("#pword").val()
	};
	if((user.username && user.username !== "") && (user.password && user.password !== "")) {
		performLogin(user);
	}
}

function performLogin(userobj) {
	
	var data = {
		WSSecurityToken:appSettings.WebServiceToken,
		MemberAccountId:userobj.username,
		SBInternalMemberId:-1,
		MemberPassword:userobj.password
	};

	$.ajax({
		type: "POST",
		url: appSettings.getServiceUrl("memberSecurity", "ValidatePassword"), 
		data: JSON.stringify(data),
		contentType:"application/json",
		success: function(result, textStatus, jqXHR) {
			if(result.d.ReturnCode === 0) {
				checkMemberStatus(userobj.username, function(isValid) {
					if(isValid) { 
						appSettings.currentAccountId = userobj.username;
						goMemberModule();
					};
				});
			}
		}
	});
}

function checkMemberStatus(accountId, callback) {
	$.ajax({
		type:"POST",
		url:appSettings.getServiceUrl("member", "FetchMemberState"),
		contentType:"application/json",
		data:JSON.stringify({ 
			WSSecurityToken:appSettings.WebServiceToken,
			MemberAccountId:accountId,
			SBInternalMemberId:-1
		}),
		success:function(result, textStatus, jqXHR) {
			if(result && result.d) {
				if(result.d.ReturnCode === 0 && result.d.MemberIsSuspended === 0) {
					callback(true);
				} else {
					callback(false);
				}
			} else {
				callback(false);
			}
		},
		error: function(result) {
			callback(false);
		}
	});
}

function goMemberModule() {
	$('.module.login').slideUp("fast", function(){
		$('.module.member-details').slideDown("fast");
		loadMemberRewards();
	})
}

function loadMemberRewards() {
	$.ajax({
		type:"POST",
		url:appSettings.getServiceUrl("offer", "MemberActiveRewards"),
		contentType:"application/json",
		data:JSON.stringify({ 
			WSSecurityToken:appSettings.WebServiceToken,
			MemberAccountId:appSettings.currentAccountId,
			SBInternalMemberId:-1,
			LocationExternalReference:appSettings.locationExternalReference
		}),
		success:function(result, textStatus, jqXHR) {
			if(result && result.d) {
				if(result.d.ReturnCode === 0) {
					if(result.d.MemberActiveRewards) {
						for(var i = 0; i < result.d.MemberActiveRewards.length; i++) {
							var rewardRow = "<tr>";
							rewardRow += "<td>" + result.d.MemberActiveRewards[i].RewardName + "</td>";
							rewardRow += "<td>" + result.d.MemberActiveRewards[i].RewardBarcode + "</td>";
							rewardRow += "<td>" + result.d.MemberActiveRewards[i].RewardValue + "</td>";
							rewardRow += "</tr>";
							$(".module.member-details .reward-list tbody").append(rewardRow);
						}
					}
				} else {
					console.error("There was an error getting rewards.");
				}
			} else {
				console.error("There was an error getting rewards. No data was returned from the server.");
			}
		},
		error: function(jqXHR, textStatus, errorMessage) {
			console.error("There was an error getting rewards. Error: " + errorMessage);
		}
	});
}