var Nobita = {
	parseQueryString: function(){
		var str = window.location.search.toLowerCase();
		var objURL = {};
		str.replace(
			new RegExp("([^?=&]+)(=([^&]*))?", "g"),
			function($0, $1, $2, $3) {
				objURL[$1] = $3;
			});
		return objURL;
	},
	slug: function(str){
		// Change handleize
		str = str.toLowerCase();
		str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
		str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
		str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
		str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
		str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
		str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
		str = str.replace(/đ/g, "d");
		str = str.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'| |\"|\&|\#|\[|\]|~|$|_/g, "-");
		str = str.replace(/-+-/g, "-"); //thay thế 2- thành 1- 
		str = str.replace(/^\-+|\-+$/g, "");
		return str;
	},
	slug_words: function(str) {
		str = str.toLowerCase();
		str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
		str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
		str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
		str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
		str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
		str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
		str = str.replace(/đ/g, "d");
		str = str.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'| |\"|\&|\#|\[|\]|~|$|_/g, " ");
		str = str.replace(/-+-/g, ""); //thay thế 2- thành 1- 
		str = str.replace(/^\-+|\-+$/g, "");
		return str;
	},
	fixHeightProduct: function (data_parent, data_target, data_image) {
		var box_height = 0;
		var box_image = 0;
		var boxtarget = data_parent + ' ' + data_target;
		var boximg = data_parent + ' ' + data_target + ' ' + data_image;
		jQuery(boximg).css('height', 'auto');
		jQuery($(boxtarget)).css('height', 'auto');
		jQuery($(boxtarget)).removeClass('fixheight');
		jQuery($(boxtarget)).each(function() {
			if (jQuery(this).find($(data_image)).height() > box_image) {
				box_image = jQuery(this).find($(data_image)).height();
			}
		});
		if (box_image > 0) {
			jQuery(boximg).height(box_image);
		}
		jQuery($(boxtarget)).each(function() {
			if (jQuery(this).height() > box_height) {
				box_height = jQuery(this).height();
			}
		});
		jQuery($(boxtarget)).addClass('fixheight');
		if (box_height > 0) {
			jQuery($(boxtarget)).height(box_height);
		}
		try {
			fixheightcallback();
		} catch (ex) {}
	},
	clone_item_view: function(product) { 
		item_product = $('#clone-item .item');
		item_product.find('.title-review').html(product.title);
		item_product.find('.url-product').attr('href', product.url).attr('title',product.title);
		item_product.find('.price-review').html(product.price);
		if (product.compare_at_price != '' && product.hasSale) {
			item_product.find('.price-sale-review').html(product.compare_at_price).removeClass('hidden');
		} else {
			item_product.find('.price-sale-review').addClass('hidden').html('');
		}
		if (product.featured_image != '') {
			item_product.find('.box-image').find('img').attr('src',Haravan.resizeImage(product.featured_image, 'medium')).attr('alt',product.title);
		} else {
			item_product.find('.box-image').find('img').attr('src','//theme.hstatic.net/1000160337/1000373353/14/image-empty.jpg?v=166').attr('alt',product.title);
		}
		item_product.clone().removeClass('hidden').appendTo('#owl-demo-daxem > .product-item')
	},
	setCookiePopup: function (nameCookie, value, exdays) {
		var d = new Date();
		d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
		var expires = "expires="+ d.toUTCString();
		document.cookie = nameCookie + "=" + value + ";" + expires + ";path=/";
	},
	getCookiePopup: function (nameCookie) {
		var name = nameCookie + "=";
		var ca = document.cookie.split(';');
		for(var i = 0; i < ca.length; i++) {
			var c = ca[i];
			while (c.charAt(0) == ' ') {
				c = c.substring(1);
			}
			if (c.indexOf(name) == 0) {
				return c.substring(name.length, c.length);
			}
		}
		return "";
	},
	changeTemplate: function(element){
		// Change grid list in collection
		if ( $(element).hasClass('active') ) {

		} else {
			$('#event-grid > div:not(.clear-ajax)').hide();
			$('.box-product-lists > .loadmore, .box-product-lists > .pagination-default').hide();
			$('.icon-loading').show();
			$('.btn-change-list').removeClass('active');
			$(element).addClass('active');
			if( $(element).attr('data-template') == 'template-list' ) {
				$('#event-grid').addClass('template-list');
			} else {
				$('#event-grid').removeClass('template-list');
			}
		}
		jQuery('#event-grid').imagesLoaded(function() {
			$('.icon-loading').hide();
			$('#event-grid > div:not(.clear-ajax)').show();
			$('.box-product-lists > .loadmore, .box-product-lists > .pagination-default').show();
			jQuery(window).resize();
		});
	},
	filterItemInList: function(object){
		// Keyup find item in list filter collection
		q = object.val().toLowerCase();
		object.parent().next().find('li').show();
		if (q.length > 0) {
			object.parent().next().find('li').each(function() {
				if ($(this).find('label').attr("data-filter").indexOf(q) == -1)
					$(this).hide();
			})
		}
	},
	filterItemInList_2: function(object){
		// Keyup find item in list filter collection
		q = object.val().toLowerCase();
		q = slug_words(q).trimRight();
		object.parent().next().find('li').show();
		if (q.length > 0) {
			object.parent().next().find('li').each(function() {
				if (slug_words($(this).find('span').html()).indexOf(q) == -1)
					$(this).hide();
			})
		}
	},
	checkItemOwlShow: function(object,tab,a,b,c,d){
		// Check owl item next/prev show or hide
		if ( tab == 'tab' ) {
			item = object.find('.active').find('.owl-carousel');
		} else {
			item = object.find('.owl-carousel');
		}	
		if ( item.find('.owl-item.active').length < a && $(window).width() >= 1200 ) {
			item.find('.owl-controls').hide();
		}
		if ( item.find('.owl-item.active').length < b && $(window).width() >= 992 && $(window).width() < 1199 ) {
			item.find('.owl-controls').hide();
		}
		if ( item.find('.owl-item.active').length < c && $(window).width() >= 768 && $(window).width() < 991 ) {
			item.find('.owl-controls').hide();
		}
		if ( item.find('.owl-item.active').length < d && $(window).width() < 768 ) {
			item.find('.owl-controls').hide();
		}
	},
	destroyResize: function(url){
		// Destroy resize image
		if ( url != undefined ) {
			if ( url.indexOf('_pico') != -1 || url.indexOf('_icon') != -1 || url.indexOf('_thumb') != -1
					|| url.indexOf('_small') != -1 || url.indexOf('_compact') != -1 || url.indexOf('_medium') != -1
					|| url.indexOf('_large') != -1 || url.indexOf('_grande') != -1 || url.indexOf('_1024x1024') != -1
					|| url.indexOf('_2048x2048') != -1 || url.indexOf('_master') != -1 ) {		
				link_image = (url.split('_')[url.split('_').length - 1]).split('.')[0];
				switch (link_image) {
					case 'pico': 
						link_image = url.split('_pico').join('').replace('http:','').replace('https:','');;
						break;
					case 'icon': 
						link_image = url.split('_icon').join('').replace('http:','').replace('https:','');;
						break;
					case 'thumb': 
						link_image = url.split('_thumb').join('').replace('http:','').replace('https:','');;
						break;
					case 'small':
						link_image = url.split('_small').join('').replace('http:','').replace('https:','');; 
						break;
					case 'compact': 
						link_image = url.split('_compact').join('').replace('http:','').replace('https:','');;
						break;
					case 'medium': 
						link_image = url.split('_medium').join('').replace('http:','').replace('https:','');;
						break;
					case 'large': 
						link_image = url.split('_large').join('').replace('http:','').replace('https:','');;
						break;
					case 'grande': 
						link_image = url.split('_grande').join('').replace('http:','').replace('https:','');;
						break;
					case '1024x1024': 
						link_image = url.split('_1024x1024').join('').replace('http:','').replace('https:','');;
						break;
					case '2048x2048': 
						link_image = url.split('_2048x2048').join('').replace('http:','').replace('https:','');;
						break;
					case 'master':
						link_image = url.split('_master').join('').replace('http:','').replace('https:','');;
						break;
				}
				return link_image;
			}
			return url;
		}
	},
	buy_now: function(id){
		// Add a product in checkout
		var quantity = 1;
		var params = {
			type: 'POST',
			url: '/cart/add.js',
			data: 'quantity=' + quantity + '&id=' + id,
			dataType: 'json',
			success: function(line_item) {
				window.location = '/checkout';
			},
			error: function(XMLHttpRequest, textStatus) {
				Haravan.onError(XMLHttpRequest, textStatus);
			}
		};
		jQuery.ajax(params);
	},
	add_item: function(id){
		// Add a product in cart
		var quantity = 1;
		var params = {
			type: 'POST',
			url: '/cart/add.js',
			data: 'quantity=' + quantity + '&id=' + id,
			dataType: 'json',
			success: function(line_item) {
				window.location = '/cart';
			},
			error: function(XMLHttpRequest, textStatus) {
				Haravan.onError(XMLHttpRequest, textStatus);
			}
		};
		jQuery.ajax(params);
	},
	plusQuantity: function(){
		// Plus number quantiy product detail 
		if ( jQuery('input[name="quantity"]').val() != undefined ) {
			var currentVal = parseInt(jQuery('input[name="quantity"]').val());
			if (!isNaN(currentVal)) {
				jQuery('input[name="quantity"]').val(currentVal + 1);
			} else {
				jQuery('input[name="quantity"]').val(1);
			}
		}else {
			console.log('error: Not see elemnt ' + jQuery('input[name="quantity"]').val());
		}
	},
	minusQuantity: function(){
		// Minus number quantiy product detail 
		if ( jQuery('input[name="quantity"]').val() != undefined ) {
			var currentVal = parseInt(jQuery('input[name="quantity"]').val());
			if (!isNaN(currentVal) && currentVal > 1) {
				jQuery('input[name="quantity"]').val(currentVal - 1);
			}
		}else {
			console.log('error: Not see elemnt ' + jQuery('input[name="quantity"]').val());
		}
	},
	searchSmart: function(){
		$.fn.smartSearch = function(_option) {
			var o, issending = false,
					timeout = null;
			var option = {
				smartoffset: true, //auto calc offset
				searchwhen: 'keyup', //0: after keydown, 1: after keypress, after space
				searchdelay: 500, //delay time before load data
			};
			if (typeof(_option) !== 'undefined') {
				$.each(_option, function(i, v) {
					if (typeof(_option[i]) !== 'undefined') option[i] = _option[i];
				})
			}
			o = $(this);
			o.attr('autocomplete', 'off');
			this.bind(option.searchwhen, function(event) {
				if (event.keyCode == 38 || event.keyCode == 40) {
					//return selectSuggest(event.keyCode);
				} else {
					$(".smart-search-wrapper").remove();
					clearTimeout(timeout);
					timeout = setTimeout(l, option.searchdelay, $(this).val());
				}
			});
			var l = function(t) {
				if (t != '') {
					if (issending) return this;
					issending = true;
					$.ajax({
						url: "/search?q=filter=(title:product**" + t +")&view=smart-search",
						dataType: "JSON",
						async: true,
						success: function(data) {
							$('.search-suggest').append("<div class='smart-search-wrapper'></div>");
							$.each(data, function(i, v) {
								var html_item = "<a href=" + v.url + " title='" + v.title +"'><div class='flexbox-grid-default'>";
								html_item = html_item + "<div class='flexbox-auto'><div class='search-image'><img src=" + v.featured_image + " alt='" + v.title +"' /></div></div>";
								html_item = html_item + "<div class='flexbox-auto-main flexbox-align-self-center overflow-hidden'><h3 class='search-title overflow-hidden'>" + v.title + "</h3>";
								if ( v.price == 0 ) {
									html_item = html_item + "<div><span class='search-price'>Liên hệ</span></div></div>";
								} else {
									if ( v.compare_at_price > v.price ) {
										html_item = html_item + "<div><span class='search-price'>" + Haravan.formatMoney(v.price, formatMoney) + "</span><span class='search-price-compare'>" + Haravan.formatMoney(v.compare_at_price, formatMoney) + "</span></div></div>";
									} else {
										html_item = html_item + "<div><span class='search-price'>" + Haravan.formatMoney(v.price, formatMoney) + "</span></div></div>";
									}
								}

								html_item = html_item + "</div></a>";
								$(".smart-search-wrapper").append(html_item);
							});
							issending = false;
						}
					});
				}
			}
			return this;
		};
		$("form.smartSearch input[name='q']").smartSearch();
	},
	submitSearch: function(){
		$('form.smartSearch').submit(function(e){
			e.preventDefault();
			var query = $(".smartSearch input[name='q']").val();
			if( query != '') {
				window.location = "/search?q=filter=(title:product**" + query + ")&type=product";
			} else {
				alert('Hãy nhập tên sản phẩm bạn cần tìm.');
			}
		});
	},
	UpdateCartNoteToCheckout: function(form_id){
		var params = {
			type: 'POST',
			url: '/cart/update.js',
			data: jQuery('#' + form_id).serialize(),
			dataType: 'json',
			success: function(cart) {
				window.location = '/checkout';
			},
			error: function(XMLHttpRequest, textStatus) {
				Haravan.onError(XMLHttpRequest, textStatus);
			}
		};
		jQuery.ajax(params);
	}
}
jQuery(document).ready(function(){
	Nobita.searchSmart();
	// Image Product Loaded fix height
	jQuery('.box-product-lists .image-resize').imagesLoaded(function() {
		Nobita.fixHeightProduct('.box-product-lists','.product-resize','.image-resize');
		jQuery(window).resize(function() {
			Nobita.fixHeightProduct('.box-product-lists','.product-resize','.image-resize');
		});
	});



	// Active image thumb and change image featured ( product detail )
	jQuery(document).on("click", ".product-thumb a", function() {
		jQuery('.product-thumb').removeClass('active');
		jQuery(this).parents('.product-thumb').addClass('active');
		jQuery(".product-image-feature").attr("src",jQuery(this).attr("data-image"));
	});

});
function add_item_flytocart(variant_id,param){
	/*Add cart product loop*/
	var img_fly = $(param).parents('.product-detail').find('.product-image img');
	var cart_mini = $('.cart_header');
	$.ajax({
		url: '/cart/add.js',
		data: 'quantity=1&id=' + variant_id,
		dataType: 'json',
		type: 'POST',
		async: false,
		success: function(){
			flytocart(img_fly,cart_mini);
			setTimeout(function(){
				getCartAjax();
				//	$('#cart_popup').modal('show');
				//$('.modal-backdrop').css({'height':$(document).height()});
			},600);
		},
		error: function(XMLHttpRequest, txtStatus){
			Haravan.onError(XMLHttpRequest, txtStatus);
		}
	});
}
function changeImageUrl(e,url){
	jQuery(e).find("img").attr( "src", url );
}
function getCartAjax(){
	var cart = null;

	jQuery.getJSON('/cart.js', function(cart, txtStatus){
		if(cart.item_count != 0){
			jQuery('.cart_header').find('.cart_header_count2').html(cart.item_count);
			jQuery('.cart_header').find('.cart_price').html(Haravan.formatMoney(cart.total_price,'')+ "₫");
		}

	});
}
function flytocart(fly, flyingto){
	var flyto = $(flyingto);
	var flyclone = $(fly).clone();
	$(flyclone).css({'position':'absolute',"left":$(fly).offset().left + "px","top":$(fly).offset().top + "px","z-index":"9999"});
	$('body').append($(flyclone));
	var goX = $(flyingto).offset().left + ($(flyingto).width()/2);
	var goY = $(flyingto).offset().top + ($(flyingto).height()/2);

	$(flyclone).animate({
		left: goX,
		top: goY,
		width: $(fly).width()/3,
		height: $(fly).height()/3
	},1000,function(){
		$(flyclone).animate({'width': 0,'height': 0}, function () {$(this).remove()});
	})
}


