var FSapp = angular.module('FSApp', []);

FSapp.controller('DirectoryModelCtrl', function ($scope, $http) {
    if ($scope.route == undefined)
    {
        $scope.route = 'api/directorymodel/?path=/';
    }
    $scope.GetDirectoryModel = function()
    {
        $http.get($scope.route).then(function (response) {
        $scope.currentPath = response.data.Path;
        $scope.smallFiles = response.data.SmallFilesCounter;
        $scope.mediumFiles = response.data.MediumFilesCounter;
        $scope.largeFiles = response.data.LargeFilesCounter;
        $scope.directories = response.data.Directories;
        $scope.files = response.data.Files;
    });
    }
    $scope.GetDirectoryModel();
    $scope.GoToParentDirectory = function () {
        var pathSlice = $scope.currentPath.slice(0, $scope.currentPath.lastIndexOf('/'));
        pathSlice = pathSlice.slice(0, pathSlice.lastIndexOf('/'));
        if ($scope.currentPath.indexOf('/') == $scope.currentPath.lastIndexOf('/')) {
            pathSlice = '';
        }
        $scope.route = '/api/DirectoryModel/?path=' + pathSlice + '/';
        $scope.GetDirectoryModel();
    }
    $scope.GoToChildDirectory = function (dir) {
        if ($scope.currentPath == '/')
        {
            $scope.currentPath = '';
        }
        $scope.route = '/api/DirectoryModel/?path=' + $scope.currentPath + dir + '/';
        $scope.GetDirectoryModel();
    }

});


