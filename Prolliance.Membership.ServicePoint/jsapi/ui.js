define(['./lib/jquery'], function ($, require, exports, module) {
    var self = exports;
    require("./lib/jqueryui");
    require("./styles/jqueryui.css");
    require("./styles/style.css");
    var grid = require('./lib/grid');
    var linq = require('./lib/linq');
    //--
    var tp = require("./lib/tp");
    var service = require("service");
    var dialogBuffer = require("./tpl/agent.ashx?name=dialog-user");

    self.selectUser = function (option) {
        option = option || {};
        service.getAppList(service.appKey, service.token, function (rs) {
            console.log(JSON.stringify(rs));
            if (self.selectUserElement) {
                self.selectUserElement.remove();
            }
            var data = rs.Data;
            //如果使用了过滤器
            if (option.filter) {
                data = linq.From(data).Where(option.filter).ToArray();
            }
            self.selectUserElement = $(tp.parse(dialogBuffer, data));
            $(document.body).append(self.selectUserElement);
            //显示
            self.selectUserElement.dialog({
                width: option.width || 600,
                height: option.height || 400,
                modal: option.modal,
                resizable: false,
                buttons: [{
                    text: '取消', click: function () {
                        if (option.onCancel) option.onCancel();
                        self.selectUserElement.dialog('close');
                    }
                }, {
                    text: '确定', click: function () {
                        if (option.onOk) option.onOk();
                    }
                }]
            });
            grid.use(self.selectUserElement);

        });
    };
    //--
});