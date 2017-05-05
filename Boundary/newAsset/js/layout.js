angular.module('Home', [])

 .controller('HomeCtrl', ['$scope', '$timeout', function ($scope, $timeout) {
  
      
     var send = '';
     var verifyLevel = 0;
     var navCounter = 0;

     loadMenu(); 

     $scope.showRegisterBox = function () {
         $('.loginBox').hide();

         $('.popUpMadule').fadeIn();
         $('.signUpBox').fadeIn();
     }

     $scope.showLoginBox = function () {
         $('.signUpBox').hide();

         $('.popUpMadule').fadeIn();
         $('.loginBox').fadeIn();
     }

     $scope.closeBox = function () {
         $('.popUpMadule').fadeOut();
         $('.loginBox').fadeOut();
         $('.signUpBox').fadeOut();

     }

     $('.navigation').on('click', function () {
         if (navCounter == 0) {
             navCounter = 1;
             $('#line1').addClass('topLine');
             $('#line3').addClass('bottomLine');
             $('.outside').show();
             $('.navigationOpen ').slideDown();

         } else {
             navCounter = 0;
             $('#line1').removeClass('topLine');
             $('#line3').removeClass('bottomLine');
             $('.outside').hide();
             $('.navigationOpen ').slideUp();
         }

     })

     $('.outside').on("click", function () {
         navCounter = 0;
         $('#line1').removeClass('topLine');
         $('#line3').removeClass('bottomLine');
         $('.outside').hide();
         $('.navigationOpen ').slideUp();
     })

     $('#searchIconInMobile').on('click', function () {
         $('#searchBoxInMobile').slideDown();
         $('.searchField').addClass('.searchActivate')
     });

     $('#closeSearchBoxInMobile').on('click', function () {
         $('#searchBoxInMobile').slideUp();
         $('.searchField').removeClass('.searchActivate')
     })

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
                         $('.hiddenMenuInFirst').show();
                         setTimeout(function () {
                             $('.submenu').hide();
                         }, 5)

                     }, 1);
                 }
             },
             error: function () {
                 console.log("ارتباط با سرور برقرار نشد ، لطفا مدتی بعد دوباره امتحان نمایید");
             }
         });

     } 

     //When page load , check search box typing
     $(function () {

         //setup before functions
         var typingTimer1, typingTimer2; //timer identifier
         var doneTypingInterval = 500; //time in ms, 1 second for example
         var $input1 = $('#SearchTextBox1');
         var $input2 = $('#SearchTextBox2');

         //desktop
         //on keyup, start the countdown
         $input1.on('keyup', function () {

             if ($input1.val().length <= 1) {
                 $("#Search_Task_Open1").slideUp("slow", function () {
                     $('#searchField1').removeClass('searchActivate');
                 });
             }
             clearTimeout(typingTimer1);
             typingTimer1 = setTimeout(ajaxSearch1, doneTypingInterval);
         });

         //on keydown, clear the countdown
         $input1.on('keydown', function () {
             clearTimeout(typingTimer1);
         });

         //mobile 
         //on keyup, start the countdown
         $input2.on('keyup', function () {

             if ($input2.val().length <= 1) {
                 $("#Search_Task_Open2").slideUp("slow", function () {
                     $('#searchField2').removeClass('searchActivate');
                 });
             }
             clearTimeout(typingTimer2);
             typingTimer2 = setTimeout(ajaxSearch2, doneTypingInterval);
         });

         //on keydown, clear the countdown
         $input2.on('keydown', function () {
             clearTimeout(typingTimer2);
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
     function ajaxSearch1() {
         if ($('#SearchTextBox1').val().length >= 2) {

             //$('#SearchInProductHolder').html("");
             //$('#SearchInStoreHolder').html("");
             //$('#LoadProduct').show();
             //$('#LoadStore').show();
             $("#Search_Task_Open1").slideDown("slow");
             $('#searchField1').addClass('searchActivate');

             $scope.searchResult = [];
             $.ajax({
                 type: "GET",
                 url: "/api/firstPage/TopSearchSummeray/",
                 data: {
                     name: $("#SearchTextBox1").val()
                 },
                 success: function (result) {
                     $timeout(function () {
                         $scope.searchResult = result.Response;
                     });
                 }

             });
         }

     };

     //When typing done , Product and Store Load
     function ajaxSearch2() {
         if ($('#SearchTextBox2').val().length >= 2) {

             //$('#SearchInProductHolder').html("");
             //$('#SearchInStoreHolder').html("");
             //$('#LoadProduct').show();
             //$('#LoadStore').show();
             $("#Search_Task_Open2").slideDown("slow");
             $('#searchField2').addClass('searchActivate');

             $scope.searchResult = [];
             $.ajax({
                 type: "GET",
                 url: "/api/firstPage/TopSearchSummeray/",
                 data: {
                     name: $("#SearchTextBox2").val()
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
         $('#loginMessage').html('');
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

                     $scope.closeBox();
                     $('#loginMessage').html("شما با موفقیت وارد شدید");

                     //success
                     if (returnUrl == "") {
                         //return to root
                         window.location = "/";

                     } else {
                         //return to returnUrl
                         window.location = returnUrl;
                     }

                 } else {
                     $('#loginMessage').html(result.Response);
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
         else if ($('#ConfirmPasswordTxt').val() != $('#PasswordTxt').val()) {
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
                         $('.VerifyBox').slideToggle();
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
 
     $scope.showSubmenu = function (element) {

         if ($(element.currentTarget).next().is(':visible')) {
             $(element.currentTarget).next().slideUp();
             $(element.currentTarget).children().removeClass('icon-arrow-up2');
             $(element.currentTarget).children().addClass('icon-arrow-down2');
          }else{
            $('.submenu').slideUp();
            $(element.currentTarget).next().slideDown();
            $(element.currentTarget).children().removeClass('icon-arrow-down2');
            $(element.currentTarget).children().addClass('icon-arrow-up2');
         }
      
     }


 }]);




