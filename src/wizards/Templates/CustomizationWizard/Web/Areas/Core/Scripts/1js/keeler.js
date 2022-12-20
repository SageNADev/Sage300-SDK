(function(){
    'use strict';
    
    // Save a reference to the global object (`window` in the browser, `exports`
    // on the server).
    let root = this;

    // Create local references to array methods we'll want to use later.
    let array = [];
    let slice = array.slice;

    // The top-level namespace. All public Keeler classes and modules will
    // be attached to this. Exported for both the browser and the server.
    var Keeler;
    if (typeof exports !== 'undefined') {
        Keeler = exports;
    } else {
        Keeler = root.Keeler = {};
    }

    // Current version of the library. Keep in sync with `package.json`.
    Keeler.VERSION = '0.0.1';

    const Events = Keeler.Events = {

        // Bind an event to a `callback` function. Passing `"all"` will bind
        // the callback to all events fired.
        on: function (name, callback, context) {
            if (!eventsApi(this, 'on', name, [callback, context]) || !callback) return this;
            this._events || (this._events = {});

            //when this._events[name] is undefinded set it to empty array
            let events = this._events[name] || (this._events[name] = []);

            let evt = apputils.find(events, function (event) {
                return event.ctx === context; 
            });

            if (apputils.isUndefined(evt)) {
                
                events.push({ callback: callback, context: this, ctx: context || this });
            } else {
                
                evt.callback = callback;
            }

            return this;
        },

        // Bind an event to only be triggered a single time. After the first time
        // the callback is invoked, it will be removed.
        once: function (name, callback, context) {
            if (!eventsApi(this, 'once', name, [callback, context]) || !callback) return this;
            let self = this;
            const once = apputils.once(function () {
                self.off(name, once);
                callback.apply(this, arguments);
            });
            once._callback = callback;
            return this.on(name, once, context);
        },

        // Remove one or many callbacks. If `context` is null, removes all
        // callbacks with that function. If `callback` is null, removes all
        // callbacks for the event. If `name` is null, removes all bound
        // callbacks for all events.
        off: function (name, callback, context) {
            let retain, ev, events, names, i, l, j, k;
            if (!this._events || !eventsApi(this, 'off', name, [callback, context])) return this;
            if (!name && !callback && !context) {
                this._events = {};
                return this;
            }

            names = name ? [name] : apputils.keys(this._events);
            for (i = 0, l = names.length; i < l; i++) {
                name = names[i];
                events = this._events[name];

                if (events) {
                    this._events[name] = retain = [];
                    if (callback || context) {
                        for (j = 0, k = events.length; j < k; j++) {
                            ev = events[j];
                            if ((callback && callback !== ev.callback && callback._callback !== ev.callback._callback) ||
                                (context && context !== ev.context)) {
                                retain.push(ev);
                            }
                        }
                    }
                    if (!retain.length) delete this._events[name];
                }
            }

            return this;
        },

        // Trigger one or many events, firing all bound callbacks. Callbacks are
        // passed the same arguments as `trigger` is, apart from the event name
        // (unless you're listening on `"all"`, which will cause your callback to
        // receive the true name of the event as the first argument).
        trigger: function (name) {
            if (!this._events) return this;
            let args = slice.call(arguments, 1);
            if (!eventsApi(this, 'trigger', name, args)) return this;
            var events = this._events[name];
            var allEvents = this._events.all;
            if (events) triggerEvents(events, args);
            if (allEvents) triggerEvents(allEvents, arguments);
            return this;
        },

        // Tell this object to stop listening to either specific events ... or
        // to every object it's currently listening to.
        stopListening: function (obj, name, callback) {
            let listeners = this._listeners;
            if (!listeners) return this;
            let deleteListener = !name && !callback;
            if (typeof name === 'object') callback = this;
            if (obj) (listeners = {})[obj._listenerId] = obj;
            for (let id in listeners) {
                listeners[id].off(name, callback, this);
                if (deleteListener) delete this._listeners[id];
            }
            return this;
        }

    };

    // Regular expression used to split event strings.
    const eventSplitter = /\s+/;

    // Implement fancy features of the Events API such as multiple event
    // names `"change blur"` and jQuery-style event maps `{change: action}`
    // in terms of the existing API.
    const eventsApi = function (obj, action, name, rest) {
        if (!name) return true;

        // Handle event maps.
        if (typeof name === 'object') {
            for (let key in name) {
                obj[action].apply(obj, [key, name[key]].concat(rest));
            }
            return false;
        }

        // Handle space separated event names.
        if (eventSplitter.test(name)) {
            let names = name.split(eventSplitter);
            for (let i = 0, l = names.length; i < l; i++) {
                obj[action].apply(obj, [names[i]].concat(rest));
            }
            return false;
        }

        return true;
    };

    // Optimized internal dispatch function for
    // triggering events. Tries to keep the usual cases speedy (most internal
    // Keeler events have 3 arguments).
    const triggerEvents = function (events, args) {
        let ev, i = -1, l = events.length, a1 = args[0], a2 = args[1], a3 = args[2];
        switch (args.length) {
            case 0: while (++i < l) (ev = events[i]).callback.call(ev.ctx); return;
            case 1: while (++i < l) (ev = events[i]).callback.call(ev.ctx, a1); return;
            case 2: while (++i < l) (ev = events[i]).callback.call(ev.ctx, a1, a2); return;
            case 3: while (++i < l) (ev = events[i]).callback.call(ev.ctx, a1, a2, a3); return;
            default: while (++i < l) (ev = events[i]).callback.apply(ev.ctx, args);
        }
    };

    const listenMethods = {listenTo: 'on', listenToOnce: 'once'};

    // Dynamcially add listenTo & listenToOnce methods to Events object
    // Using design principle to increase pluggability and to avoid duplicating code since `listenTo` internally uses `on` 
    // and `listenToOnce` internally uses`once`. Attached 'listeners' to *this* object to keep track of what it's listening to.
    apputils.each(listenMethods, function (implementation, method) {
        Events[method] = function (obj, name, callback, ctx) {
            let listeners = this._listeners || (this._listeners = {});
            let id = obj._listenerId || (obj._listenerId = apputils.uniqueId('l'));
            listeners[id] = obj;
            if (typeof name === 'object') callback = this;
            obj[implementation](name, callback, ctx || this);
            return this;
        };
    });

    // Allow the `Keeler` object to serve as a global event bus, for folks who
    // want global "pubsub" in a convenient place.
    apputils.extend(Keeler, Events);
  
}).call(this);