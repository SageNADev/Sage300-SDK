(function() {

    'use strict';

    let root = this;

    var MessageBus;

    if (typeof exports !== 'undefined') {
        MessageBus = exports;
    } else {
        MessageBus = root.MessageBus = {};
    }

    // Current version of the library. Keep in sync with `package.json`.
    MessageBus.VERSION = '0.0.1';

    var _messageBus = apputils.extend({}, Keeler.Events);

    /**
    * The MessageBus object
    *
    * @class MessageBus
    * @static
    **/
    var msg = MessageBus.msg = {

        /**
        * Binds callbacks to events on an object. `listenTo` and `stopListening` should be used instead of directly calling `on` and `off`
        * Simply route `on` calls to Keeler.Events.on(...) 
        *
        * @method on
        * @param {String} events A space-delimited list of events to listen to
        * @param {Function} callback The callback to invoke when the event(s) is fired
        * @param {Object} [context] The context in which to invoke the callback
        **/
        on: function(events, callback, context) {

            _messageBus.on(events, callback, context);
        },

        /**
        * Unbinds callbacks from events on an object. `stopListening` should be used instead of directly calling `off`
        * Simply route `off` calls to Keeler.Events.off(...)
        *
        * @method off
        * @param {String} [events='all'] A space-delimited list of events to remove from the object
        * @param {Function} [callback] The callback to remove from the event/object combination
        * @param {Object} [context] The context from which to remove the callback
        **/
        off: function(events, callback, context) {
            _messageBus.off(events, callback, context);
        },

        /**
        * Trigger callbacks on a particular event
        *
        * @method trigger
        * @param {String} event The event to trigger
        **/
        trigger: function ( /* event, [*args] */) {
            _messageBus.trigger.apply(_messageBus, arguments);
        },

        /**
        * Binds a callback using `on`, which is removed the first time it is triggered
        *
        * @method once
        * @param {String} event The event to attach to the object
        * @param {Function} callback The callback to fire when the event is triggered
        * @param {Object} context The context in which to fire the callback
        **/
        once: function (/* event, callback, context */) {
            throw new this.exception('Not Implemented: ' +
                'once can create zombie views if the event is never called. ' +
                'Use Keeler.listenTo and Keeler.stopListening in the callback instead');

        },

        /**
        * listenTo needs to be implemented by the Event objects, see keeler.js
        * 'MessageBus.listenTo' would not provide this and should not be used like this.
        **/
        listenTo: function () {
            throw new this.exception('Not Implemented: ' +
                'listenTo should be implemented by the Event object: ' +
                'Keeler.listenTo(MessageBus, \'event\', callback); ');
            
        },

        /**
        * stopListening needs to be implemented by the Event objects, see keeler.js
        * This removes the events from listenTo.
        * 'MessageBus.stopListening' would not provide this and should not be used like this.
        **/
        stopListening: function () {
            throw new this.exception('Not Implemented: ' +
                'stopListening should be implemented by the Event object: ' +
                'Keeler.stopListening(MessageBus, \'event\', callback); ');

        },

        exception: function (message) {
            this.message = message;
            this.name = 'UserException';
        }

    };
}).call(this);