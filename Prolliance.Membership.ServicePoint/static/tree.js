define(function (require,exports,module) {
    "require:nomunge,exports:nomunge,module:nomunge";

    var $=require('mokit/jquery');

    var option={};

    var toggle = function (node) {
        var handler = node.find('.node_handler');
        var childNodes = node.children().filter('ul')
        if (node.hasClass('node_close')) {
            node.removeClass("node_close");
            node.addClass("node_open");
            if (option.onOpen) option.onOpen(node[0]);
        }
        else {
            node.removeClass("node_open");
            node.addClass("node_close");
            if (option.onClose) option.onClose(node[0]);
        }
    };
    $(function () {
        $(document.body).on('click', '.easy-tree .node_handler', function () {
            toggle($(this).parent());
        });
    });
    //
    exports.use=function (el,_option) {
        option=_option||{};
        $(el).addClass('easy-tree');
    };
});
