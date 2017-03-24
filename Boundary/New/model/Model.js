'use strict';

angular.module('post.Model', [])
 .factory('Model', ['$http',
    function ($http) {

        var Model = {};

        var apiBaseUrl = "http://myaddress.space/infra/core/";


        //--------------------------1- User Management ------------------------------


        //SignIn
        Model.SignIn = function (email, password) {
            return $http({
                url: apiBaseUrl + 'user/login',
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                transformRequest: function (obj) {
                    var str = [];
                    for (var p in obj)
                        str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
                    return str.join("&");
                },
                method: "POST",
                data: {
                    email: email,
                    password: password
                }
            }).then(function (result) { return result.data; });
        };


        //--------------------------2-Basic Api---------------------------------------


        //organizationTypeList
        Model.organizationTypeList = function () {
            return $http({
                url: apiBaseUrl + 'company/organization-type/list',
                method: "GET",
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            }).then(function (result) { return result.data; });
        };

        //requestTypeList
        Model.requestTypeList = function () {
            return $http({
                url: apiBaseUrl + 'request-type/list',
                method: "GET",
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            }).then(function (result) { return result.data; });
        };

        //companyRequestTypeList
        Model.companyRequestTypeList = function () {
            return $http({
                url: apiBaseUrl + 'default-company/request-type/list',
                method: "POST",
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            }).then(function (result) { return result.data; });
        };

        //genderList
        Model.genderList = function () {
            return $http({
                url: apiBaseUrl + 'gender/list',
                method: "GET",
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            }).then(function (result) { return result.data; });
        };

        //passTypeList
        Model.passTypeList = function () {
            return $http({
                url: apiBaseUrl + 'pass-type/list',
                method: "GET",
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            }).then(function (result) { return result.data; });
        };

        //stateList
        Model.stateList = function () {
            return $http({
                url: apiBaseUrl + 'province/list',
                method: "GET",
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            }).then(function (result) { return result.data; });
        };

        //districtList
        Model.districtList = function () {
            return $http({
                url: apiBaseUrl + 'district-type/list',
                method: "GET",
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            }).then(function (result) { return result.data; });
        };

        //cityList
        Model.cityList = function (stateId) {
            return $http({
                url: apiBaseUrl + 'city/list',
                method: "POST",
                transformRequest: function (obj) {
                    var str = [];
                    for (var p in obj)
                        str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
                    return str.join("&");
                },
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                data: {
                    province_id: stateId
                }
            }).then(function (result) { return result.data; });
        };

        //districtTypeList
        Model.districtTypeList = function () {
            return $http({
                url: apiBaseUrl + 'district-type/list',
                method: "GET",
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            }).then(function (result) { return result.data; });
        };

        //personTypeList
        Model.personTypeList = function () {
            return $http({
                url: apiBaseUrl + 'person-type/list',
                method: "GET",
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            }).then(function (result) { return result.data; });
        };

        //organizationalPositionList
        Model.organizationalPositionList = function () {
            return $http({
                url: apiBaseUrl + 'organizational-position/list',
                method: "GET",
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            }).then(function (result) { return result.data; });
        };



        //--------------------------3-request Api---------------------------------------
        


        //requestNew
        Model.requestNew = function (Value) {
            return $http({
                url: apiBaseUrl + 'default-company/request/new',
                method: "POST",
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                transformRequest: function (obj) {
                    var str = [];
                    for (var p in obj)
                        str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
                    return str.join("&");
                },
                data: {
                    first_name: Value.FirstName,
                    last_name: Value.LastName,
                    gender_id: Value.Gender,
                    national_id: Value.NationalCode,
                    cell: Value.PhoneNumber,
                    email: Value.Email,
                    postcode: Value.PostalCode1,
                    national_card_postcode: Value.PostalCode2,
                    city_id: Value.City,
                    district_type_id: Value.VillageDrop,
                    district: Value.VillageTxt,
                    pass_type_1_id: Value.MainRoadDrop,
                    pass_1: Value.MainRoadTxt,
                    pass_type_2_id: Value.SecondaryRoadDrop1,
                    pass_2: Value.SecondaryRoadTxt1,
                    pass_type_3_id: Value.SecondaryRoadDrop2,
                    pass_3: Value.SecondaryRoadTxt2,
                    plaque: Value.Plaque,
                    floor: Value.Floor[0],
                    unit_no: Value.Unit,
                    company_request_type_id: Value.companyRequestTypeId,
                    template_id: 1,
                    duration: 12,
                    resend_period: 13,
                    resend_expiration_time: '',
                    building: Value.BuildingName,
                    api_key: '4b3025ca-357a-4ffb-aa68-6655e4b617d7``'

                }
            }).then(function (result) { return result.data; });
        };

        //rquestVerify
        Model.rquestVerify = function (value) {

             return $http({
                url: apiBaseUrl + 'request/verify',
                method: "POST",
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                transformRequest: function (obj) {
                    var str = [];
                    for (var p in obj)
                        str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
                    return str.join("&");
                },
                data: {
                    national_id: value.national_id,
                    secret_code: value.secret_code

                }
            }).then(function (result) { return result.data; });
        };

        //rquestGet
        Model.rquestGet = function (value) {

            return $http({
                url: apiBaseUrl + 'request/get',
                method: "POST",
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                transformRequest: function (obj) {
                    var str = [];
                    for (var p in obj)
                        str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
                    return str.join("&");
                },
                data: {
                    unique_id: value.uniqeId
                }
            }).then(function (result) { return result.data; });
        };

        //rquestResend
        Model.rquestResend = function (value) {

            return $http({
                url: apiBaseUrl + 'request/resend',
                method: "POST",
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                transformRequest: function (obj) {
                    var str = [];
                    for (var p in obj)
                        str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
                    return str.join("&");
                },
                data: {
                    unique_id: value.uniqeId
                }
            }).then(function (result) { return result.data; });
        };

        //rquestReport
        Model.rquestReport = function (value) {

            return $http({
                url: apiBaseUrl + 'request/report',
                method: "POST",
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                transformRequest: function (obj) {
                    var str = [];
                    for (var p in obj)
                        str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
                    return str.join("&");
                },
                data: {
                    unique_id: value.uniqeId
                }
            }).then(function (result) { return result.data; });
        };

        //messageNew
        Model.messageNew = function (value) {

            return $http({
                url: apiBaseUrl + 'message/new',
                method: "POST",
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                transformRequest: function (obj) {
                    var str = [];
                    for (var p in obj)
                        str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
                    return str.join("&");
                },
                data: {
                    email: value.email,
                    name: value.name,
                    cell: value.cell,
                    unique_id: value.uniqId,
                    message:value.text
                }
            }).then(function (result) { return result.data; });
        };

        //newsletter
        Model.newsletter = function (email) {
            return $http({
                url: apiBaseUrl + 'newsletter/new-email',
                method: "POST",
                transformRequest: function (obj) {
                    var str = [];
                    for (var p in obj)
                        str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
                    return str.join("&");
                },
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                data: {
                    email: email
                }
            }).then(function (result) { return result.data; });
        };




        return Model;
    }]);






























