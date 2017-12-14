var app = angular.module('jogging-tracker', ["ngRoute", "ui.bootstrap"]);


app.config(function($routeProvider, $locationProvider) {
    $locationProvider.hashPrefix('');
    'use strict';

    $routeProvider.when("/login", {
        templateUrl: "views/login.html",
        controller: "LoginController"
    }).when("/jogging", {
        templateUrl: "views/jogging.html",
        controller: "JoggingController"
    }).when("/register", {
        templateUrl: "views/register.html",
        controller: "RegisterController"
    }).when("/users", {
        templateUrl: "views/users.html",
        controller: "UsersController"
    }).when("/alljogs", {
        templateUrl: "views/alljogs.html",
        controller: "AllJogsController"
    }).when("/logout", {
        templateUrl: "views/login.html",
        controller: "LogoutController"
    })
    $routeProvider.otherwise({
        redirectTo: "jogging"
    });
    $locationProvider.html5Mode(true);
})



app.controller('LogoutController', LogoutController);

function LogoutController($http, $location, $window, $rootScope) {
    $http.post('logout').then(function() {
        $rootScope.user = null;
        $window.localStorage.clear();
        $location.url('/login');
    }, function() {
        $rootScope.user = null;
        $window.localStorage.clear();
        $location.url('/login');
    });
}



app.factory('JoggingInfo', function($http, $location, $window, $rootScope) {

    var JoggingInfo = {};
    JoggingInfo.urlBase = "http://localhost:4218/api/";

    JoggingInfo.getUserInfo = function(success) {
        var token = $window.localStorage.getItem('access-token');
        if ($rootScope.user) {
            success();
            return;
        }
        if (!token) {
            $rootScope.user = null;
            $window.localStorage.clear();
            $location.url('/login');
            return JoggingInfo;
        }
        $http.get(JoggingInfo.urlBase + 'userinfo')
            .then(function(response) {
                $rootScope.user = response.data;
                success();
                $location.url('/jogging');
            }, function() { $window.localStorage.clear(); });
    }
    return JoggingInfo;
});

app.factory('errorInterceptor', ['$q', '$rootScope', '$location', '$window',
    function($q, $rootScope, $location, $window) {
        return {
            request: function(config) {
                var token = $window.localStorage.getItem('access-token');
                config.headers = { 'access-token': token, 'Content-Type': 'application/json' };
                return config || $q.when(config);
            },
            requestError: function(request) {
                return $q.reject(request);
            },
            response: function(response) {
                return response || $q.when(response);
            },
            responseError: function(response) {
                if (response && response.status === 401) {
                    $window.localStorage.clear();
                    $rootScope.user = null;
                    $location.url('/login');
                }
                if (response && response.status >= 500) {
                    console.log(response);
                }
                return $q.reject(response);
            }
        };
    }
]).config(['$httpProvider', function($httpProvider) {
    $httpProvider.interceptors.push('errorInterceptor');
}]);




app.factory('httpService', function($http, $window) {

    var urlBase = "http://localhost:4218/";
    var httpService = {};
    var token = $window.localStorage.getItem('access-token');

    httpService.get = function(requestedUrl) {
        var token = $window.localStorage.getItem('access-token');
        var req = { method: 'GET', url: urlBase + requestedUrl, headers: { 'access-token': token, 'Content-Type': 'application/json' } }
        return $http(req);
    };

    httpService.delete = function(requestedUrl) {
        var token = $window.localStorage.getItem('access-token');
        var req = { method: 'DELETE', url: urlBase + requestedUrl, headers: { 'access-token': token } }
        return $http(req);
    };

    httpService.post = function(requestedUrl, sendData) {
        var token = $window.localStorage.getItem('access-token');
        var req = { method: 'POST', url: urlBase + requestedUrl, data: sendData, headers: { 'access-token': token } }
        return $http(req);
    };

    httpService.put = function(requestedUrl, sendData) {
        var token = $window.localStorage.getItem('access-token');
        var req = { method: 'PUT', url: urlBase + requestedUrl, data: sendData, headers: { 'access-token': token } }
        return $http(req);
    };
    return httpService;
});