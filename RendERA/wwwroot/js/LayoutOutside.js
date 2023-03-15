// jQuery
$(function () {

	var images = [
		'../../assets/img/1063.jpg',
		'../../assets/img/PLAYERUNKNOWNS-BATTLEGROUNDS-12937710.jpg',
		'../../assets/img/overwatch_4k-HD.jpg',
		'../../assets/img/186993.jpg',
		'../../assets/img/lords_of_the_fallen-5.jpg',
		'../../assets/img/thumb-1920-708533.jpg',
		'../../assets/img/18639.jpg',
		'../../assets/img/18664.jpg',
		'../../assets/img/warhammer-6.jpg',
		'../../assets/img/evolve-7.jpg',
		'../../assets/img/thumb-1920-602788.png']
	$('#container').append('<style>#container, .acceptContainer:before, #logoContainer:before {background: url(' + images[Math.floor(Math.random() * images.length)] + ') center fixed }');




	setTimeout(function () {
		//$('.logoContainer').transition({ scale: 1 }, 700, 'ease');
		setTimeout(function () {
			$('.logoContainer .logo').addClass('loadIn');
			setTimeout(function () {
				$('.logoContainer .text').addClass('loadIn');
				setTimeout(function () {
					$('.acceptContainer').transition({ height: '431.5px' });
					setTimeout(function () {
						$('.acceptContainer').addClass('loadIn');
						setTimeout(function () {
							$('.formDiv, form h1').addClass('loadIn');
						}, 500)
					}, 500)
				}, 500)
			}, 500)
		}, 1000)
	}, 10)

	// Switch to Forgot Password
	$('.forgotBtn').click(function () {
		$('#forgot').toggleClass('toggle');
	});

	// Switch to Register
	$('.registerBtn').click(function () {
		$('#register, #formContainer').toggleClass('toggle');
	});

});