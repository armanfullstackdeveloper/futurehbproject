'use strict';

angular.module('post', [
        'ngRoute',
        'ngCkeditor',
        'post.Controller',
        'post.Model',
        'Pagination'
])

    .config([
       '$locationProvider', '$routeProvider', function ($locationProvider, $routeProvider) {
           //$locationProvider.html5Mode(true);
           $locationProvider.hashPrefix('!');

           $routeProvider.
               when("/", { templateUrl: "View/login.html", controller: "HomeCtrl" }).
               when("/logout", { templateUrl: "View/logout.html", controller: "HomeCtrl" }).
               when("/company-template-new", { templateUrl: "View/company-template-new.html", controller: "CompanyCtrl" }).
               when("/company-template-list", { templateUrl: "View/company-template-list.html", controller: "CompanyCtrl" }).
               when("/company-template-edit/:id", { templateUrl: "View/company-template-edit.html", controller: "CompanyCtrl" }).
               when("/company-api-new", { templateUrl: "View/company-api-new.html", controller: "CompanyCtrl" }).
               when("/company-api-list", { templateUrl: "View/company-api-list.html", controller: "CompanyCtrl" }).
               when("/company-api-edit/:id", { templateUrl: "View/company-api-edit.html", controller: "CompanyCtrl" }).
               when("/company-info-edit", { templateUrl: "View/company-info-edit.html", controller: "CompanyCtrl" }).
               when("/company-invoice-list", { templateUrl: "View/company-invoice-list.html", controller: "CompanyCtrl" }).
               when("/company-request-search", { templateUrl: "View/company-request-search.html", controller: "CompanyCtrl" }).
               when("/company-request-statistics-send", { templateUrl: "View/company-request-statistics-send.html", controller: "CompanyCtrl" }).
               when("/company-request-statistics-verified", { templateUrl: "View/company-request-statistics-verified.html", controller: "CompanyCtrl" }).
               when("/company-request-statistics-cancelled", { templateUrl: "View/company-request-statistics-cancelled.html", controller: "CompanyCtrl" }).
               when("/change-password", { templateUrl: "View/change-password.html", controller: "HomeCtrl" }).
               otherwise({ redirectTo: '/' });
       }
    ])

    .run(['$rootScope', '$location', 'Auth', 'Model', function ($rootScope, $location, Auth, Model) {
        $rootScope.$on('$routeChangeStart', function (event) {

            if (!Auth.isLoggedIn() && !Auth.infoChecked()) {
                Model.companyInfo().then(function (data) {

                    var user = '';
                    if (data.name) {
                        user = data.name
                    } else {
                        user = 'کاربر'
                    }
                    $('.firstHide').show();
                    Auth.setUser(user);
                    $rootScope.CompanyName = user;
                    $('.firstHide').show();

                    if ($location.path() != "/!#/" && $location.path() != "/") {
                        window.location.href = '/panel/#!' + $location.path();
                    }
                    else {
                        //$('.firstHide').hide();
                        //window.location.href = '/panel/#!/' ;
                        window.location.href = '#!/company-request-search';
                    }

                }, function (error) {
                    event.preventDefault();
                    $('.firstHide').hide();
                    Auth.setCheckedTrue();
                    window.location.href = '/panel/#!/';

                }).finally(function () {

                });

            }
            else {
                if ($location.path() != "/!#/" || $location.path() != "/") {
                    if (!Auth.isLoggedIn()) {
                        $('.firstHide').hide();
                        window.location.href = '/panel/#!/';
                    }
                    else {
                        $('.firstHide').show();
                        window.location.href = '#!/company-request-search';
                    }
                }
                else {
                    $('.firstHide').hide();
                }
            }
        });
    }]);


