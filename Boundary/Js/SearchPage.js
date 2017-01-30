$(function () {


    $('#showMoreBrand').click(function () {
       // document.getElementById('BrandHolder').style.height = "1100px";
        document.getElementById('BrandHolder').style.height = "auto";
        document.getElementById('showMoreBrand').style.display = "none";
        document.getElementById('showLessBrand').style.display = "block";
    });

    $('#showLessBrand').click(function () {
        document.getElementById('BrandHolder').style.height = "270px";
        document.getElementById('showMoreBrand').style.display = "block";
        document.getElementById('showLessBrand').style.display = "none";
    });
	$('#showMoreColor').click(function () {
       // document.getElementById('BrandHolder').style.height = "1100px";
        document.getElementById('colorHolder').style.height = "auto";
        document.getElementById('showMoreColor').style.display = "none";
        document.getElementById('showLessColor').style.display = "block";
    });

    $('#showLessColor').click(function () {
        document.getElementById('colorHolder').style.height = "270px";
        document.getElementById('showMoreColor').style.display = "block";
        document.getElementById('showLessColor').style.display = "none";
    });

    $('#hommyHolder').click(function () {
        setTimeout(function ()
        { document.getElementById('Container').style.display = "none"; }, 600);
    });

    $('#city').click(function () {
        setTimeout(function ()
        { document.getElementById('Container').style.display = "none"; }, 600);
    });

    $('#online').click(function () {
        setTimeout(function ()
        { document.getElementById('Container').style.display = "none"; }, 600);
    });

    $('#Nearest').click(function () {
        document.getElementById('NearestContainer').style.display = "block";
        $('#NearestContainer').addClass('animationForLogIn');
        $('#NearestHolder').fadeIn(500);
    });

    $('#CloseNearest').click(function () {
        document.getElementById('NearestContainer').style.display = "none";
        document.getElementById('NearestHolder').style.display = "none";
        $('#NearestContainer').removeClass('animationForLogIn');
    });

    $('#CloseNear').click(function () {
        document.getElementById('NearestContainer').style.display = "none";
        document.getElementById('NearestHolder').style.display = "none";
        $('#NearestContainer').removeClass('animationForLogIn');
    });


});