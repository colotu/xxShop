function PageLock(el, options) {
    // Becomes this.options
    var defaults = {
        color: 'white',
        bgColor: '#fff',
        background: 'url(/Scripts/jquery.pagelock/ajax-loader_blue.gif) no-repeat center center transparent',
        fontWeight: 'bold',
        duration: 800,
        opacity: 0.7,
        classOveride: false,
        text: ''
    }
    this.options = jQuery.extend(defaults, options);
    this.container = $(el);

    this.init = function () {
        var container = this.container;
        // Delete any other loaders
        this.remove();
        // Create the overlay
        var overlay = $('<div></div>').css({
            'background-color': this.options.bgColor,
            'opacity': this.options.opacity,
            'width': container.width(),
            'height': container.height(),
            'text-align': 'center',
            'position': 'absolute',
            'top': '0px',
            'left': '0px',
            'z-index': 99999
        }).addClass('ajax_overlay');
        // add an overiding class name to set new loader style
        if (this.options.classOveride) {
            overlay.addClass(this.options.classOveride);
        }
        // insert overlay and loader into DOM
        container.append(
            overlay.append(
                $('<div></div>').css({
                    'background': this.options.background,
                    'width': '100%',
                    'height': '100%',
                    'text-align': 'center',
                    'color': this.options.color,
                    'line-height': (container.height() - 40) + 'px',
                    'font-weight': this.options.fontWeight
                }).text(this.options.text)
            ).fadeIn(this.options.duration)
        );
    };

    this.remove = function () {
        var overlay = this.container.children(".ajax_overlay");
        if (overlay.length) {
            overlay.fadeOut(this.options.classOveride, function () {
                overlay.remove();
            });
        }
    }

    this.init();
}
    