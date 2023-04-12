$(document).ready(function(){
	//$('.tcg-elevator').tcgElevator();
	$('.elevator').tcgElevator({
		toggleSelector: '.panel-heading',
		contentSelector: '.panel-body',
		speed: 500,
		openingSpeed: 500,
		closingSpeed: 1000,
		openClass: 'open',
		openingClass: 'opening',
		closingClass: 'closing'
	});
	var closed = true;
	$('.toggle-all').click(function(e){
		var functionToCall = closed ? 'open' : 'close';
		closed = !closed;
		$('.tcg-elevator').tcgElevator(functionToCall);
	})
})
