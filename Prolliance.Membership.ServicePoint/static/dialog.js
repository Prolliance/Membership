(function (self, top, $) {
    //创建对话框对象
    var dialog = self.dialog = self.dialog || {};
    //解析 open 的选项参数
    function parseOptions(str) {
        str = str || "";
        var options = {};
        var list = str.split(',');
        for (var i in list) {
            var itemParts = list[i].split('=');
            options[itemParts[0]] = itemParts[1];
        }
        return options;
    }
    //公共变量
    var closeButtonImg = "data:image/gif;base64,R0lGODlhCwAQAPcAAD9fPwAAAP///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACH5BAEAAAIALAAAAAALABAAQAgrAAUIHEiQIIAABwsmHLiwoMOHDw82FDhxIsSLGB0utEgxQESPAjhmHPkwIAA7";
    var modalStyle = "z-index:999998;position:fixed;width:100%;height:100%;left:0px;top:0px;opacity:0.25;background:#888;";
    var dlgBorderStyle = "z-index:999999;background:#fff;position:fixed;border:solid 1px #bbb;left:50%;top:50%;box-shadow: 0 0 20px #777;";
    var closeButtonStyle = "position:absolute;right:10px;top:8px;cursor:pointer;";
    var dlgFrameStyle = "width:100%;height:100%;border:solid 1px #fff;border:none;";
    var alertStyle = 'z-index:999999;font-size:1.1em;position:fixed;top:50px;left:50%;padding:4px 10px; background:#f9edbe; word-break: break-all; width: auto !important; max-width: 400px !important; _width: 400px !important; border: 1px solid #fbda91 !important; -moz-box-shadow: 1px 2px 5px rgba(0,0,0,.5) !important; -webkit-box-shadow: 1px 2px 5px rgba(0,0,0,.5) !important; box-shadow: 1px 2px 5px rgba(0,0,0,.5) !important;  border-radius: 3px !important;';
    var confirmBarStyle = "position:absolute;width:100%;background:#eee;";
    var confirmButtonStyle = "padding:4px 12px;margin-left:5px;";
    //
    top.openers = top.openers || {};
    //方法
    dialog._remove = function (name) {
        //操作父窗口对象时防止IE莫明报错，很神奇 setTimeout 就不报错了。
        top.setTimeout(function () {
            $('.' + name, top.document).fadeOut(150, function () {
                $(this).remove();
            });
        }, 0);
    };
    dialog.open = function (url, target, options) {
        options = parseOptions(options);
        var dlg = $('<div class="' + target + '" style="' + dlgBorderStyle + '"><img name="' + target + '" style="' + closeButtonStyle + '" src="' + closeButtonImg + '"/><iframe frameborder="0" style="' + dlgFrameStyle + '"></iframe></div>', top.document);
        dlg.frame = dlg.find('iframe');
        dlg.modal = $('<div class="' + target + '" style="' + modalStyle + '"></div>', top.document);
        dlg.css({ width: options.width, height: options.height, marginLeft: -(options.width / 2), marginTop: -(options.height / 2) });
        dlg.frame.attr("name", target);
        dlg.frame.attr('src', url);
        dlg.closeButton = dlg.find("img");
        dlg.closeButton.on('click', function () {
            var name = $(this).attr('name');
            dialog._remove(name);
        });
        //创建时在 style 中写 display:none 神奇的 IE 会有文本框不能获得焦点编辑的问题
        $(top.document.body).append(dlg.modal);
        $(top.document.body).append(dlg);
        //创建并添到dom完成,再 .hide 则没有问题
        dlg.hide();
        top.setTimeout(function () {
            dlg.fadeIn(200, function () {
                dlg.frame.focus();
            });
        }, 25);
        top.openers[target] = self;
        return dlg;
    };
    dialog.close = function () {
        var name = $(self.frameElement).attr('name');
        dialog._remove(name);
    };
    dialog.getOpener = function () {
        var name = $(self.frameElement).attr('name');
        return top.openers[name];
    };
    dialog.confirm = function (msg, okCallback, cancelCallback) {
        var width = 400, height = 30;
        var dlg = $('<div style="' + dlgBorderStyle + ';padding:60px 10px;text-align:center;"><div style="' + confirmBarStyle + ';left:0px;top:0px;text-align:left;"><div style="padding:8px;">提示</div><img style="' + closeButtonStyle + '" src="' + closeButtonImg + '"/></div>' + msg + '<div style="' + confirmBarStyle + ';left:0px;bottom:0px;text-align:right;"><div style="padding:8px;"><input data-confirm-rs="1" type="button" value="确定" style="' + confirmButtonStyle + '"/><input data-confirm-rs="0" type="button" value="取消" style="' + confirmButtonStyle + '"/></div></div></div>', top.document);
        dlg.modal = $('<div style="' + modalStyle + '"></div>', top.document);
        dlg.css({ "width": width, "height": height }).css({ "marginLeft": -(dlg.outerWidth() / 2), "marginTop": -(dlg.outerHeight() / 2) });
        dlg._close = function (rs) {
            dlg.fadeOut(150, function () {
                dlg.remove();
                dlg.modal.remove();
            });
            if (rs === 1) {
                if (okCallback) okCallback();
            } else {
                if (cancelCallback) cancelCallback();
            }
        };
        dlg.closeButton = dlg.find("img");
        dlg.closeButton.on('click', function () {
            dlg._close(0);
        });
        dlg.find('[data-confirm-rs]').on('click', function () {
            var rs = parseInt($(this).attr('data-confirm-rs'));
            dlg._close(rs);
        });
        $(top.document.body).append(dlg.modal);
        $(top.document.body).append(dlg);
        dlg.fadeIn(200);
    };
    dialog.alert = function (msg, callback) {
        var dlgAlert = $("<div style='" + alertStyle + "'>" + msg + "</div>", top.document);
        $(top.document.body).append(dlgAlert);
        dlgAlert.css({ marginLeft: -(dlgAlert.outerWidth() / 2) });
        //用 top.setTimeout 防止子窗弹出了提示，却在提示窗消息前关闭，setTimeout 被销毁，而无法关闭 alert.
        top.setTimeout(function () {
            dlgAlert.remove();
        }, 1500);
    };
    if (self.AjaxEngine) {
        self.AjaxEngine.dialog = dialog;
    }
    //对话框内布局辅助
    $(function () {
        var ui = {
            "header": $('.dialog-header'),
            "content": $('.dialog-content'),
            "footer": $('.dialog-footer')
        };
        ui.resize = function () {
            var dlgHeight = $(self).height();
            ui.content.height(dlgHeight - ui.header.outerHeight() - ui.footer.outerHeight());
        };
        ui.resize();
        $(self).resize(ui.resize);
    });
}(this, this.top, this.jQuery));