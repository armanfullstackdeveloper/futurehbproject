'use strict';

var module = angular.module('post.Controller', []);


//-------------------------Home-------------------------


//Home Controler
module.controller('HomeCtrl', ['$scope', '$rootScope', 'Model', function ($scope, $rootScope, Model) {

    $('.firstHide').show();
    var request = false;


    $scope.addNewRequestFunction = function () {

        $('#addNewRequestSendBtn').attr('disabled', 'disabled');

        if (request == true) return;

        request = true;

        $scope.addNewRequest.success = '';
        $scope.addNewRequest.error = '';

        $scope.addNewRequest.success = 'درخواست عضویت شما با موفقیت ثبت شد ، به زودی ایمیل فعال سازی برای شما ارسال می شود';

        //Model.requestNew($scope.addNewRequest).then(function (data) {
        //         $scope.addNewRequest.success = 'درخواست عضویت شما با موفقیت ثبت شد ، به زودی ایمیل فعال سازی برای شما ارسال می شود';

        //}, function (error) {
        //    if (error.error = 'NOT_FOUND') {
        //        $scope.addNewRequest.error = 'اطلاعات وارد شده صحیح نمی باشد ، لطفا مجددا اطلاعات را با دقت وارد نمایید ';
        //    }
        //    else {
        //        $scope.addNewRequest.error = 'با عرض پوزش ، خطایی رخ داده است ، لطفا کمی بعد مجددا اقدام نمایید. ';
        //    }
        //}).finally(function () {
        //    request = false;
        //});
    }
 

    function isValidIranianNationalCode(input) {
        if (!/^\d{10}$/.test(input))
            return false;

        var check = parseInt(input[9]);
        var sum = 0;
        var i;
        for (i = 0; i < 9; ++i) {
            sum += parseInt(input[i]) * (10 - i);
        }
        sum %= 11;

        return (sum < 2 && check == sum) || (sum >= 2 && check + sum == 11);
    }

    $('#main').on('keydown', '#nationalCode , #addNewRequestNationalCode ,#addNewRequestPostalCode1,#addNewRequestPostalCode2, #confirmCode , #trackingCode1, #trackingCode2, #trackingCode3 , #trackingCode2 ,#phoneNumber ,#phoneNumber2 , #postalCode1, #postalCode2', function (e) {
        -1 !== $.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) || /65|67|86|88/.test(e.keyCode) && (!0 === e.ctrlKey || !0 === e.metaKey) || 37 == e.keyCode || 39 == e.keyCode || (e.shiftKey || 48 > e.keyCode || 57 < e.keyCode) && (96 > e.keyCode || 105 < e.keyCode) && e.preventDefault();
    });

    $('.js-example-basic-multiple-limit').select2({
        maximumSelectionLength: 1,
        //allowClear: true
        placeholder: "طبقه",
        dir: "rtl",
        language: "fa"
    });

    var myLanguage = {
        requiredFields: 'لطفا همه های اجباری را پر نمایید',
        requiredField: 'لطفا این فیلد را پر نمایید',
        badEmail: 'لطفا آدرس ایمیل را به درستی وارد نمایید',
        lengthBadStart: 'لطفا  ',
        lengthBadEnd: ' کاراکتر وارد نمایید',
        lengthTooLongStart: 'لطفا حداکثر ',
        lengthTooShortStart: 'لطفا حداقل ',
        notConfirmed: 'مقدار وارد شده درست نمی باشد',
        badCustomVal: 'مقدار وارد شده صحیح نمی باشد',
        andSpaces: ' and spaces ',
        badInt: 'لطفا در این فیلد فقط از اعداد استفاده نمایید',
        badNumberOfSelectedOptionsStart: 'لطفا حداقل  ',
        badNumberOfSelectedOptionsEnd: ' گزینه را انتخاب نمایید',
        badAlphaNumeric: 'لطفا در این فیلد فقط از اعداد و حروف استفاده نمایید ',
        badAlphaNumericExtra: ' and ',
        groupCheckedRangeStart: 'شما باید  ',
        groupCheckedTooFewStart: 'شما باید حداقل ',
        groupCheckedTooManyStart: 'شما باید حداکثر ',
        groupCheckedEnd: ' مورد را انتخاب نمایید',
        min: 'حداقل',
        max: 'حداکثر'
    };

    $.validate({
        language: myLanguage,
        form: '#addNewRequestForm',
        onSuccess: function () {

        },
        scrollToTopOnError: true
    });
  
}]);




