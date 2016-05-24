/**
 * @preserve jQuery Plugin: xHashchange v0.2.2
 *
 * LICENSE: http://hail2u.mit-license.org/2009
 */

/*jslint indent: 2, browser: true */
/*global jQuery, $ */

(function ($) {
    "use strict";

    // Private: check whether onhashchange event is supported or not
    function hasOnhashchange() {
        return typeof window.onhashchange !== "undefined";
    }

    $.fn.hashchange = function (handler) {
        $(window).bind("hashchange", handler);

        return this;
    };

    $.xHashchange = function () {
        var o = $.xHashchange.defaults,
          hash = null,
          intervalID = null,
          interval = 0;

        if (hasOnhashchange()) {
            $(window).trigger("hashchange");
        } else {

            if (hash === null) {
                hash = location.hash;
            }

            if (intervalID !== null) {
                clearInterval(intervalID);
            }

            if (interval !== o.interval) {
                intervalID = setInterval(function () {
                    if (hash !== location.hash) {
                        hash = location.hash;
                        $(window).trigger("hashchange");
                    }
                }, o.interval);
                interval = o.interval;
            }
        }

        return this;
    };

    // Public: default options
    $.xHashchange.defaults = {
        interval: 500
    };

    $.xHashchange();
}(jQuery));