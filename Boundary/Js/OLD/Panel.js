// JavaScript Document

// JavaScript Document

var counterNext=0;

$(document).ready(function(){
	
	$('#Holder_BNavigation').click(function () {
		counterNext++;
		if (counterNext%2==1) {
		$('#Bottom_Navigation').addClass('Up');
		setTimeout(function () {document.getElementById('Sub_BNavigation_First').style.display="block"; }, 1000);
		setTimeout(function () {document.getElementById('Sub_BNavigation_Seconde').style.display="block"; }, 1000);
		setTimeout(function () {document.getElementById('Sub_BNavigation_Third').style.display="block"; }, 1000);
		setTimeout(function () {document.getElementById('Sub_BNavigation_Forth').style.display="block"; }, 1000);
		}
		if (counterNext%2==0) {
		$('#Bottom_Navigation').removeClass('Up');
		setTimeout(function () {document.getElementById('Sub_BNavigation_First').style.display="none"; }, 0);
		setTimeout(function () {document.getElementById('Sub_BNavigation_Seconde').style.display="none"; }, 0);
		setTimeout(function () {document.getElementById('Sub_BNavigation_Third').style.display="none"; }, 0);
		setTimeout(function () {document.getElementById('Sub_BNavigation_Forth').style.display="none"; }, 0);
		}
		
	});

});