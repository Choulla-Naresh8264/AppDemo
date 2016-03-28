angular.module("sbLoginModule",[])

	// =========== Controllers =============
	.controller("sbMainController", ["$scope", "webServiceFactory", function($scope, webServiceInvoker) {
		$scope.panelState = { selectedPanel: 'login', currentUserId: null };
		$scope.member = {};
		$scope.AccountId = null;
		$scope.getMemberRewards = function() {
			webServiceInvoker.callOfferService("MemberActiveRewards", {
				WSSecurityToken:null,
				MemberAccountId:$scope.panelState.currentUserId,
				SBInternalMemberId:-1,
				LocationExternalReference:null
			}, function(result, status) {
				console.log(result);
				if(result.data && result.data.d && result.data.d.MemberActiveRewards) {
					$scope.member.Rewards = result.data.d.MemberActiveRewards;
				}
			})
		}
	}])
	.controller("sbLoginController", ["$scope", "webServiceFactory", function($scope, webServiceInvoker) {
		$scope.user = {
			username:'',
			password:''
		};

		$scope.loginError = "";

		$scope.login = function() {
			webServiceInvoker.callMemberSecurityService("ValidatePassword", {
				WSSecurityToken:null,
				MemberAccountId:$scope.user.username,
				SBInternalMemberId:-1,
				MemberPassword:$scope.user.password
			}, function(result, status) {
				console.log(result);
				if(result.data.d.ReturnCode === 0) {
					webServiceInvoker.callMemberService("FetchMemberState", {
						WSSecurityToken:null,
						MemberAccountId:$scope.user.username,
						SBInternalMemberId:-1
					}, function(result, status) {
						if(result.data.d.ReturnCode === 0) {
							$scope.panelState.selectedPanel = 'member-details';
							$scope.panelState.currentUserId = $scope.user.username;
							$scope.getMemberRewards();
						}
					});
				} else {
					$scope.loginError = "There was an error logging you in. Please contact your system administrator.";
				}
			});
		}
	}])
	.controller("sbMemberController", ["$scope", function($scope) {

	}])



	// =========== Config =============
	.config(['$httpProvider', function($httpProvider) {
		$httpProvider.defaults.useXDomain = true;

		delete $httpProvider.defaults.headers.common['X-Requested-with'];
	}])


	

	// =========== Services =============
	.factory('webServiceFactory',['$http', 'webServiceBaseUrl', 'webServiceToken', 'webServiceLocationExtRef', function($http, webServiceBaseUrl, webServiceToken, wsLocationExtRef) {
		function callService(serviceUrl, data, callback) {
			if(data.WSSecurityToken === null) {
				data.WSSecurityToken = webServiceToken;
			}

			if(data.LocationExternalReference === null) {
				data.LocationExternalReference = wsLocationExtRef;
			}

			var httpConfig = {
				headers: {
					"content-type": "application/json"
				}
			};

			$http.post(serviceUrl, data, httpConfig).then(function(data, status, headers, config, statusText) {
				if(callback) {
					callback(data, status, headers, config, statusText);
				}
			});
		}

		var serviceBase

		return {
			callMemberService: function(methodName, data, callback) {
				callService(webServiceBaseUrl+"Member.asmx/"+methodName, data, callback);
			},
			callMemberSecurityService: function(methodName, data, callback) {
				callService(webServiceBaseUrl+"MemberSecurity.asmx/"+methodName, data, callback);
			},
			callOfferService: function(methodName, data, callback) {
				callService(webServiceBaseUrl+"Offer.asmx/"+methodName, data, callback);
			}
		};
	}])


	// =============== Values =================
	.value("webServiceBaseUrl", "https://app.smartbutton.com/WS/")
	.value("webServiceToken", "YourWebServiceTokenHere")
	.value("webServiceLocationExtRef", "YourLocationExternalReference");