angular.module('Home', [])

 .controller('HomeCtrl', ['$scope', function ($scope) {


     $scope.showRegisterBox = function () {
         $('.popUpMadule').fadeIn();
         $('.signUpBox').slideDown();
     }

     $scope.showLoginBox = function () {
         $('.popUpMadule').fadeIn();
         $('.loginBox').slideDown();
     }

     $scope.closeBox = function () {
         $('.popUpMadule').fadeOut();
         $('.loginBox').slideUp();
         $('.signUpBox').slideUp();

     }




 }]);




