
   app.controller('orderCtrl', ['$scope', function ($scope) {

      $('.firstHide').show();
      $scope.Data = data;
      console.log($scope.Data)

      $scope.AllTotalPrice=0;
      $.each($scope.Data , function(index , value){
          var totalTemp=parseInt(value.RealPrice)*parseInt(value.Count);

          $scope.AllTotalPrice+=totalTemp;
          $scope.Data[index].TotalPrice=(totalTemp +'').replace(/,/g, '').replace(/(\d)(?=(\d{3})+$)/g, '$1,');
          $scope.Data[index].RealPrice=(value.RealPrice+'').replace(/,/g, '').replace(/(\d)(?=(\d{3})+$)/g, '$1,');
          $scope.Data[index].PostalCost=(value.PostalCost+'').replace(/,/g, '').replace(/(\d)(?=(\d{3})+$)/g, '$1,');
      })
      $scope.AllTotalPrice=($scope.AllTotalPrice+'').replace(/,/g, '').replace(/(\d)(?=(\d{3})+$)/g, '$1,');

      $scope.CountMinus = function (id) {
          var deletedIndex = -1;
          $.each($scope.Data, function (index, value) { 
              if (value && (value.ProductCode == id)) { 
                  if (value.Count == 1   ) {
                      deletedIndex = index;
                  }
                  else {
                      value.Count--;
                  }
              }
          })

 
         if (deletedIndex != -1) {
               $scope.Data.splice(deletedIndex,1);
          }
 

          $.ajax({
              type: "GET",
              url: "/order/MinusProductCount?productCode="+id,
              success: function (result) {
                //  window.location.reload();
              } 
          });
      }
      $scope.CountPlus = function (id) {
          $.each($scope.Data, function (index, value) {
              if (value && (value.ProductCode == id)) {
                  value.Count++;
              }
          })
          $.ajax({
              type: "GET",
              url: "/order/AddProductCount?productCode="+id,
              success: function (result) {
                  // window.location.reload();
              } 
          });
      } 
      $scope.RemoveProduct = function (id) {
          var deletedIndex = -1;
          $.each($scope.Data, function (index, value) {
              if (value && value.ProductCode == id) {
                  deletedIndex = index;
              }
          })
          if (deletedIndex != -1) {
              $scope.Data.splice(deletedIndex,1);
          }
          $.ajax({
              type: "GET",
              url: "/order/RemoveFromBag?productCode="+id,
              success: function (result) {
                  //window.location.reload(); 
              } 
          });
      }

  }]);
 