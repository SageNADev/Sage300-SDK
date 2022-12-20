(function () {
    'use strict';
    /**  Trace log object*/
    var traceLog = {

        log: function(msg){
            this.write("log", msg);
        },

        info: function(msg){
            this.write("info", msg);
        },

        warn: function(msg){
            this.write("warn", msg);
        },

        error: function(msg){
            this.write("error", msg);
        },

        throwError: function(e){
            //this.write("error", e);
            throw e;
        },

        /**
         * Log the message to console 
         * @param {any} type Message type
         * @param {any} msg Message
         */
        write: function(type, msg){

            if (appconfig.trace.disabled){
                return;
            }

            switch (type){
                case "log":
                    if (appconfig.trace.log){console.log(msg)};
                    break;
                case "info":
                    if (appconfig.trace.info){console.info(msg)};
                    break;
                case "warn":
                    if (appconfig.trace.warn){console.warn(msg)};
                    break;
                case "error":
                    if (appconfig.trace.error){console.error(msg)};
                    break;
            }
        },
    };

    this.trace = helpers.View.extend({}, traceLog);

}).call(this);