//----------------------------translate------------------------------------

module.controller('LanguageSwitchController', ['$scope', '$rootScope', '$translate',
      function ($scope, $rootScope, $translate) {
          $scope.changeLanguage = function (langKey) {
              $translate.use(langKey);
          };

          $rootScope.$on('$translateChangeSuccess', function (event, data) {
              var language = data.language;


              $translate(['font', 'direction', 'float', 'h1', 'h2', 'h3', 'h4', 'h5', 'h6']).then(function (translations) {

                  $rootScope.lang = language;

                  //  $rootScope.default_direction = language === 'fa' ? 'rtl' : 'ltr';
                  //    $rootScope.opposite_direction = language === 'fa' ? 'ltr' : 'rtl';

                  //  $rootScope.default_float = language === 'fa' ? 'right' : 'left';
                  // $rootScope.opposite_float = language === 'fa' ? 'left' : 'right';

                  $rootScope.font = translations.font + '';
                  $rootScope.direction = translations.direction + '';
                  $rootScope.float = translations.float + '';
                  if (translations.direction == 'rtl') {
                      $rootScope.mirrorDirection = 'ltr';
                  } else {
                      $rootScope.mirrorDirection = 'rtl';
                  }
                  if (translations.float == 'right') {
                      $rootScope.mirrorFloat = 'left';
                  } else {
                      $rootScope.mirrorFloat = 'right';
                  }
                  $rootScope.h1 = translations.h1;
                  $rootScope.h2 = translations.h2;
                  $rootScope.h3 = translations.h3;
                  $rootScope.h4 = translations.h4;
                  $rootScope.h5 = translations.h5;
                  $rootScope.h6 = translations.h6;

                  if (translations.float == 'right') {
                      $('input').css({ 'text-align': 'right' })
                  } else {
                      $('input').css({ 'text-align': 'left' })
                  }


              }, function (translationIds) {

              });




          });
      }]);


























//$scope.initFunction = function () {

//    Model.stateList().then(function (data) {

//        $scope.state = data.data;
//        Model.passTypeList().then(function (data) {

//            $scope.pass = data.data;
//            Model.companyRequestTypeList().then(function (data) {

//                $scope.sendType = data;
//                Model.districtList().then(function (data) {
//                    $scope.district = data.data;

//                }, function (error) {
//                }).finally(function () {
//                    request = false;
//                });
//            }, function (error) {
//            }).finally(function () {
//                request = false;
//            });

//        }, function (error) {
//        }).finally(function () {
//            //$(".form-control").prop("selectedIndex", 1);
//        });


//    }, function (error) {
//    }).finally(function () {
//    });

//}

//$("#addNewRequestState").on("change", function () {

//    Model.cityList($("#addNewRequestState option:selected").val()).then(function (data) {
//        $scope.city = data.data;
//    }, function (error) {

//    }).finally(function () {

//    });

//});

//$scope.verifyRequestFunction = function (value) {

//    if (request == true) return;
//    request = true;

//    $scope.verify.success = '';
//    $scope.verify.error = '';

//    Model.rquestVerify(value).then(function (data) {
//        $scope.verify.success = 'تایید نشانی شما با موفقیت انجام شد  ، با تشکر از همکاری شما';
//    }, function (error) {
//        if (error.error = 'NOT_FOUND') {
//            $scope.verify.error = 'اطلاعات وارد شده صحیح نمی باشد ، لطفا مجددا اطلاعات را با دقت وارد نمایید ';
//        }
//        else {
//            $scope.verify.error = 'با عرض پوزش ، خطایی رخ داده است ، لطفا کمی بعد مجددا اقدام نمایید. ';
//        }
//    }).finally(function () {
//        request = false;
//    });
//}

//$scope.getRequestFunction = function (value) {

//    if (request == true) return;
//    request = true;

//    $scope.get.success = '';
//    $scope.get.error = '';

//    Model.rquestGet(value).then(function (data) {
//        $scope.get.success = true;

