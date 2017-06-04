  app.controller('getProductCtrl', ['$scope', function ($scope) {

       $scope.Data = data;
       console.log($scope.Data)

        if ($scope.Data && $scope.Data.Product && $scope.Data.Product.ImgAddress && $scope.Data.Product.ImgAddress!=null) {
            var temp = $scope.Data.Product.ImgAddress + '';
            $scope.Data.Product.ImgAddress = temp.replace(/\\/g, '/');
        }else{
            $scope.Data.Product.ImgAddress = "Img/MainPage/NoProductPic.png"
        }

        if ($scope.Data && $scope.Data.Product && $scope.Data.Product.OtherImagesAddress) {
            $.each($scope.Data.Product.OtherImagesAddress , function(index,value){
                var temp = $scope.Data.Product.OtherImagesAddress[index].ImgAddress + '';
                $scope.Data.Product.OtherImagesAddress[index].ImgAddress = temp.replace(/\\/g, '/');
            })
        }

        if ($scope.Data && $scope.Data.Product && $scope.Data.Product.DiscountedPrice) {
            var temp = $scope.Data.Product.DiscountedPrice + '';
            $scope.Data.Product.DiscountedPrice = temp.replace(/,/g, '').replace(/(\d)(?=(\d{3})+$)/g, '$1,');
        }

        if ($scope.Data && $scope.Data.Product && $scope.Data.Product.Price) {
            var temp = $scope.Data.Product.Price + '';
            $scope.Data.Product.Price = temp.replace(/,/g, '').replace(/(\d)(?=(\d{3})+$)/g, '$1,');
        }

        if($scope.Data&&$scope.Data.ProductAttrbiutesViewModels&&$scope.Data.ProductAttrbiutesViewModels!='')
        {
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
           
        $scope.AddToBasket=function(){

            var realPrice='';
            if($scope.Data.Product && $scope.Data.Product.DiscountedPrice && $scope.Data.Product.DiscountedPrice!=0){
                realPrice=$scope.Data.Product.DiscountedPrice;
            }
            else if($scope.Data.Product && $scope.Data.Product.Price && $scope.Data.Product.Price!=0){
                realPrice=$scope.Data.Product.Price;
            }
            else{
                realPrice=0;
            }

            if(realPrice && realPrice!=0){
                realPrice= realPrice.replace(/,/g,'');
            }

            var ProductToShoppingBagDataModel ={
                ProductCode:$scope.Data.Product.Id,
                ProductName:$scope.Data.Product.Name,
                RealPrice:realPrice,
                StoreName:$scope.Data.StoreSummery.Name,
                ImgAddress:$scope.Data.Product.ImgAddress,
                PostalCostInCountry:$scope.Data.Product.PostalCostInCountry,
                PostalCostInTown:$scope.Data.Product.PostalCostInTown
            };

            console.log(ProductToShoppingBagDataModel)

            $.ajax({
                type: "GET",
                url: "/order/AddToBag/",
                data: ProductToShoppingBagDataModel,
                success: function (result) {
                    window.location.href='/order' ;
                },
                error: function () {
                    console.log("ارتباط با سرور برقرار نشد ، لطفا مدتی بعد دوباره امتحان نمایید");
                }
            });
        };

        var lat = $scope.Data.StoreSummery.Latitude;
        var longi = $scope.Data.StoreSummery.Longitude;

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


    }]);

 