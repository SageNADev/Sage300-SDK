(function () {
    'use strict';

    var root = this;

    var helpers = root.helpers = {};

    // Current version of the library. Keep in sync with `package.json`.
    helpers.VERSION = '0.0.1';

    // Require Underscore, if we're on the server, and it's not already present.
    //var _ = root._;
    
    var View = helpers.View = function (options) {
        //console.log("options: " + options);
    };

    // Set up all inheritable **View** properties and methods.
    //apputils.extend(View.prototype, keeler.Events, {
    // any 'thing' within the code block below is accessible from 
    // all static or instantiate object
    apputils.extend(View.prototype, {}, {

        tagName: 'div',

        render: function () {
            return this;
        },

    });
    

   /**
    * Helper function to correctly setup the prototype chain, for subclasses.
    * See "inheritenace" usage below.
    * Both instance and static are supported:
    * usage:
    * instance: 
    * var far = helpers.extend({foo});
    * new  far().foo;
    * static: 
    * var far = helpers.extend({}, {foo});
    * far.foo;
    * protoProps - this is a quick way to add prototype properties and methods from a prototype. These are made avialable to instantiated objects & .prototype.
    */
    var extend = helpers.extend = function (protoProps, staticProps) {
        var parent = this;
        var child;
        
        if (protoProps && apputils.has(protoProps, 'constructor')) {
            child = protoProps.constructor;
        } else {
            child = function () { return parent.apply(this, arguments); };
        }

        // Add static properties to the constructor function, if supplied.
        apputils.extend(child, parent, staticProps);

        // Set the prototype chain to inherit from `parent`, without calling
        // `parent`'s constructor function.
        var Surrogate = function () { this.constructor = child; };
        Surrogate.prototype = parent.prototype;
        child.prototype = new Surrogate();

        // Add prototype properties (instance properties) to the subclass,
        // if supplied.
        if (protoProps){
            // '__proto__' is Deprecated. therefore add child ref using newer standards
            // using protoExt will get access to base object from instantiated
            protoProps.protoExt = Object.getPrototypeOf(child.prototype);
            apputils.extend(child.prototype, protoProps);
        }

        // Set a convenience property in case the parent's prototype is needed
        // later. But "__super__" is not present for instantiate object
        child.__super__ = parent.prototype;

        return child;
    };

    /**
     *Setup "inheritance" to our view.
     * Eg:  helpers.View.extend({});
     */
    View.extend = extend;

    if (typeof define === 'function' && define.amd) {
        define('helpers', [], function () {
            return helpers;
        });
    }
}).call(this);