//        var pass = '';
//        if (data.pass_type_1)
//        { if (data.pass_type_1 != '' && data.pass_1 != '') { pass += data.pass_type_1.label + ' ' + data.pass_1 + ' ' } }
//        if (data.pass_type_2)
//        { if (data.pass_type_2 != '' && data.pass_2 != '') { pass += data.pass_type_2.label + ' ' + data.pass_2 + ' ' } }
//        if (data.pass_type_3)
//        { if (data.pass_type_3 != '' && data.pass_3 != '') { pass += data.pass_type_3.label + ' ' + data.pass_3 + ' ' } }

//        $scope.get.success1 = '  نامه تصدیق مورد نظر شما برای   ' + data.first_name + ' ' + data.last_name + '  می باشد  ';
//        $scope.get.success2 = '  و به  نشانی   ' + data.city.label + ' ' + data.district_type.label + ' ' + data.district + ' ' + pass + ' پلاک ' + data.plaque + ' طبقه ' + data.floor + ' واحد ' + data.unit_no + '   ارسال شده است.  ';
//        $scope.get.success3 = 'وضعیت  نامه شما : ' + data.status.label;

//    }, function (error) {
//        if (error.error = 'NOT_FOUND') {
//            $scope.get.error = 'اطلاعات وارد شده صحیح نمی باشد ، لطفا مجددا اطلاعات را با دقت وارد نمایید ';
//        }
//        else {
//            $scope.get.error = 'با عرض پوزش ، خطایی رخ داده است ، لطفا کمی بعد مجددا اقدام نمایید. ';
//        }
//    }).finally(function () {
//        request = false;
//    });

//}

//$scope.beforeResendRequestFunction = function (value) {

//    if (request == true) return;
//    request = true;

//    $scope.resendGet.success = '';
//    $scope.resendGet.error = '';
//    $scope.resend.success = '';
//    $scope.resend.error = '';

//    Model.rquestGet(value).then(function (data) {
//        $scope.resendGet.success = true;

//        var pass = '';
//        if (data.pass_type_1)
//        { if (data.pass_type_1 != '' && data.pass_1 != '') { pass += data.pass_type_1.label + ' ' + data.pass_1 + ' ' } }
//        if (data.pass_type_2)
//        { if (data.pass_type_2 != '' && data.pass_2 != '') { pass += data.pass_type_2.label + ' ' + data.pass_2 + ' ' } }
//        if (data.pass_type_3)
//        { if (data.pass_type_3 != '' && data.pass_3 != '') { pass += data.pass_type_3.label + ' ' + data.pass_3 + ' ' } }

//        $scope.resendGet.success1 = '  نامه تصدیق مورد نظر شما برای   ' + data.first_name + ' ' + data.last_name + '  می باشد  ';
//        $scope.resendGet.success2 = '  و به  نشانی   ' + data.city.label + ' ' + data.district_type.label + ' ' + data.district + ' ' + pass + ' پلاک ' + data.plaque + ' طبقه ' + data.floor + ' واحد ' + data.unit_no + '   ارسال شده است.  ';

//    }, function (error) {
//        if (error.error = 'NOT_FOUND') {
//            $scope.resendGet.error = 'اطلاعات وارد شده صحیح نمی باشد ، لطفا مجددا اطلاعات را با دقت وارد نمایید ';
//        }
//        else {
//            $scope.resendGet.error = 'با عرض پوزش ، خطایی رخ داده است ، لطفا کمی بعد مجددا اقدام نمایید. ';
//        }
//    }).finally(function () {
//        request = false;
//    });
//}

//$scope.resendRequestFunction = function (value) {

//    if (request == true) return;
//    request = true;

//    $scope.resend.success = '';
//    $scope.resend.error = '';

//    Model.rquestResend(value).then(function (data) {
//        $scope.resend.success = 'درخواست شما با موفقیت ثبت شد';

//    }, function (error) {
//        if (error.error = 'NOT_FOUND') {
//            $scope.resend.error = 'اطلاعات وارد شده صحیح نمی باشد ، لطفا مجددا اطلاعات را با دقت وارد نمایید ';
//        }
//        else {
//            $scope.resend.error = 'با عرض پوزش ، خطایی رخ داده است ، لطفا کمی بعد مجددا اقدام نمایید. ';
//        }
//    }).finally(function () {
//        request = false;
//    });
//}

//$scope.reportRequestConfirmFunction = function (value) {

//    if (request == true) return;
//    request = true;

//    $scope.confirm.success = '';

//    $scope.confirm.error = '';
//    $scope.report.success = '';
//    $scope.report.error = '';
//    // 5574633
//    Model.rquestGet(value).then(function (data) {
//        $scope.confirm.success = true;

