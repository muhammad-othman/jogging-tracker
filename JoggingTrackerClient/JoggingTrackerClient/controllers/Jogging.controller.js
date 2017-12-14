(function() {
    'use strict';

    angular
        .module('jogging-tracker')
        .controller('JoggingController', JoggingController);

    function JoggingController($scope, $location, $window, $http, $rootScope, JoggingInfo, $document) {
        JoggingInfo.getUserInfo(getJogs);
        $("#datepicker").datepicker();
        $("#todatepicker").datepicker({
            onSelect: function(selectedDate) {
                $("#fromdatepicker").datepicker("option", "maxDate", selectedDate);
            }
        });
        $("#fromdatepicker").datepicker({
            onSelect: function(selectedDate) {
                $("#todatepicker").datepicker("option", "minDate", selectedDate);
            }
        });
        var filter = false;
        $scope.filter = function() {
            if ($("#fromdatepicker").datepicker('getDate') && $("#todatepicker").datepicker('getDate')) {
                filter = true;
                getJogs($scope.currentPage);
            } else {
                angular.element('#pickDates').modal('show')
            }

        }
        $scope.reset = function() {
            filter = false;
            getJogs($scope.currentPage);
        }

        function getJogs(pageIndex) {
            var url = JoggingInfo.urlBase + "jogs/?userID=" + $rootScope.user.id;
            if (pageIndex > 1)
                url += ("&pageIndex=" + pageIndex);
            else
                pageIndex = 1;
            if (filter) {
                url += ("&from=" + $("#fromdatepicker").datepicker('getDate').toLocaleDateString());
                url += ("&to=" + $("#todatepicker").datepicker('getDate').toLocaleDateString());
            }

            $http.get(url).then(function(response) {
                $scope.myJogs = response.data.jogs.map(formatJog);
                $scope.totalItems = response.data.totalCount;
                $scope.currentPage = pageIndex;
                $scope.itemsPerPage = 10;
                var url = JoggingInfo.urlBase + "jogs/report/" + $rootScope.user.id;
                $http.get(url).then(function(response) {
                    SetChart(response.data);
                    SetDistanceChart(response.data);
                }, function(response) {
                    console.log(response)
                });
            }, function(response) {
                console.log(response)
            });


        }
        var update = false;
        $scope.pageChanged = function() {
            getJogs($scope.currentPage)
        };

        $scope.showModal = function() {
            $scope.currentJog = { date: new Date().toLocaleDateString() };
            $scope.error = '';
            update = false;
            angular.element('#myModal').modal('show')

        }
        $scope.saveJog = function() {
            if (update)
                updateJog();
            else
                addJog()
        }


        function addJog() {
            $scope.error = '';
            if ($scope.currentJog.distance > 0 && $scope.currentJog.duration > 0) {
                $http.post(JoggingInfo.urlBase + "jogs", $scope.currentJog).then(function(response) {
                    getJogs($scope.currentPage);
                    angular.element('#myModal').modal('hide')
                }, (response) => $scope.error = response.error)
                $scope.currentJog = { date: new Date().toLocaleDateString() };

            } else if ($scope.currentJog.distance > 0)
                $scope.error = "Please pick the duration of your jog";
            else if ($scope.currentJog.duration > 0)
                $scope.error = "Please pick the distance of your jog";
            else
                $scope.error = "Please pick the distance and the duration of your jog";
        }
        $scope.deleteJog = function(id) {
            $http.delete(JoggingInfo.urlBase + "jogs/" + id).then(function(response) {
                getJogs($scope.currentPage);
            }, (response) => alert(response))
        }

        function updateJog() {
            if ($scope.currentJog.distance > 0 && $scope.currentJog.duration > 0) {
                $http.put(JoggingInfo.urlBase + "jogs/" + $scope.currentJog.id, $scope.currentJog).then(function(response) {
                    getJogs($scope.currentPage);
                    angular.element('#myModal').modal('hide');
                }, (response) => $scope.error = response.error)

            } else if ($scope.currentJog.distance > 0)
                $scope.error = "Please pick the duration of your jog";
            else if ($scope.currentJog.duration > 0)
                $scope.error = "Please pick the distance of your jog";
            else
                $scope.error = "Please pick the distance and the duration of your jog";
        }
        $scope.updateJog = function(jog) {

            $scope.error = '';
            angular.element('#myModal').modal('show')
            $scope.currentJog = Object.assign({}, jog);
            update = true;
            $scope.currentJog.date = new Date($scope.currentJog.date).toLocaleDateString()
        }



    }



})();


function formatJog(jog) {
    jog.date = new Date(jog.date).toDateString();
    jog.average = Math.round(((jog.distance / 1000) / (jog.duration / 60)) * 100) / 100
    return jog;
}



function SetDistanceChart(report) {
    weeklyReportDistance = [{
        key: "Total Week",
        values: report
    }];

    nv.addGraph(function() {
        var chart = nv.models.discreteBarChart()
            .x(function(d) { return new Date(d.weekstarts).toLocaleDateString() })
            .y(function(d) { return d.totalDistance })
            .staggerLabels(false)
            .tooltips(true)
            .showValues(false)
            .transitionDuration(1000).options({ height: 300 })
            .color([getRandomColor(), getRandomColor(), getRandomColor()]);


        d3.select('#weeklyReportDistance svg')
            .datum(weeklyReportDistance)
            .call(chart);


        nv.utils.windowResize(chart.update);

        return chart;
    });
}

function getRandomColor() {
    var letters = '0123456789ABCDEF';
    var color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}


function SetChart(report) {
    weeklyReport = [{
        key: "Average Week",
        values: report
    }];
    nv.addGraph(function() {
        var chart = nv.models.discreteBarChart()
            .x(function(d) { return new Date(d.weekstarts).toLocaleDateString() })
            .y(function(d) { return d.average })
            .staggerLabels(false)
            .tooltips(true)
            .showValues(false)
            .transitionDuration(1000).options({ height: 300 })
            .color([getRandomColor(), getRandomColor(), getRandomColor()]);


        d3.select('#weeklyReport svg')
            .datum(weeklyReport)
            .call(chart);


        nv.utils.windowResize(chart.update);

        return chart;
    });
}