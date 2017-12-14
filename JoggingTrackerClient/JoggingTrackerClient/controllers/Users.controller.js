(function() {
    'use strict';

    angular
        .module('jogging-tracker')
        .controller('UsersController', UsersController);

    function UsersController($scope, $location, $window, $http, $rootScope, JoggingInfo, $document) {
        JoggingInfo.getUserInfo(getPermissions);
        $("#datepicker").datepicker();

        function getPermissions() {
            $http.get(JoggingInfo.urlBase + 'permissions')
                .then((r) => {
                    $scope.permissions = r.data;
                    getUsers()
                }, (r) => console.log(r))
        }

        function getUsers(pageIndex, from, to) {
            var url = JoggingInfo.urlBase + "users";
            if (pageIndex > 1)
                url += ("?pageIndex=" + pageIndex);
            else
                pageIndex = 1;
            if (from && to)
                console.log(from);

            $http.get(url).then(function(response) {
                $scope.myUsers = response.data.users.map(formatUser);
                $scope.totalItems = response.data.totalCount;
                $scope.currentPage = pageIndex;
                $scope.itemsPerPage = 10;
            }, function(response) {
                console.log(response)
            });
        }

        function formatUser(user) {
            console.log(user)
            console.log($scope.permissions)
            user.permissionName = $scope.permissions.find(e=>e.permission == user.permission).permissionName;
            return user;
        }

        $scope.pageChanged = function() {
            getUsers($scope.currentPage)
        };
        $scope.showModal = function() {
            $scope.currentUser = { date: new Date().toLocaleDateString() };
            $scope.error = '';
            angular.element('#myModal').modal('show')

        }


        $scope.deleteUser = function(id) {
            $http.delete(JoggingInfo.urlBase + "users/" + id).then(function(response) {
                console.log(response);
                getUsers($scope.currentPage);
            }, (response) => console.log(response))
        }

        $scope.saveUser= function() {
            console.log('fuser')
                console.log($scope.currentUser)
            if ($scope.currentUser.age > 9 && $scope.currentUser.userName.length > 4 && validateEmail($scope.currentUser.email)) {
                angular.element('#myModal').modal('hide');
                $scope.currentUser.permission = $scope.permissions.filter(e=>e.permissionName == $scope.currentUser.permissionName)[0].permission
                $http.put(JoggingInfo.urlBase + "users/" + $scope.currentUser.id, $scope.currentUser).then(function(response) {
                    getUsers($scope.currentPage);
                }, (response) => {$scope.error = response.error;})

            }
            else
                $scope.error = "Please Check the user's info";
        }
        $scope.updateUser = function(user) {

            $scope.error = '';
            angular.element('#myModal').modal('show')
            $scope.currentUser = Object.assign({}, user);
        }
            



    }



})();
function validateEmail(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}