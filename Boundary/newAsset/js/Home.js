angular.module('Home', [])

 .controller('HomeCtrl', ['$scope', '$timeout', function ($scope, $timeout) {

     $scope.noProductPic = "Img/MainPage/NoProductPic.png";
     $scope.noStorePic = "Img/MainPage/NoStorePic.png";

     var root = "http://hoojibooji.com/";
     var owl1, owl2, owl3, owl4, owl5;
     loadCustomBox();
     loadInitProduct(1);
     loadInitProduct(3);
     loadInitProduct(5);
     loadInitStore(4);

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


         var html = '<a href="/product/' + value.Id + '/نام_فروشگاه=' + value.StoreName.replace(/:/g, '_').replace(/ /g, '_') + '/نام_محصول=' + value.Name.replace(/:/g, '_').replace(/ /g, '_') + '/قیمت=' + PriceTemp + 'تومان" > <div class="borderRight item">'
                + ' <div class="organizer standardVerticalMargin">'
                + '<div class="circleImageContainer">'
                + " <img src='" + root + value.ImgAddress + "?w=143&h=143&mode=carve' alt='نام_فروشگاه=" + value.StoreName + '/نام_محصول=' + value.Name + "/قیمت=" + PriceTemp + "تومان' class='imageInMiddle'>"
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
                        + " <img src='" + root + value.LogoAddress + "?w=143&h=143&mode=carve' alt='" + value.Name + "," + storeType + "' class='imageInMiddle'>"
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
      
      
 }]);




