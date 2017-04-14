angular.module('Home', [])

 .controller('HomeCtrl', ['$scope', '$timeout', function ($scope, $timeout) {

     $scope.noProductPic = "Img/MainPage/NoProductPic.png";
     $scope.noStorePic = "Img/MainPage/NoStorePic.png";
     
     var root = "http://hoojibooji.com/",
       send = '',
       verifyLevel = 0,
       owl1, owl2, owl3, owl4, owl5;


     $scope.showRegisterBox = function () {
         $('.loginBox').hide();

         $('.popUpMadule').fadeIn();
         $('.signUpBox').slideDown();
     }

     $scope.showLoginBox = function () {
         $('.signUpBox').hide();

         $('.popUpMadule').fadeIn();
         $('.loginBox').slideDown();
     }

     $scope.closeBox = function () {
         $('.popUpMadule').fadeOut();
         $('.loginBox').slideUp();
         $('.signUpBox').slideUp();

     }

     loadMenu();
     loadCustomBox();
     loadInitProduct(1);
     loadInitProduct(3);
     loadInitProduct(5);
     loadInitStore(4);

     function loadMenu() {

         $.ajax({
             type: "GET",
             url: "/api/firstPage/getMenue",
             success: function (result) {
                 if (result.Success != true) {
                     console.log("ارتباط با سرور برقرار نشد ، لطفا مدتی بعد دوباره امتحان نمایید");
                     return;
                 }
                 if (result.Response.length > 0) {
                     $timeout(function () {
                         $scope.allMenu = result.Response;
                     }, 1);
                 }
             },
             error: function () {
                 console.log("ارتباط با سرور برقرار نشد ، لطفا مدتی بعد دوباره امتحان نمایید");
             }
         });

     }

     function loadInitProduct(type) {

         $.ajax({
             type: "POST",
             url: "/api/product/search",
             data: {
                 sortBy: type,
                 PageNumber: 1,
                 RowsPage: 10
             },
             success: function (result) {

                 if (result.Success != true) {
                     console.log("ارتباط با سرور برقرار نشد ، لطفا مدتی بعد دوباره امتحان نمایید");
                     return;
                 }
                 if (result.Response.ProductsSummery.length > 0) {
                     $('.owlSlider' + type).html('');

                     $.each(result.Response.ProductsSummery, function (index, value) {
                         $('.owlSlider' + type).append(createProductHtml(value));
                     });

                     if (type == 1) {
                         initOwl(type, owl1, 'product')
                     }
                     else if (type == 3) {
                         initOwl(type, owl3, 'product')
                     }
                     else if (type == 5) {
                         initOwl(type, owl5, 'product')
                     }

                 }
             },
             error: function () {
                 console.log("ارتباط با سرور برقرار نشد ، لطفا مدتی بعد دوباره امتحان نمایید");
             }
         });
     }

     function loadInitStore(type) {
         $.ajax({
             type: "GET",
             url: "/api/firstPage/GetNewestStore",
             data: {
                 PageNumber: 1,
                 RowsPage: 10
             },
             success: function (result) {
                 if (result.Success != true) {
                     console.log("ارتباط با سرور برقرار نشد ، لطفا مدتی بعد دوباره امتحان نمایید");
                     return;
                 }
                 if (result.Response.length > 0) {
                     $('.owlSlider' + type).html('');
                     $.each(result.Response, function (index, value) {
                         $('.owlSlider' + type).append(createStoreHtml(value));
                     });
                     initOwl(type, owl4, 'store')
                 }
             },
             error: function () {
                 console.log("ارتباط با سرور برقرار نشد ، لطفا مدتی بعد دوباره امتحان نمایید");
             }
         });
     }

     function loadNewProduct(event, type) {
         var PageNumber = 1;

         if (event && event.item) {
             count = event.item.count;
             PageNumber = parseInt((count * 1) / 10) + 1;
         }
         $.ajax({
             type: "POST",
             url: "/api/product/search",
             data: {
                 sortBy: type,
                 PageNumber: PageNumber,
                 RowsPage: 10
             },
             success: function (result) {
                 $scope.loading = false;
                 if (result.Success != true) {
                     console.log("ارتباط با سرور برقرار نشد ، لطفا مدتی بعد دوباره امتحان نمایید");
                     return;
                 }
                 setTimeout(function (index, value) {
                     if (result.Response.ProductsSummery.length > 0) {
                         $.each(result.Response.ProductsSummery, function (index, value) {
                             event.relatedTarget.add(createProductHtml(value))
                         })
                         event.relatedTarget.update();
                     }
                 }, 100)

             },
             error: function () {
                 $scope.loading = false;
                 console.log("ارتباط با سرور برقرار نشد ، لطفا مدتی بعد دوباره امتحان نمایید");
             }
         });
     }

     function loadNewStore(event, type) {
         var PageNumber = 1;

         if (event && event.item) {
             count = event.item.count;
             PageNumber = parseInt((count * 1) / 10) + 1;
         }

         $.ajax({
             type: "GET",
             url: "/api/firstPage/GetNewestStore",
             data: {
                 PageNumber: PageNumber,
                 RowsPage: 10
             },
             success: function (result) {
                 $scope.loading = false;
                 if (result.Success != true) {
                     console.log("ارتباط با سرور برقرار نشد ، لطفا مدتی بعد دوباره امتحان نمایید");
                     return;
                 }
                 setTimeout(function (index, value) {
                     if (result.Response.length > 0) {
                         $.each(result.Response, function (index, value) {
                             event.relatedTarget.add(createStoreHtml(value));
                         });
                         event.relatedTarget.update();
                     }
                 }, 100)


             },
             error: function () {
                 $scope.loading = false;
                 console.log("ارتباط با سرور برقرار نشد ، لطفا مدتی بعد دوباره امتحان نمایید");
             }
         });
     }

     function createProductHtml(value) {
         if (value.ImgAddress == null) value.ImgAddress = $scope.noProductPic;
         var PriceTemp = value.Price + '';
         PriceTemp = PriceTemp.replace(/,/g, '').replace(/(\d)(?=(\d{3})+$)/g, '$1,');


         var html = '<a href="/product/' + value.Id + '/' + value.StoreName + '/' + value.Name + '/قیمت' + PriceTemp + 'تومان" > <div class="borderRight item">'
                + ' <div class="organizer standardVerticalMargin">'
                + '<div class="circleImageContainer">'
                + " <img src='" + root + value.ImgAddress + "' alt='"+value.StoreName + '/'+ value.Name + "/قیمت" + PriceTemp + "' class='imageInMiddle'>"
                + ' </div></div>'
                + ' <div class="organizer pinkColor">' + value.Name + '</div>'
                + ' <div class="organizer smallExplain">قیمت' + PriceTemp + 'تومان ' + '</div>'
                + ' </div></a>';
         return html;
     }

     function createStoreHtml(value) {
         if (value.LogoAddress == null) value.LogoAddress = $scope.noStorePic;
         var storeType = 'فروشگاه مجازی'
         if (value && value.CityName && value.CityName != '-') {
             storeType = 'شهر : ' + value.CityName;
         }
         var html = '<a href="/shop/code/' + value.Id + '"> <div class="borderRight item">'
                        + ' <div class="organizer standardVerticalMargin">'
                        + '<div class="circleImageContainer">'
                        + " <img src='" + root + value.LogoAddress + "' alt='" + value.Name + "," + storeType + "' class='imageInMiddle'>"
                        + ' </div></div>'
                        + ' <div class="organizer pinkColor">' + value.Name + '</div>'
                        + ' <div class="organizer smallExplain"> ' + storeType + '</div>'
                        + ' </div></a>';
         return html;
     }

     function initOwl(type, owl, kind) {
         owl = $('.owlSlider' + type).owlCarousel({
             rtl: true,
             loop: false,
             margin: 10,
             center: false,
             nav: false,
             items: 5,
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
         });
         $(".prev" + type).click(function () {
             owl.trigger('prev.owl.carousel');
         });
         $(".next" + type).click(function () {
             owl.trigger('next.owl.carousel');
         });
         owl.on('changed.owl.carousel', function (event) {
             if (!event.item || (event.item.index + event.page.size + 4 >= event.item.count)) {
                 if ($scope.loading) return;
                 $scope.loading = true;
                 if (kind == 'product') {
                     loadNewProduct(event, type);
                 } else if (kind == 'store') {
                     loadNewStore(event, type);
                 }
             }
         });
     }

     function loadCustomBox() {
         $.ajax({
             type: "GET",
             url: "/api/firstPage/GetActiveBox",
             data: {
                 position: 'slider'
             },
             success: function (result) {
                 if (result.Success != true) {
                     console.log("ارتباط با سرور برقرار نشد ، لطفا مدتی بعد دوباره امتحان نمایید");
                     return;
                 }

                 $timeout(function () {
                     $scope.Box = result.Response;
                     $timeout(function () {
                         showSlider(2);
                         showBoxs();
                     }, 500)
                 }, 1)




             },
             error: function () {
                 $scope.loading = false;
                 console.log("ارتباط با سرور برقرار نشد ، لطفا مدتی بعد دوباره امتحان نمایید");
             }
         });
     }

     function showSlider(type) {
         owl2 = $('.owlSlider' + type + '').owlCarousel({
             rtl: true,
             loop: true,
             nav: false,
             items: 1,
             autoplay: true,
             autoplayTimeout: 2000,
             autoplayHoverPause: true
         });
         $(".prev" + type).click(function () {
             owl2.trigger('prev.owl.carousel');
         });
         $(".next" + type).click(function () {
             owl2.trigger('next.owl.carousel');
         });
     }

     function showBoxs() {
         $.ajax({
             type: "GET",
             url: "/api/firstPage/GetActiveBox",
             data: {
                 position: 'category'
             },
             success: function (result) {
                 if (result.Success != true) {
                     console.log("ارتباط با سرور برقرار نشد ، لطفا مدتی بعد دوباره امتحان نمایید");
                     return;
                 }
                 $scope.Box2 = result.Response;
             },
             error: function () {
                 console.log("ارتباط با سرور برقرار نشد ، لطفا مدتی بعد دوباره امتحان نمایید");
             }
         });

     }



     //When page load , check search box typing
     $(function () {
         //setup before functions
         var typingTimer; //timer identifier
         var doneTypingInterval = 500; //time in ms, 1 second for example
         var $input = $('.SearchTextBox');

         //on keyup, start the countdown
         $input.on('keyup', function () {

             if ($('.SearchTextBox').val().length <= 1) {
                 $(".Search_Task_Open").slideUp("slow", function () {
                     $('.searchField').removeClass('searchActivate');
                 });

             }

             clearTimeout(typingTimer);
             typingTimer = setTimeout(ajaxSearch, doneTypingInterval);
         });

         //on keydown, clear the countdown
         $input.on('keydown', function () {
             clearTimeout(typingTimer);
         });

         $("#loginFrm input").keypress(function (e) {
             if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
                 $('#EnterBtn').click();
                 return false;
             } else {
                 return true;
             }
         });

     });

     //if click outside of search result , hide search result
     $(document).click(function (event) {
         if (!$(event.target).closest('.Search_Task_Open').length) {
             $(".Search_Task_Open").slideUp("slow", function () {
                 $('.searchField').removeClass('searchActivate');
             });
         }
     });

     //When typing done , Product and Store Load
     function ajaxSearch() {
         if ($('.SearchTextBox').val().length >= 2) {

             //$('#SearchInProductHolder').html("");
             //$('#SearchInStoreHolder').html("");
             //$('#LoadProduct').show();
             //$('#LoadStore').show();
             console.log('1')
             $(".Search_Task_Open").slideDown("slow");
             $('.searchField').addClass('searchActivate');

             $scope.searchResult = [];
             $.ajax({
                 type: "GET",
                 url: "/api/firstPage/TopSearchSummeray/",
                 data: {
                     name: $(".SearchTextBox").val()
                 },
                 success: function (result) {
                     $timeout(function () {
                         $scope.searchResult = result.Response;
                     });
                 }

             });
         }

     };

     //Login
     $('#EnterBtn').on("click", function () {

         var form = $('#__AjaxAntiForgeryForm');
         var token = $('input[name="__RequestVerificationToken"]', form).val();

         $.ajax({
             url: '/Account/Login',
             type: 'POST',
             dataType: "json",
             data: {
                 __RequestVerificationToken: token,
                 UserName: $('#UserName').val(),
                 Password: $.md5($('#Password').val())
             },
             success: function (result) {

                 if (result.Success == true) {

                     //success
                     if (returnUrl == "") {
                         //return to root
                         window.location = "/";

                     } else {
                         //return to returnUrl
                         window.location = returnUrl;
                     }

                 } else {
                     //failed
                 }
             },
             error: function () {
                 //failed
             }

         });


     });

     //register
     $('#register').on("click", function () {
         $('#registerStatus').html('');
         if (send != '') return;


         if ($('#UserNameTxt').val().length < 11) {
             $('#registerStatus').html('لطفا شماره تلفن صحیح خود را با 09 وارد نمایید');
             return;
         }
         else if ($('#PasswordTxt').val().length < 6) {
             $('#registerStatus').html('لطفا کلمه عبور خود را بیشتر از 6 کارکتر وارد نمایید');
             return;
         }
         else if ($('#ConfirmPasswordTxt').val().length < 6) {
             $('#registerStatus').html('لطفا تکرا کلمه عبور را مشابه کلمه عبور وارد نمایید');
             return;
         }
         else if (confPass != pass) {
             $('#registerStatus').html('لطفا تکرا کلمه عبور را مشابه کلمه عبور وارد نمایید');
             return;
         }
         $('#Alert_UserName_Wait').css("display", "block");
         send = 1;

         if (verifyLevel == 0) {
             $.ajax({
                 type: "GET",
                 url: "/Api/sms/register",
                 data: { phoneNumber: $('#UserNameTxt').val() },
                 success: function (result) {
                     send = '';
                     if (result.Success == false) {
                         $('#registerStatus').html(result.Response);
                         $('#Alert_UserName_Wait').css("display", "none");

                     }
                     else {
                         $('#Alert_UserName_Wait').css("display", "none");
                         $('#Alert_Verify_Send').slideToggle();
                         $('#Verify_Code').slideToggle();
                         verifyLevel = 1;

                     }
                 },
                 error: function () {
                     console.log("ارتباط با سرور برقرار نشد ، لطفا مدتی بعد دوباره امتحان نمایید");
                 }
             });
         } else {

             //  $('#registerStatus').html('لطفا کمی صبر نمایید');

             TempData = {
                 "phoneNumber": $('#UserNameTxt').val(),
                 "Password": $.md5($('#PasswordTxt').val()),
                 "ConfirmPassword": $.md5($('#ConfirmPasswordTxt').val()),
                 "VerificationCode": $('#VerifyTxt').val()

             };

             $.ajax({
                 url: '/Api/account/userregister',
                 type: 'POST',
                 dataType: "json",
                 contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                 data: $.param(TempData),
                 success: function (result) {

                     if (result.Success == true) {
                         SaveToUser()

                     } else {
                         $('#Alert_UserName_Wait').css("display", "none");
                         $('#registerStatus').html('ثبت نام شما با خطا مواجه شد');
                         send = "";
                     }
                 },
                 error: function () {
                     $('#registerStatus').html("ارتباط با سرور برقرار نشد ، لطفا مدتی بعد دوباره امتحان نمایید");
                     send = "";
                 }
             });
         }
     })

     function SaveToUser() {

         var form = $('#__AjaxAntiForgeryForm');
         var token = $('input[name="__RequestVerificationToken"]', form).val();

         TempData = {
             "__RequestVerificationToken": token,
             "UserName": $('#UserNameTxt').val(),
             "Password": $.md5($('#PasswordTxt').val()),
             "ConfirmPassword": $.md5($('#PasswordTxt').val())
         };

         $.ajax({
             url: '/Account/RegisterMember',
             type: 'POST',
             dataType: "json",
             contentType: "application/x-www-form-urlencoded; charset=UTF-8",
             data: $.param(TempData),
             success: function (result) {
                 $('#Alert_UserName_Wait').css("display", "none");

                 if (result.Success == true) {
                     $('#registerStatus').html('شما با موفقیت ثبت نام شدید');

                     //return to root
                     window.location = "/";
                     // window.location = "/Member/Panel";
                 } else {
                     $('#registerStatus').html('ثبت نام شما با خطا مواجه شد');
                     //location.reload();
                     send = "";
                 }
                 //console.log("فروشگاه شما با موفقیت ثبت شد");
             },
             error: function () {
                 $('#registerStatus').html("ارتباط با سرور برقرار نشد ، لطفا مدتی بعد دوباره امتحان نمایید");
                 send = "";
             }
         });

     }

 }]);