/**menu*/
$(document).ready(function(){
	$("#pt_menu_link ul li").each(function(){
		var url = document.URL;
		$("#pt_menu_link ul li a").removeClass("act");
		$('#pt_menu_link ul li a[href="'+url+'"]').addClass('act');
	}); 

	$('.pt_menu_no_child').hover(function(){
		$(this).addClass("active");
	},function(){
		$(this).removeClass("active");
	})

	$('.pt_menu').hover(function(){
		if($(this).attr("id") != "pt_menu_link"){
			$(this).addClass("active");
		}
	},function(){
		$(this).removeClass("active");
	})

	$('.pt_menu').hover(function(){
		/*show popup to calculate*/
		$(this).find('.popup').css('display','inline-block');

		/* get total padding + border + margin of the popup */
		var extraWidth       = 0
		var wrapWidthPopup   = $(this).find('.popup').outerWidth(true); /*include padding + margin + border*/
		var actualWidthPopup = $(this).find('.popup').width(); /*no padding, margin, border*/
		extraWidth           = wrapWidthPopup - actualWidthPopup;    

		/* calculate new width of the popup*/
		var widthblock1 = $(this).find('.popup .block1').outerWidth(true);
		var widthblock2 = $(this).find('.popup .block2').outerWidth(true);
		var new_width_popup = 0;
		if(widthblock1 && !widthblock2){
			new_width_popup = widthblock1;
		}
		if(!widthblock1 && widthblock2){
			new_width_popup = widthblock2;
		}
		if(widthblock1 && widthblock2){
			if(widthblock1 >= widthblock2){
				new_width_popup = widthblock1;
			}
			if(widthblock1 < widthblock2){
				new_width_popup = widthblock2;
			}
		}
		var new_outer_width_popup = new_width_popup + extraWidth;

		/*define top and left of the popup*/
		var wraper = $('.pt_custommenu');
		var wWraper = wraper.outerWidth();
		var posWraper = wraper.offset();
		var pos = $(this).offset();

		var xTop = pos.top - posWraper.top + CUSTOMMENU_POPUP_TOP_OFFSET;
		var xLeft = pos.left - posWraper.left;
		if ((xLeft + new_outer_width_popup) > wWraper) xLeft = wWraper - new_outer_width_popup;

		$(this).find('.popup').css('top',xTop);
		$(this).find('.popup').css('left',xLeft);

		/*set new width popup*/
		$(this).find('.popup').css('width',new_width_popup);
		$(this).find('.popup .block1').css('width',new_width_popup);

		/*return popup display none*/
		$(this).find('.popup').css('display','none');

		/*show hide popup*/
		if(CUSTOMMENU_POPUP_EFFECT == 0) $(this).find('.popup').stop(true,true).slideDown('slow');
		if(CUSTOMMENU_POPUP_EFFECT == 1) $(this).find('.popup').stop(true,true).fadeIn('slow');
		if(CUSTOMMENU_POPUP_EFFECT == 2) $(this).find('.popup').stop(true,true).show('slow');
	},function(){
		if(CUSTOMMENU_POPUP_EFFECT == 0) $(this).find('.popup').stop(true,true).slideUp();
		if(CUSTOMMENU_POPUP_EFFECT == 1) $(this).find('.popup').stop(true,true).fadeOut('slow');
		if(CUSTOMMENU_POPUP_EFFECT == 2) $(this).find('.popup').stop(true,true).hide('fast');
	})

	/*-----mobile menu-----*/

	$("ul.mobilemenu li span.button-view1,ul.mobilemenu li span.button-view2").each(function(){
		$(this).append('<span class="ttclose"><a href="javascript:void(0)"></a></span>');
	});

	$('#wrap-ma-mobilemenu').css('display','none');

	$("ul.mobilemenu li.active").each(function(){
		$(this).children().next("ul").css('display', 'block');
	});
	$("ul.mobilemenu li ul").hide();


	$('span.button-view1 span').click(function() { 
		if ($(this).hasClass('ttopen')) {varche = true} else {varche = false};
		if(varche == false){
			$(this).addClass("ttopen");
			$(this).removeClass("ttclose");
			$(this).parent().parent().find('ul.level2').slideDown();
			varche = true;
		} else 
		{	
			$(this).removeClass("ttopen");
			$(this).addClass("ttclose");
			$(this).parent().parent().find('ul.level2').slideUp();
			varche = false;
		}
	});

	$('span.button-view2 span').click(function() { 
		if ($(this).hasClass('ttopen')) {varche = true} else {varche = false};
		if(varche == false){
			$(this).addClass("ttopen");
			$(this).removeClass("ttclose");
			$(this).parent().parent().find('ul.level3').slideDown();
			varche = true;
		} else 
		{	
			$(this).removeClass("ttopen");
			$(this).addClass("ttclose");
			$(this).parent().parent().find('ul.level3').slideUp();
			varche = false;
		}
	});


	//mobile
	$('.btn-navbar').click(function() {

		var chk = 0;
		if ( $('#navbar-inner').hasClass('navbar-inactive') && ( chk==0 ) ) {
			$('#navbar-inner').removeClass('navbar-inactive');
			$('#navbar-inner').addClass('navbar-active');
			$('#ma-mobilemenu').css('display','block');
			chk = 1;
		}
		if ($('#navbar-inner').hasClass('navbar-active') && ( chk==0 ) ) {
			$('#navbar-inner').removeClass('navbar-active');
			$('#navbar-inner').addClass('navbar-inactive');			
			$('#ma-mobilemenu').css('display','none');
			chk = 1;
		}
		//$('#ma-mobilemenu').slideToggle();
	}); 

	/*---end mobile menu -----*/




	/*-------------------------------------------------------------------------*/
	/*fixed sản phẩm liên quan*/
	if (navigator.userAgent.match(/(iPod|iPhone|iPad|Android)/)) {
		$(".detail-pr-sidebar").removeClass("detail-pr-sidebar");
	}
	function fixSidebar() {
		if ( !! $('.detail-pr-sidebar').length) {
			var product_info = $('.main-pr-page');
			var el = $('.detail-pr-sidebar');
			var stickyTop = (el.offset().top) + 0;
			$(window).scroll(function() { 
				var footerTop = ($('.section-map').offset().top) - 100;
				var stickyHeight = el.height();
				var height_info = product_info.height();
				var limit = footerTop - stickyHeight - 100;
				var windowTop = $(window).scrollTop();
				var windowsize = $(window).width();
				if (windowsize > 1170) {
					if (height_info <= stickyHeight) {} else {
						if (stickyTop < windowTop) {
							el.css({
								position: 'fixed',
								top: 10,
								width: '264.8',
							});
							$('.detail-pr-sidebar').show();
						} else {
							el.css({
								'position': 'static',
								'top': '0',
								'width': 'initial',
							});
							$('.detail-pr-sidebar').show();
						}
						if (limit < windowTop) {
							var diff = limit - windowTop;
							el.css({
								top: diff
							});
						}
					}
				}
			});
		}
	}
	fixSidebar();

	// Slider index
	
	var carol = $('#slide-home').owlCarousel({
		items: 1,
		dots: true,
		autoplay:true,
		autoplayTimeout:7000,
		loop:true,
		dots: true,
		nav: true,
		navText : ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"],
		responsive:{
			0:{
				items:1
			},
			768:{
				items:1
			},
			1024:{
				items:1
			},
			1200:{
				items:1
			}
		}
	});
	carol.each(function (index) {
		$(this).find('.owl-nav, .owl-dots').wrapAll("<div class='owl-controls'></div>");
	});

	var carol2 = $('#slide-home-mobile').owlCarousel({
		items: 1,
		dots: true,
		autoplay:true,
		autoplayTimeout:7000,
		loop: true,
		nav: true,
		dots:false,
		navText : ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"]

	});

	carol2.each(function (index) {
		$(this).find('.owl-nav, .owl-dots').wrapAll("<div class='owl-controls'></div>");
	});


	//thumb product
	var carolthumb = $('#thumb-product.owl-carousel').owlCarousel({
		items: 4,
		margin: 15,
		loop:true,
		nav:true,
		dots:false,
		navText : ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"],
		responsive:{
			0:{
				items:4
			},
			768:{
				items:4
			},
			1024:{
				items:4
			},
			1200:{
				items:4
			}
		}
	});
	carolthumb.each(function (index) {
		$(this).find('.owl-nav, .owl-dots').wrapAll("<div class='owl-controls'></div>");
	});


	/* Owl Carousel For All product slider, logo đối tác */

	if( $('.owl-carousel').length ){

		$('.owl-carousel').each(function(){
			var owl = $('.owl-carousel');
			$(this).owlCarousel({
				margin: 20,
				loop: true,
				autoplayTimeout: $(this).data('autotime'),
				smartSpeed:$(this).data('speed'), 
				autoplay : $(this).data('autoplay'),
				items    : $(this).data('carousel-items'),
				nav      : false,
				dots     : $(this).data('dots'),
				responsive: {

					0: {
						items: $(this).data('mobile'),
						margin: 0
					},
					768: {
						items: $(this).data('tablet')
					},
					992: {
						items: $(this).data('items')
					}
				}
			});    
		});
	}



	/*Menu mobile*/
	$("#trigger_click_mobile").click(function(e){
		e.preventDefault();
		$("#mobile_wrap_menu").toggleClass("show");
		$('#opacity').addClass("opacity_body");
		$('body').addClass("overflow_hidden");
	});
	$('#opacity, .close_menu').click(function(){
		$("#mobile_wrap_menu").removeClass("show");
		$('#opacity').removeClass("opacity_body");
		$('body').removeClass("overflow_hidden");
	});
	$(".more").on("click", function() {
		$("i", this).toggleClass("fa-plus fa-minus");
	});
	$('.ajax_qty .btn_plus').click(function(){
		var variant_id = $(this).data('id');
		plus_quantity($(this),variant_id);
	});
	$('.ajax_qty .btn_minus').click(function(){
		var variant_id = $(this).data('id');
		minus_quantity($(this),variant_id);
	});

if (navigator.userAgent.match(/(iPod|iPhone|iPad|Android)/)) {
	$(".bolocmobile").click(function(){
		$(".hidden-boloc-mobile").toggleClass("show-filter");
	});
}

});

