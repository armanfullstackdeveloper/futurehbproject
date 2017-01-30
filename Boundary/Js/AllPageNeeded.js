
$(function () {
    //----------------------------------------------------------------------------

    //Login

    //----------------------------------------------------------------------------
	

    $('#LogIn').click(function () {
        document.getElementById('LogInOpen').style.display = "block";
        document.getElementById('ForClose').style.display = "block";
        $('#LogInOpen').addClass('animationForLogIn');
        $('#LogInOHolder').fadeIn(500);
    });

    $('#ForClose').click(function () {
        document.getElementById('LogInOpen').style.display = "none";
        document.getElementById('ForClose').style.display = "none";
        document.getElementById('LogInOHolder').style.display = "none";
        $('#LogInOpen').removeClass('animationForLogIn');
    });

    $('#CloseOnly').click(function () {
        document.getElementById('LogInOpen').style.display = "none";
        document.getElementById('ForClose').style.display = "none";
        document.getElementById('LogInOHolder').style.display = "none";
        $('#LogInOpen').removeClass('animationForLogIn');
    });


    //----------------------------------------------------------------------------

    //Menu

    //----------------------------------------------------------------------------

    $('#cssmenu li.active').addClass('open').children('ul').show();
    $('#cssmenu li.has-sub>a').on('click', function () {
        $(this).removeAttr('href');
        var element = $(this).parent('li');
        if (element.hasClass('open')) {
            element.removeClass('open');
            element.find('li').removeClass('open');
            element.find('ul').slideUp(200);
        }
        else {
            element.addClass('open');
            element.children('ul').slideDown(200);
            element.siblings('li').children('ul').slideUp(200);
            element.siblings('li').removeClass('open');
            element.siblings('li').find('li').removeClass('open');
            element.siblings('li').find('ul').slideUp(200);
        }
    });

    //----------------------------------------------------------------------------

    //Categori

    //----------------------------------------------------------------------------


});