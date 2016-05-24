define(function (require, exports, module) {
    var self = exports;
    //--

    self.init = function (option, callback) {
        option = option || {};
        self.service = require('service');
        self.ui = require('ui');
        //应用认证处理
        if (!option.appKey) {
            throw "缺少 appKey.";
            return;
        } else {
            self.service.appKey = option.appKey;
        }
        //身份认证处理
        if (option.token) {
            self.service.token = option.token;
            if (callback) return callback(self);
        } else if (option.account && option.password) {
            self.service.login(self.service.appKey, self.service.token, {
                account: option.account,
                password: option.password
            }, function (rs) {
                if (rs.State == self.service.state.success) {
                    self.service.token = rs.Data.Token;
                    if (callback) return callback();
                } else {
                    throw "身份验证失败," + rs.Data;
                    return;
                }
            });
        } else {
            throw "缺少必要的 token 或 account + password.";
            return;
        }
    };

    self.dispose = function (callback) {
        if (callback) callback(self);
    };
    //--
    var loadReady = module.uri.split('?')[1];
    if (loadReady && window[loadReady]) {
        window.Pro = self
        window[loadReady](self);
    }

});
