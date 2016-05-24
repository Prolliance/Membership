define(['./lib/jquery'], function ($, require, exports, module) {
    var service = require("./service.ashx?client-script=core,agent");
    service.crossDomain = true;
    service.state = {
        success: 200,
        unAuth: 401,
        failed: 500
    };
    module.exports = service;
});