//        var pass = '';
//        if (data.pass_type_1)
//        { if (data.pass_type_1 != '' && data.pass_1 != '') { pass += data.pass_type_1.label + ' ' + data.pass_1 + ' ' } }
//        if (data.pass_type_2)
//        { if (data.pass_type_2 != '' && data.pass_2 != '') { pass += data.pass_type_2.label + ' ' + data.pass_2 + ' ' } }
//        if (data.pass_type_3)
//        { if (data.pass_type_3 != '' && data.pass_3 != '') { pass += data.pass_type_3.label + ' ' + data.pass_3 + ' ' } }

//        $scope.confirm.success1 = '  نامه تصدیق مورد نظر شما برای   ' + data.first_name + ' ' + data.last_name + '  می باشد  ';
//        $scope.confirm.success2 = '  و به  نشانی   ' + data.city.label + ' ' + data.district_type.label + ' ' + data.district + ' ' + pass + ' پلاک ' + data.plaque + ' طبقه ' + data.floor + ' واحد ' + data.unit_no + '   ارسال شده است.  ';
//        $scope.confirm.success3 = "  آیا مایل هستید گزارش کنید که آدرس برای فرد مذکور اشتباه است ؟  ";
//    }, function (error) {
//        if (error.error = 'NOT_FOUND') {
//            $scope.confirm.error = 'اطلاعات وارد شده صحیح نمی باشد ، لطفا مجددا اطلاعات را با دقت وارد نمایید ';
//        }
//        else {
//            $scope.confirm.error = 'با عرض پوزش ، خطایی رخ داده است ، لطفا کمی بعد مجددا اقدام نمایید. ';
//        }
//    }).finally(function () {
//        request = false;
//    });

//}

//$scope.reportRequestFunction = function (value) {

//    if (request == true) return;
//    request = true;

//    $scope.report.success = '';
//    $scope.report.error = '';

//    Model.rquestReport(value).then(function (data) {
//        $scope.report.success = 'گزارش شما با موفقیت ثبت شد  ، با تشکر از همکاری شما';
//    }, function (error) {
//        if (error.error = 'NOT_FOUND') {
//            $scope.report.error = 'اطلاعات وارد شده صحیح نمی باشد ، لطفا مجددا اطلاعات را با دقت وارد نمایید ';
//        }
//        else {
//            $scope.report.error = 'با عرض پوزش ، خطایی رخ داده است ، لطفا کمی بعد مجددا اقدام نمایید. ';
//        }
//    }).finally(function () {
//        request = false;
//    });
//}

//$scope.beforeAddNewRequest = function () {
//    if (($scope.addNewRequest.MainRoadDrop === '' || $scope.addNewRequest.MainRoadTxt === '') && ($scope.addNewRequest.SecondaryRoadDrop1 === '' || $scope.addNewRequest.SecondaryRoadTxt1 === '') && ($scope.addNewRequest.SecondaryRoadDrop2 === '' || $scope.addNewRequest.SecondaryRoadTxt2 === '')) {
//        $scope.addNewRequest.error = 'لطفا معبر اصلی یا معبر فرعی خود را مشخص کنید';
//        $scope.addNewRequest.postKind = false;
//        return;
//    } else {
//        if (!isValidIranianNationalCode($scope.addNewRequest.NationalCode)) {
//            $scope.addNewRequest.error = 'کد ملی وارد شده معتبر نمی باشد ، لطفا کد ملی خود را با دقت وارد کنید';
//            $scope.addNewRequest.postKind = false;
//            return;
//        }
//        else {
//            $scope.addNewRequest.postKind = true;
//            $scope.addNewRequest.error = false;
//            $('#addNewRequestSubmit').attr('disabled', 'disabled');
//            $('#addNewRequestFirstName').attr('disabled', 'disabled');
//            $('#addNewRequestLastName').attr('disabled', 'disabled');
//            $('#addNewRequestGender').attr('disabled', 'disabled');
//            $('#addNewRequestNationalCode').attr('disabled', 'disabled');
//            $('#addNewRequestPhoneNumber').attr('disabled', 'disabled');
//            $('#addNewRequestEmail').attr('disabled', 'disabled');
//            $('#addNewRequestPostalCode1').attr('disabled', 'disabled');
//            $('#addNewRequestPostalCode2').attr('disabled', 'disabled');
//            $('#addNewRequestState').attr('disabled', 'disabled');
//            $('#addNewRequestCity').attr('disabled', 'disabled');
//            $('#addNewRequestVillageDrop').attr('disabled', 'disabled');
//            $('#addNewRequestVillageTxt').attr('disabled', 'disabled');
//            $('#addNewRequestMainRoadDrop').attr('disabled', 'disabled');
//            $('#addNewRequestMainRoadTxt').attr('disabled', 'disabled');
//            $('#addNewRequestSecondaryRoadDrop1').attr('disabled', 'disabled');
//            $('#addNewRequestSecondaryRoadTxt1').attr('disabled', 'disabled');
//            $('#addNewRequestSecondaryRoadDrop2').attr('disabled', 'disabled');
//            $('#addNewRequestSecondaryRoadTxt2').attr('disabled', 'disabled');
//            $('#addNewRequestPlaque').attr('disabled', 'disabled');
//            $('#addNewRequestFloor').attr('disabled', 'disabled');
//            $('#addNewRequestUnit').attr('disabled', 'disabled');
//            $('#addNewRequestBuildingName').attr('disabled', 'disabled');
//            $('#addNewRequestSubmit').attr('disabled', 'disabled');
//        }
//    }
//}

