;(function($, window, document, undefined) {

	"use strict";

	var pluginName = "tcgElevator";
	var defaults = {
		toggleSelector: '.tcg-elevator-toggle',
		contentSelector: '.tcg-elevator-content',
		speed: 400,
		openingSpeed: false,
		closingSpeed: false,
		openClass: 'tcg-elevator-open',
		openingClass: 'tcg-elevator-opening',
		closingClass: 'tcg-elevator-closing'
	};

	function Plugin (element, options)
	{
		var _this = this;
		var _element = element;
		var _$element = $(element);
		var _settings = $.extend({}, defaults, options);
		_settings['openingSpeed'] = _settings['openingSpeed'] === false ? _settings['speed'] : _settings['openingSpeed'];
		_settings['closingSpeed'] = _settings['closingSpeed'] === false ? _settings['speed'] : _settings['closingSpeed'];

		var _toggle = _$element.find(_settings['toggleSelector']);
		var _content = _$element.find(_settings['contentSelector']);

		var _doClose = false;
		var _animationRunning = false;

		init();

		function init()
		{
			_toggle = _$element.find(_settings['toggleSelector']);
			_content = _$element.find(_settings['contentSelector']);
			_content.hide();

			_toggle.click(function(e){
				_doClose = _$element.hasClass(_settings.openingClass) || _$element.hasClass(_settings.openClass)
				if(_doClose)
				{
					_this.close();
				}
				else
				{
					_this.open();
				}
			});
		}

		_this.open = function()
		{
			_$element.removeClass(_settings.closingClass)
			_$element.addClass(_settings.openingClass);
			_content.stop(true).slideDown(_settings['openingSpeed'], function(){
				_$element.removeClass(_settings.openingClass).addClass(_settings.openClass);
			});
		}
		_this.close = function()
		{
			_$element.removeClass(_settings.openingClass + ' ' + _settings.openClass);
			_$element.addClass(_settings.closingClass);
			_content.stop(true).slideUp(_settings['closingSpeed'], function(){
				_$element.removeClass(_settings.closingClass);
			});
		}
		_this.set = function(attr, value)
		{
			_settings[attr] = value;
		}

	}

	$.fn[pluginName] = function() {
		var args = arguments;
		return this.each(function() {
			if (!$.data(this, pluginName + '_instance'))
			{
				$.data(this, pluginName + '_instance', new Plugin(this, args[0]));
			}
			else
			{
				var _this = $.data(this, pluginName + '_instance');

				if(typeof args[0] === 'string' && typeof _this[args[0]] === 'function')
				{
					var functionArgs = [];
					for(var i = 1; i < args.length; i++)
					{
						functionArgs.push(args[i]);
					}
					_this[args[0]].apply(_this, functionArgs);
				}
			}
		});
	};
})(jQuery,window, document);
