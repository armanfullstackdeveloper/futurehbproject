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

     owl1= $('.owl-carousel').owlCarousel({
         rtl: true,
         loop: false,
         margin: 10,
         center:true,
         nav: false,
          responsive: {
             0: {
                 items: 1
             },
             600: {
                 items: 3
             },
             1000: {
                 items: 5
             }
         }
     })
      

     $(".prev1").click(function () {
         owl1.trigger('prev.owl.carousel');
     });

     $(".next1").click(function () {
         owl1.trigger('next.owl.carousel');
     });


 }]);




