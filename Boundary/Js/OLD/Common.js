// JavaScript Document



$(document).ready(function(){
	
	
	
	$('.AnimateNext').mouseover(function() {
		$('.AnimateNext').addClass('NavMove');
		$('.AnimateNext').removeClass('NavMoveReverse');
	});
	$('.AnimatePerv').mouseover(function() {
		$('.AnimatePerv').addClass('NavMovePerv');
		$('.AnimatePerv').removeClass('NavMovePervRevers');
	});
	
	$('.AnimateNext').mouseleave(function() {
		$('.AnimateNext').removeClass('NavMove');
		$('.AnimateNext').addClass('NavMoveReverse');
	});
	$('.AnimatePerv').mouseleave(function() {
		$('.AnimatePerv').removeClass('NavMovePerv');
		$('.AnimatePerv').addClass('NavMovePervRevers');
	});
	
	
	
	$('#Category_1').mouseover(function() {
		setTimeout(function () 
		{document.getElementById('SubCategory_1_1').style.display="block";
		$('#SubCategory_1_1').addClass('Fade');
		 }, 250);
		 
		 
		 setTimeout(function () 
		{document.getElementById('SubCategory_1_2').style.display="block";
		$('#SubCategory_1_2').addClass('Fade');
		 }, 250);
		 
		 
		  setTimeout(function () 
		{document.getElementById('SubCategory_1_3').style.display="block";
		$('#SubCategory_1_3').addClass('Fade');
		 }, 250);
		 
		 
		 setTimeout(function () 
		{document.getElementById('SubCategory_1_4').style.display="block";
		$('#SubCategory_1_4').addClass('Fade');
		 }, 250);
		 
		 
		document.getElementById('open_1').style.display="block";
		$('#open_1').removeClass('boomSplashReverse');
		$('#open_1').addClass('boomSplash');
		document.getElementById('Category_1').style.zIndex="9";
	});
	$('#holder_1').mouseleave(function() {
		document.getElementById('SubCategory_1_1').style.display="none";
		$('#SubCategory_1_1').removeClass('Fade');
		
		
		document.getElementById('SubCategory_1_2').style.display="none";
		$('#SubCategory_1_2').removeClass('Fade');
		
		
		document.getElementById('SubCategory_1_3').style.display="none";
		$('#SubCategory_1_3').removeClass('Fade');
		
		
		document.getElementById('SubCategory_1_4').style.display="none";
		$('#SubCategory_1_4').removeClass('Fade');
		
		
		setTimeout(function () {document.getElementById('open_1').style.display="none"; }, 500);
		$('#open_1').addClass('boomSplashReverse');
		$('#open_1').removeClass('boomSplash');
		document.getElementById('Category_1').style.zIndex="9";
	});
	$('#Category_2').mouseover(function() {
		setTimeout(function () 
		{document.getElementById('SubCategory_2_1').style.display="block";
		$('#SubCategory_2_1').addClass('Fade');
		 }, 250);
		 
		 
		 setTimeout(function () 
		{document.getElementById('SubCategory_2_2').style.display="block";
		$('#SubCategory_2_2').addClass('Fade');
		 }, 250);
		 
		 
		  setTimeout(function () 
		{document.getElementById('SubCategory_2_3').style.display="block";
		$('#SubCategory_2_3').addClass('Fade');
		 }, 250);
		 
		 
		 setTimeout(function () 
		{document.getElementById('SubCategory_2_4').style.display="block";
		$('#SubCategory_2_4').addClass('Fade');
		 }, 250);
		 
		 
		document.getElementById('open_2').style.display="block";
		$('#open_2').removeClass('boomSplashReverse');
		$('#open_2').addClass('boomSplash');
		document.getElementById('Category_2').style.zIndex="9";
	});
	$('#holder_2').mouseleave(function() {
		document.getElementById('open_2').style.display="none";
		document.getElementById('Category_2').style.zIndex="8";
	});
	$('#Category_3').mouseover(function() {
		setTimeout(function () 
		{document.getElementById('SubCategory_3_1').style.display="block";
		$('#SubCategory_3_1').addClass('Fade');
		 }, 250);
		 
		 
		 setTimeout(function () 
		{document.getElementById('SubCategory_3_2').style.display="block";
		$('#SubCategory_3_2').addClass('Fade');
		 }, 250);
		 
		 
		  setTimeout(function () 
		{document.getElementById('SubCategory_3_3').style.display="block";
		$('#SubCategory_3_3').addClass('Fade');
		 }, 250);
		 
		 
		 setTimeout(function () 
		{document.getElementById('SubCategory_3_4').style.display="block";
		$('#SubCategory_3_4').addClass('Fade');
		 }, 250);
		 
		 
		document.getElementById('open_3').style.display="block";
		$('#open_3').removeClass('boomSplashReverse');
		$('#open_3').addClass('boomSplash');
		document.getElementById('Category_3').style.zIndex="9";
	});
	$('#holder_3').mouseleave(function() {
		document.getElementById('open_3').style.display="none";
		document.getElementById('Category_3').style.zIndex="8";
	});
	$('#Category_4').mouseover(function() {
		setTimeout(function () 
		{document.getElementById('SubCategory_4_1').style.display="block";
		$('#SubCategory_4_1').addClass('Fade');
		 }, 250);
		 
		 
		 setTimeout(function () 
		{document.getElementById('SubCategory_4_2').style.display="block";
		$('#SubCategory_4_2').addClass('Fade');
		 }, 250);
		 
		 
		  setTimeout(function () 
		{document.getElementById('SubCategory_4_3').style.display="block";
		$('#SubCategory_4_3').addClass('Fade');
		 }, 250);
		 
		 
		 setTimeout(function () 
		{document.getElementById('SubCategory_4_4').style.display="block";
		$('#SubCategory_4_4').addClass('Fade');
		 }, 250);
		 
		 
		document.getElementById('open_4').style.display="block";
		$('#open_4').removeClass('boomSplashReverse');
		$('#open_4').addClass('boomSplash');
		document.getElementById('Category_4').style.zIndex="9";
	});
	$('#holder_4').mouseleave(function() {
		document.getElementById('open_4').style.display="none";
		document.getElementById('Category_4').style.zIndex="8";
	});
	
	$('#SubCategory_1_1').mouseover(function() {
		document.getElementById('SubCategory_1_1_open_1').style.display="block";
		$('#SubCategory_1_1_open_1').addClass('Fade');
	});
	$('#HolderForSubMain_1').mouseleave(function() {
		document.getElementById('SubCategory_1_1_open_1').style.display="none";
		$('#SubCategory_1_1_open_1').removeClass('Fade');
	});
	
	
	$('#SubCategory_1_2').mouseover(function() {
		document.getElementById('SubCategory_1_2_open_2').style.display="block";
		$('#SubCategory_1_2_open_2').addClass('Fade');
	});
	$('#HolderForSubMain_2').mouseleave(function() {
		document.getElementById('SubCategory_1_2_open_2').style.display="none";
		$('#SubCategory_1_2_open_2').removeClass('Fade');
	});
	
	
	$('#SubCategory_1_3').mouseover(function() {
		document.getElementById('SubCategory_1_3_open_3').style.display="block";
		$('#SubCategory_1_3_open_3').addClass('Fade');
	});
	$('#HolderForSubMain_3').mouseleave(function() {
		document.getElementById('SubCategory_1_3_open_3').style.display="none";
		$('#SubCategory_1_3_open_3').removeClass('Fade');
	});
	
	
	$('#SubCategory_1_4').mouseover(function() {
		document.getElementById('SubCategory_1_4_open_4').style.display="block";
		$('#SubCategory_1_4_open_4').addClass('Fade');
	});
	$('#HolderForSubMain_4').mouseleave(function() {
		document.getElementById('SubCategory_1_4_open_4').style.display="none";
		$('#SubCategory_1_4_open_4').removeClass('Fade');
	});
	
	
	$('#SubCategory_2_1').mouseover(function() {
		document.getElementById('SubCategory_2_1_open_1').style.display="block";
		$('#SubCategory_2_1_open_1').addClass('Fade');
	});
	$('#HolderForSubMain_5').mouseleave(function() {
		document.getElementById('SubCategory_2_1_open_1').style.display="none";
		$('#SubCategory_2_1_open_2').removeClass('Fade');
	});
	
	$('#SubCategory_2_2').mouseover(function() {
		document.getElementById('SubCategory_2_2_open_2').style.display="block";
		$('#SubCategory_2_2_open_2').addClass('Fade');
	});
	$('#HolderForSubMain_6').mouseleave(function() {
		document.getElementById('SubCategory_2_2_open_2').style.display="none";
		$('#SubCategory_2_2_open_2').removeClass('Fade');
	});
	
	$('#SubCategory_2_3').mouseover(function() {
		document.getElementById('SubCategory_2_3_open_3').style.display="block";
		$('#SubCategory_2_3_open_3').addClass('Fade');
	});
	$('#HolderForSubMain_7').mouseleave(function() {
		document.getElementById('SubCategory_2_3_open_3').style.display="none";
		$('#SubCategory_2_3_open_3').removeClass('Fade');
	});
	
	$('#SubCategory_2_4').mouseover(function() {
		document.getElementById('SubCategory_2_4_open_4').style.display="block";
		$('#SubCategory_2_4_open_4').addClass('Fade');
	});
	$('#HolderForSubMain_8').mouseleave(function() {
		document.getElementById('SubCategory_2_4_open_4').style.display="none";
		$('#SubCategory_2_4_open_4').removeClass('Fade');
	});
	
	
	$('#SubCategory_3_1').mouseover(function() {
		document.getElementById('SubCategory_3_1_open_1').style.display="block";
		$('#SubCategory_3_1_open_1').addClass('Fade');
	});
	$('#HolderForSubMain_9').mouseleave(function() {
		document.getElementById('SubCategory_3_1_open_1').style.display="none";
		$('#SubCategory_3_1_open_2').removeClass('Fade');
	});
	
	$('#SubCategory_3_2').mouseover(function() {
		document.getElementById('SubCategory_3_2_open_2').style.display="block";
		$('#SubCategory_3_2_open_2').addClass('Fade');
	});
	$('#HolderForSubMain_10').mouseleave(function() {
		document.getElementById('SubCategory_3_2_open_2').style.display="none";
		$('#SubCategory_3_2_open_2').removeClass('Fade');
	});
	
	$('#SubCategory_3_3').mouseover(function() {
		document.getElementById('SubCategory_3_3_open_3').style.display="block";
		$('#SubCategory_3_3_open_3').addClass('Fade');
	});
	$('#HolderForSubMain_11').mouseleave(function() {
		document.getElementById('SubCategory_3_3_open_3').style.display="none";
		$('#SubCategory_3_3_open_3').removeClass('Fade');
	});
	
	$('#SubCategory_3_4').mouseover(function() {
		document.getElementById('SubCategory_3_4_open_4').style.display="block";
		$('#SubCategory_3_4_open_4').addClass('Fade');
	});
	$('#HolderForSubMain_12').mouseleave(function() {
		document.getElementById('SubCategory_3_4_open_4').style.display="none";
		$('#SubCategory_3_4_open_4').removeClass('Fade');
	});
	
	
	$('#SubCategory_4_1').mouseover(function() {
		document.getElementById('SubCategory_4_1_open_1').style.display="block";
		$('#SubCategory_4_1_open_1').addClass('Fade');
	});
	$('#HolderForSubMain_13').mouseleave(function() {
		document.getElementById('SubCategory_4_1_open_1').style.display="none";
		$('#SubCategory_4_1_open_2').removeClass('Fade');
	});
	
	$('#SubCategory_4_2').mouseover(function() {
		document.getElementById('SubCategory_4_2_open_2').style.display="block";
		$('#SubCategory_4_2_open_2').addClass('Fade');
	});
	$('#HolderForSubMain_14').mouseleave(function() {
		document.getElementById('SubCategory_4_2_open_2').style.display="none";
		$('#SubCategory_4_2_open_2').removeClass('Fade');
	});
	
	$('#SubCategory_4_3').mouseover(function() {
		document.getElementById('SubCategory_4_3_open_3').style.display="block";
		$('#SubCategory_4_3_open_3').addClass('Fade');
	});
	$('#HolderForSubMain_15').mouseleave(function() {
		document.getElementById('SubCategory_4_3_open_3').style.display="none";
		$('#SubCategory_4_3_open_3').removeClass('Fade');
	});
	
	$('#SubCategory_4_4').mouseover(function() {
		document.getElementById('SubCategory_4_4_open_4').style.display="block";
		$('#SubCategory_4_4_open_4').addClass('Fade');
	});
	$('#HolderForSubMain_16').mouseleave(function() {
		document.getElementById('SubCategory_4_4_open_4').style.display="none";
		$('#SubCategory_4_4_open_4').removeClass('Fade');
	});
	
	
	
	

	
	
	$('#Category_4').mouseover(function() {
		document.getElementById('open_4').style.display="block";
		document.getElementById('Category_4').style.zIndex="9";
	});
	$('#holder_4').mouseleave(function() {
		document.getElementById('open_4').style.display="none";
		document.getElementById('Category_4').style.zIndex="6";
	});
	
	
	
	
	
	
	$('#LogIn').click(function() {
		document.getElementById('LogInOpen').style.display="block";
		document.getElementById('ForClose').style.display="block";
		$('#LogInOpen').addClass('animationForLogIn');
		$('#LogInOHolder').fadeIn(500);
    });
	
	$('#ForClose').click(function() {
		document.getElementById('LogInOpen').style.display="none";
		document.getElementById('ForClose').style.display="none";
		document.getElementById('LogInOHolder').style.display="none";
		$('#LogInOpen').removeClass('animationForLogIn');
	});
	
	$('#CloseOnly').click(function() {
		document.getElementById('LogInOpen').style.display="none";
		document.getElementById('ForClose').style.display="none";
		document.getElementById('LogInOHolder').style.display="none";
		$('#LogInOpen').removeClass('animationForLogIn');
	});
	
	
	$('#Nearest').click(function() {
		document.getElementById('NearestContainer').style.display="block";
		$('#NearestContainer').addClass('animationForLogIn');
		$('#NearestHolder').fadeIn(500);
    });
	
	$('#CloseNearest').click(function() {
		document.getElementById('NearestContainer').style.display="none";
		document.getElementById('NearestHolder').style.display="none";
		$('#NearestContainer').removeClass('animationForLogIn');
	});
	
	$('#CloseNear').click(function() {
		document.getElementById('NearestContainer').style.display="none";
		document.getElementById('NearestHolder').style.display="none";
		$('#NearestContainer').removeClass('animationForLogIn');
	});
	
	
	
	
	
	
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
	
	
	 // Ripple
    $('[ripple]').on('click', function (e) {
        var rippleDiv = $('<div class="ripple" />'),
            rippleOffset = $(this).offset(),
            rippleY = e.pageY - rippleOffset.top,
            rippleX = e.pageX - rippleOffset.left,
            ripple = $('.ripple');

        rippleDiv.css({
            top: rippleY - (ripple.height() / 2),
            left: rippleX - (ripple.width() / 2),
            background: $(this).attr("ripple-color")
        }).appendTo($(this));

        window.setTimeout(function () {
            rippleDiv.remove();
        }, 1500);
    });
	
});


