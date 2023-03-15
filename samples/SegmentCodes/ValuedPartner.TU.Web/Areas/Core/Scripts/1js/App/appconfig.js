(function () {
    'use strict';

    var appconfig = this.appconfig = {};

    const traceConfig = {
        disabled : false,
        log : true,
        info : true,
        warn : true,
        error : true
    };

    appconfig.trace = apputils.extend({}, traceConfig);

    const systemConfig = {
        service : 'SAGE300'
    };

    appconfig.system = apputils.extend({}, systemConfig);

    const responseConfig = {
        tag: 'resp'
    };

    appconfig.response = apputils.extend({}, responseConfig);

    if (typeof define === 'function' && define.amd) {
        define('appconfig', [], function () {
            return appconfig;
        });
    }

}).call(this);