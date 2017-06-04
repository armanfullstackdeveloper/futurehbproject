app.controller('getProductCtrl', ['$scope', function ($scope) {

    $scope.Data = data;
    $scope.noProductPic = "Img/MainPage/NoProductPic.png";
    $scope.noStorePic = "Img/MainPage/NoStorePic.png";
    $scope.owl1Data;
    $scope.owl2Data;
    var root = "http://hoojibooji.com/";
    var lat = $scope.Data.StoreSummery.Latitude;
    var longi = $scope.Data.StoreSummery.Longitude;
    var owl1, owl2;


    console.log($scope.Data)

    if ($scope.Data && $scope.Data.Product && $scope.Data.Product.ImgAddress && $scope.Data.Product.ImgAddress != null) {
        var temp = $scope.Data.Product.ImgAddress + '';
        $scope.Data.Product.ImgAddress = temp.replace(/\\/g, '/');
    } else {
        $scope.Data.Product.ImgAddress = "Img/MainPage/NoProductPic.png"
    }

    if ($scope.Data && $scope.Data.Product && $scope.Data.Product.OtherImagesAddress) {
        $.each($scope.Data.Product.OtherImagesAddress, function (index, value) {
            var temp = $scope.Data.Product.OtherImagesAddress[index].ImgAddress + '';
            $scope.Data.Product.OtherImagesAddress[index].ImgAddress = temp.replace(/\\/g, '/');
        })
    }

    if ($scope.Data && $scope.Data.Product && $scope.Data.Product.Price) {
        var PriceValue = ($scope.Data.Product.Price) * 1;
        var temp = $scope.Data.Product.Price + '';
        $scope.Data.Product.Price = temp.replace(/,/g, '').replace(/(\d)(?=(\d{3})+$)/g, '$1,');
    }

    if ($scope.Data && $scope.Data.Product && $scope.Data.Product.DiscountedPrice) {
        var DiscountedPriceValue = ($scope.Data.Product.DiscountedPrice) * 1;
        var temp = $scope.Data.Product.DiscountedPrice + '';
        $scope.Data.Product.DiscountedPrice = temp.replace(/,/g, '').replace(/(\d)(?=(\d{3})+$)/g, '$1,');

        var DiscountAndPriceTemp = PriceValue - DiscountedPriceValue;
        $scope.percentDiscount = ((DiscountAndPriceTemp * 100) / PriceValue).toFixed(1) + ' درصد تخفیف ';
        console.log($scope.percentDiscount)

    }

    if ($scope.Data && $scope.Data.ProductAttrbiutesViewModels && $scope.Data.ProductAttrbiutesViewModels != '') {
        $.each($scope.Data.ProductAttrbiutesViewModels, function (index, value) {
            if (value) {
                value.Value = '';
                $.each(value.AttributeValues, function (index2, value2) {
                    if (value2) {
                        if (value.Value) {
                            value.Value += ' , ' + value2;
                        } else {
                            value.Value += value2;
                        }
                    }
                })
            }
        })
    }

    $scope.AddToBasket = function () {

        var realPrice = '';
        if ($scope.Data.Product && $scope.Data.Product.DiscountedPrice && $scope.Data.Product.DiscountedPrice != 0) {
            realPrice = $scope.Data.Product.DiscountedPrice;
        }
        else if ($scope.Data.Product && $scope.Data.Product.Price && $scope.Data.Product.Price != 0) {
            realPrice = $scope.Data.Product.Price;
        }
        else {
            realPrice = 0;
        }

        if (realPrice && realPrice != 0) {
            realPrice = realPrice.replace(/,/g, '');
        }

        var ProductToShoppingBagDataModel = {
            ProductCode: $scope.Data.Product.Id,
            ProductName: $scope.Data.Product.Name,
            RealPrice: realPrice,
            StoreName: $scope.Data.StoreSummery.Name,
            ImgAddress: $scope.Data.Product.ImgAddress,
            PostalCostInCountry: $scope.Data.Product.PostalCostInCountry,
            PostalCostInTown: $scope.Data.Product.PostalCostInTown
        };

        console.log(ProductToShoppingBagDataModel)

        $.ajax({
            type: "GET",
            url: "/order/AddToBag/",
            data: ProductToShoppingBagDataModel,
            success: function (result) {
                window.location.href = '/order';
            },
            error: function () {
                console.log("ارتباط با سرور برقرار نشد ، لطفا مدتی بعد دوباره امتحان نمایید");
            }
        });
    };

    //Google map Api
    $(function () {
        var latlng = new google.maps.LatLng(parseFloat(lat), parseFloat(longi));
        var options = {
            zoom: 13,
            center: latlng,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

        var map = new google.maps.Map(document.getElementById("MiniMapHolder"), options);
        new google.maps.Marker({
            position: latlng,
            map: map
        });

    });


    loadInitProduct('RelatedProduct');
    loadInitProduct('OtherShopProduct');

    function loadInitProduct(type) {

        var data;
        if (type == 'RelatedProduct') {
            data = {
                sortBy: 3,//پر بازدید ترین ها
                SearchPlace: 3,//ProductDetails
                CategoryCode: $scope.Data.Product.CategoryCode,
                PageNumber: 1,
                RowsPage: 10
            }
        }
        else if (type == 'OtherShopProduct') {
            data = {
                sortBy: 1,// جدید ترین ها 
                StoreCode: $scope.Data.StoreSummery.Id,
                SearchPlace: 3,//ProductDetails
                PageNumber: 1,
                RowsPage: 10
            }
        }

        $.ajax({
            type: "POST",
            url: "/api/product/search",
            data: data,
            success: function (result) {

                if (result.Success != true) {
                    console.log("ارتباط با سرور برقرار نشد ، لطفا مدتی بعد دوباره امتحان نمایید");
                    return;
                }
                if (result.Response.ProductsSummery.length > 0) {


                    if (type == 'RelatedProduct') {
                        $scope.owl1Data=result;

                        $('.owlSlider1').html('');
                        $.each(result.Response.ProductsSummery, function (index, value) {
                            $('.owlSlider1').append(createProductHtml(value, type));
                        });
                        initOwl(type, owl1);
                    }
                    else if (type == 'OtherShopProduct') {
                        $scope.owl2Data=result;

                        $('.owlSlider2').html('');
                        $.each(result.Response.ProductsSummery, function (index, value) {
                            $('.owlSlider2').append(createProductHtml(value, type));
                        });
                        initOwl(type, owl2)
                    }
                    $scope.$apply();

                }
            },
            error: function () {
                console.log("ارتباط با سرور برقرار نشد ، لطفا مدتی بعد دوباره امتحان نمایید");
            }
        });
    }

    function initOwl(type, owl) {
         if (type == 'RelatedProduct') {
            owl = $('.owlSlider1').owlCarousel({
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


        }
        else if (type == 'OtherShopProduct') {
            owl = $('.owlSlider2' ).owlCarousel({
                rtl: true,
                loop: false,
                margin: 10,
                center: false,
                nav: false,
                items: 4,
                responsive: {
                    0: {
                        items: 1
                    },
                    600: {
                        items: 2
                    },
                    1000: {
                        items: 4
                    }
                }
            });

        }


        if (type == 'RelatedProduct') {
            $(".prev1").click(function () {
                owl.trigger('prev.owl.carousel');
            });
            $(".next1").click(function () {
                owl.trigger('next.owl.carousel');
            });
        }
        else if (type == 'OtherShopProduct') {
            $(".prev2").click(function () {
                owl.trigger('prev.owl.carousel');
            });
            $(".next2").click(function () {
                owl.trigger('next.owl.carousel');
            });
        }

        owl.on('changed.owl.carousel', function (event) {
            if (!event.item || (event.item.index + event.page.size + 4 >= event.item.count)) {
                if ($scope.loading) return;
                $scope.loading = true;
                loadNewProduct(event, type);

            }
        });
    }

    function loadNewProduct(event, type) {
        var PageNumber = 1;

        if (event && event.item) {
            count = event.item.count;
            PageNumber = parseInt((count * 1) / 10) + 1;
        }


        var data;
        if (type == 'RelatedProduct') {
            data = {
                sortBy: 3,//پر بازدید ترین ها
                SearchPlace: 3,//ProductDetails
                CategoryCode: $scope.Data.Product.CategoryCode,
                PageNumber: PageNumber,
                RowsPage: 10
            }
        }
        else if (type == 'OtherShopProduct') {
            data = {
                sortBy: 1,// جدید ترین ها 
                StoreCode: $scope.Data.StoreSummery.Id,
                SearchPlace: 3,//ProductDetails
                PageNumber: PageNumber,
                RowsPage: 10
            }
        }



        $.ajax({
            type: "POST",
            url: "/api/product/search",
            data: data,
            success: function (result) {
                $scope.loading = false;
                if (result.Success != true) {
                    console.log("ارتباط با سرور برقرار نشد ، لطفا مدتی بعد دوباره امتحان نمایید");
                    return;
                }
                setTimeout(function (index, value) {
                    if (result.Response.ProductsSummery.length > 0) {
                        $.each(result.Response.ProductsSummery, function (index, value) {
                            event.relatedTarget.add(createProductHtml(value, type))
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

    function createProductHtml(value, type) {
        if (value.ImgAddress == null) value.ImgAddress = $scope.noProductPic;
        var PriceTemp = value.Price + '';
        if (value.DiscountedPrice) PriceTemp = value.DiscountedPrice + '';
        PriceTemp = PriceTemp.replace(/,/g, '').replace(/(\d)(?=(\d{3})+$)/g, '$1,');
        var html
        if (type == 'RelatedProduct') {
            html = '<a href="/product/' + value.Id + '/نام_فروشگاه=' + value.StoreName.replace(/:/g, '_').replace(/ /g, '_') + '/نام_محصول=' + value.Name.replace(/:/g, '_').replace(/ /g, '_') + '/قیمت=' + PriceTemp + 'تومان" > <div class="borderRight item">'
                  + ' <div class="organizer standardVerticalMargin">'
                  + '<div class="circleImageContainer">'
                  + " <img src='" + root + value.ImgAddress + "?w=143&h=143&mode=carve' alt='نام_فروشگاه=" + value.StoreName + '/نام_محصول=' + value.Name + "/قیمت=" + PriceTemp + "تومان' class='imageInMiddle'>"
                  + ' </div></div>'
                  + ' <div class="organizer pinkColor">' + value.Name + '</div>'
                  + ' <div class="organizer smallExplain">قیمت' + PriceTemp + 'تومان ' + '</div>'
                  + ' </div></a>';
        }
        else if (type == 'OtherShopProduct') {

            html = '<a href="/product/' + value.Id + '/نام_فروشگاه=' + value.StoreName.replace(/:/g, '_').replace(/ /g, '_') + '/نام_محصول=' + value.Name.replace(/:/g, '_').replace(/ /g, '_') + '/قیمت=' + PriceTemp + 'تومان" > <div class="fourFullColumns item">'
              + ' <div class="squareItem relativeMatte">'
               + " <img src='" + root + value.ImgAddress + "?w=143&h=143&mode=carve' alt='نام_فروشگاه=" + value.StoreName + '/نام_محصول=' + value.Name + "/قیمت=" + PriceTemp + "تومان' class='imageInMiddle'>"
              + ' </div>'
              + ' </div></a>';
        }
        return html;

    }


    $('#mainContainer').slideDown();
}]);