//$scope.addNewRequestFunction = function () {

//    $('#addNewRequestSendBtn').attr('disabled', 'disabled');

//    if (request == true) return;

//    request = true;

//    $scope.addNewRequest.success = '';
//    $scope.addNewRequest.error = '';

//    Model.requestNew($scope.addNewRequest).then(function (data) {
//        $scope.addNewRequest.success = '  آدرس شما با موفقیت ثبت شد و به زودی نامه تصدیق به دستتان میرسد ، کد رهگیری شما :  ' + data.id + '  با تشکر از همکاریتان ';

//    }, function (error) {
//        if (error.error = 'NOT_FOUND') {
//            $scope.addNewRequest.error = 'اطلاعات وارد شده صحیح نمی باشد ، لطفا مجددا اطلاعات را با دقت وارد نمایید ';
//        }
//        else {
//            $scope.addNewRequest.error = 'با عرض پوزش ، خطایی رخ داده است ، لطفا کمی بعد مجددا اقدام نمایید. ';
//        }
//    }).finally(function () {
//        request = false;
//    });
//}

//$scope.addNewMessageFunction = function (value) {

//    if (request == true) return;
//    request = true;

//    $scope.addNewMessage.success = '';
//    $scope.addNewMessage.error = '';

//    Model.messageNew(value).then(function (data) {
//        $scope.addNewMessage.success = 'یکی از همکاران ما ظرف 48 ساعت آینده از طریق تلفن یا ایمیل پاسخگوی سوال شما خواهد بود';
//    }, function (error) {
//        console.log(error)
//        if (error.error = 'NOT_FOUND') {
//            $scope.addNewMessage.error = 'اطلاعات وارد شده صحیح نمی باشد ، لطفا مجددا اطلاعات را با دقت وارد نمایید ';
//        }
//        else {
//            $scope.addNewMessage.error = 'با عرض پوزش ، خطایی رخ داده است ، لطفا کمی بعد مجددا اقدام نمایید. ';
//        }
//    }).finally(function () {
//        request = false;
//    });
//}

//$scope.newsletterFunction = function (email) {

//    if (request == true) return;
//    if (!email) {
//        $scope.newsletterResponse = 'لطفا ایمیل خود را وارد نمایید';
//        return;
//    }
//    request = true;

//    $scope.newsletterResponse = '';

//    Model.newsletter(email).then(function (data) {
//        $scope.newsletterResponse = 'ایمیل شما با موفقیت ثبت شد. با تشکر از شما';
//    }, function (error) {
//        console.log(error)
//        if (error.error = 'NOT_FOUND') {
//            $scope.newsletterResponse = 'اطلاعات وارد شده صحیح نمی باشد ، لطفا مجددا اطلاعات را با دقت وارد نمایید ';
//        }
//        else {
//            $scope.newsletterResponse = 'با عرض پوزش ، خطایی رخ داده است ، لطفا کمی بعد مجددا اقدام نمایید. ';
//        }
//    }).finally(function () {
//        request = false;
//    });
//}
