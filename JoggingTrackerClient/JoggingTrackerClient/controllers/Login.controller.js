(function() {
    'use strict';
    angular
        .module('jogging-tracker')
        .controller('LoginController', LoginController);

    function LoginController($scope, $location, $window,$http,JoggingInfo,$rootScope) {
        JoggingInfo.getUserInfo(()=>$location.url('/jogging'));

        $scope.error = '';
        $scope.login = function(){
            $scope.error='';
            if(!$scope.user || (!$scope.user.username && !$scope.user.password))
                    $scope.error += "Please Enter your Username and Password";
            else if(!$scope.user.username)
                    $scope.error += "Please Enter your Username";
            else if(!$scope.user.password)
                    $scope.error += "Please Enter your Password";
            else
               $http.post(JoggingInfo.urlBase+"login",$scope.user).then(function(response) {
                        $window.localStorage.setItem('access-token',response.data.accessToken);
                        $window.localStorage.setItem('user-id',response.data.user.id);
                        $rootScope.user = response.data.user;
                        $location.url('/jogging');
                    },function(response) {
                        $scope.error = response.data.message;
                     });
        }
    }
})();
