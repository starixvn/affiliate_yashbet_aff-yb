/*!
 * clipboard.js v2.0.4
 * https://zenorocha.github.io/clipboard.js
 * 
 * Licensed MIT © Zeno Rocha
 */
(function webpackUniversalModuleDefinition(root, factory) {
	if(typeof exports === 'object' && typeof module === 'object')
		module.exports = factory();
	else if(typeof define === 'function' && define.amd)
		define([], factory);
	else if(typeof exports === 'object')
		exports["ClipboardJS"] = factory();
	else
		root["ClipboardJS"] = factory();
})(this, function() {
return /******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, { enumerable: true, get: getter });
/******/ 		}
/******/ 	};
/******/
/******/ 	// define __esModule on exports
/******/ 	__webpack_require__.r = function(exports) {
/******/ 		if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 			Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 		}
/******/ 		Object.defineProperty(exports, '__esModule', { value: true });
/******/ 	};
/******/
/******/ 	// create a fake namespace object
/******/ 	// mode & 1: value is a module id, require it
/******/ 	// mode & 2: merge all properties of value into the ns
/******/ 	// mode & 4: return value when already ns object
/******/ 	// mode & 8|1: behave like require
/******/ 	__webpack_require__.t = function(value, mode) {
/******/ 		if(mode & 1) value = __webpack_require__(value);
/******/ 		if(mode & 8) return value;
/******/ 		if((mode & 4) && typeof value === 'object' && value && value.__esModule) return value;
/******/ 		var ns = Object.create(null);
/******/ 		__webpack_require__.r(ns);
/******/ 		Object.defineProperty(ns, 'default', { enumerable: true, value: value });
/******/ 		if(mode & 2 && typeof value != 'string') for(var key in value) __webpack_require__.d(ns, key, function(key) { return value[key]; }.bind(null, key));
/******/ 		return ns;
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";
/******/
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = 0);
/******/ })
/************************************************************************/
/******/ ([
/* 0 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _clipboardAction = __webpack_require__(1);

var _clipboardAction2 = _interopRequireDefault(_clipboardAction);

var _tinyEmitter = __webpack_require__(3);

var _tinyEmitter2 = _interopRequireDefault(_tinyEmitter);

var _goodListener = __webpack_require__(4);

var _goodListener2 = _interopRequireDefault(_goodListener);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

/**
 * Base class which takes one or more elements, adds event listeners to them,
 * and instantiates a new `ClipboardAction` on each click.
 */
var Clipboard = function (_Emitter) {
    _inherits(Clipboard, _Emitter);

    /**
     * @param {String|HTMLElement|HTMLCollection|NodeList} trigger
     * @param {Object} options
     */
    function Clipboard(trigger, options) {
        _classCallCheck(this, Clipboard);

        var _this = _possibleConstructorReturn(this, (Clipboard.__proto__ || Object.getPrototypeOf(Clipboard)).call(this));

        _this.resolveOptions(options);
        _this.listenClick(trigger);
        return _this;
    }

    /**
     * Defines if attributes would be resolved using internal setter functions
     * or custom functions that were passed in the constructor.
     * @param {Object} options
     */


    _createClass(Clipboard, [{
        key: 'resolveOptions',
        value: function resolveOptions() {
            var options = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {};

            this.action = typeof options.action === 'function' ? options.action : this.defaultAction;
            this.target = typeof options.target === 'function' ? options.target : this.defaultTarget;
            this.text = typeof options.text === 'function' ? options.text : this.defaultText;
            this.container = _typeof(options.container) === 'object' ? options.container : document.body;
        }

        /**
         * Adds a click event listener to the passed trigger.
         * @param {String|HTMLElement|HTMLCollection|NodeList} trigger
         */

    }, {
        key: 'listenClick',
        value: function listenClick(trigger) {
            var _this2 = this;

            this.listener = (0, _goodListener2.default)(trigger, 'click', function (e) {
                return _this2.onClick(e);
            });
        }

        /**
         * Defines a new `ClipboardAction` on each click event.
         * @param {Event} e
         */

    }, {
        key: 'onClick',
        value: function onClick(e) {
            var trigger = e.delegateTarget || e.currentTarget;

            if (this.clipboardAction) {
                this.clipboardAction = null;
            }

            this.clipboardAction = new _clipboardAction2.default({
                action: this.action(trigger),
                target: this.target(trigger),
                text: this.text(trigger),
                container: this.container,
                trigger: trigger,
                emitter: this
            });
        }

        /**
         * Default `action` lookup function.
         * @param {Element} trigger
         */

    }, {
        key: 'defaultAction',
        value: function defaultAction(trigger) {
            return getAttributeValue('action', trigger);
        }

        /**
         * Default `target` lookup function.
         * @param {Element} trigger
         */

    }, {
        key: 'defaultTarget',
        value: function defaultTarget(trigger) {
            var selector = getAttributeValue('target', trigger);

            if (selector) {
                return document.querySelector(selector);
            }
        }

        /**
         * Returns the support of the given action, or all actions if no action is
         * given.
         * @param {String} [action]
         */

    }, {
        key: 'defaultText',


        /**
         * Default `text` lookup function.
         * @param {Element} trigger
         */
        value: function defaultText(trigger) {
            return getAttributeValue('text', trigger);
        }

        /**
         * Destroy lifecycle.
         */

    }, {
        key: 'destroy',
        value: function destroy() {
            this.listener.destroy();

            if (this.clipboardAction) {
                this.clipboardAction.destroy();
                this.clipboardAction = null;
            }
        }
    }], [{
        key: 'isSupported',
        value: function isSupported() {
            var action = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : ['copy', 'cut'];

            var actions = typeof action === 'string' ? [action] : action;
            var support = !!document.queryCommandSupported;

            actions.forEach(function (action) {
                support = support && !!document.queryCommandSupported(action);
            });

            return support;
        }
    }]);

    return Clipboard;
}(_tinyEmitter2.default);

/**
 * Helper function to retrieve attribute value.
 * @param {String} suffix
 * @param {Element} element
 */


function getAttributeValue(suffix, element) {
    var attribute = 'data-clipboard-' + suffix;

    if (!element.hasAttribute(attribute)) {
        return;
    }

    return element.getAttribute(attribute);
}

module.exports = Clipboard;

/***/ }),
/* 1 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _select = __webpack_require__(2);

var _select2 = _interopRequireDefault(_select);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

/**
 * Inner class which performs selection from either `text` or `target`
 * properties and then executes copy or cut operations.
 */
var ClipboardAction = function () {
    /**
     * @param {Object} options
     */
    function ClipboardAction(options) {
        _classCallCheck(this, ClipboardAction);

        this.resolveOptions(options);
        this.initSelection();
    }

    /**
     * Defines base properties passed from constructor.
     * @param {Object} options
     */


    _createClass(ClipboardAction, [{
        key: 'resolveOptions',
        value: function resolveOptions() {
            var options = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {};

            this.action = options.action;
            this.container = options.container;
            this.emitter = options.emitter;
            this.target = options.target;
            this.text = options.text;
            this.trigger = options.trigger;

            this.selectedText = '';
        }

        /**
         * Decides which selection strategy is going to be applied based
         * on the existence of `text` and `target` properties.
         */

    }, {
        key: 'initSelection',
        value: function initSelection() {
            if (this.text) {
                this.selectFake();
            } else if (this.target) {
                this.selectTarget();
            }
        }

        /**
         * Creates a fake textarea element, sets its value from `text` property,
         * and makes a selection on it.
         */

    }, {
        key: 'selectFake',
        value: function selectFake() {
            var _this = this;

            var isRTL = document.documentElement.getAttribute('dir') == 'rtl';

            this.removeFake();

            this.fakeHandlerCallback = function () {
                return _this.removeFake();
            };
            this.fakeHandler = this.container.addEventListener('click', this.fakeHandlerCallback) || true;

            this.fakeElem = document.createElement('textarea');
            // Prevent zooming on iOS
            this.fakeElem.style.fontSize = '12pt';
            // Reset box model
            this.fakeElem.style.border = '0';
            this.fakeElem.style.padding = '0';
            this.fakeElem.style.margin = '0';
            // Move element out of screen horizontally
            this.fakeElem.style.position = 'absolute';
            this.fakeElem.style[isRTL ? 'right' : 'left'] = '-9999px';
            // Move element to the same position vertically
            var yPosition = window.pageYOffset || document.documentElement.scrollTop;
            this.fakeElem.style.top = yPosition + 'px';

            this.fakeElem.setAttribute('readonly', '');
            this.fakeElem.value = this.text;

            this.container.appendChild(this.fakeElem);

            this.selectedText = (0, _select2.default)(this.fakeElem);
            this.copyText();
        }

        /**
         * Only removes the fake element after another click event, that way
         * a user can hit `Ctrl+C` to copy because selection still exists.
         */

    }, {
        key: 'removeFake',
        value: function removeFake() {
            if (this.fakeHandler) {
                this.container.removeEventListener('click', this.fakeHandlerCallback);
                this.fakeHandler = null;
                this.fakeHandlerCallback = null;
            }

            if (this.fakeElem) {
                this.container.removeChild(this.fakeElem);
                this.fakeElem = null;
            }
        }

        /**
         * Selects the content from element passed on `target` property.
         */

    }, {
        key: 'selectTarget',
        value: function selectTarget() {
            this.selectedText = (0, _select2.default)(this.target);
            this.copyText();
        }

        /**
         * Executes the copy operation based on the current selection.
         */

    }, {
        key: 'copyText',
        value: function copyText() {
            var succeeded = void 0;

            try {
                succeeded = document.execCommand(this.action);
            } catch (err) {
                succeeded = false;
            }

            this.handleResult(succeeded);
        }

        /**
         * Fires an event based on the copy operation result.
         * @param {Boolean} succeeded
         */

    }, {
        key: 'handleResult',
        value: function handleResult(succeeded) {
            this.emitter.emit(succeeded ? 'success' : 'error', {
                action: this.action,
                text: this.selectedText,
                trigger: this.trigger,
                clearSelection: this.clearSelection.bind(this)
            });
        }

        /**
         * Moves focus away from `target` and back to the trigger, removes current selection.
         */

    }, {
        key: 'clearSelection',
        value: function clearSelection() {
            if (this.trigger) {
                this.trigger.focus();
            }

            window.getSelection().removeAllRanges();
        }

        /**
         * Sets the `action` to be performed which can be either 'copy' or 'cut'.
         * @param {String} action
         */

    }, {
        key: 'destroy',


        /**
         * Destroy lifecycle.
         */
        value: function destroy() {
            this.removeFake();
        }
    }, {
        key: 'action',
        set: function set() {
            var action = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : 'copy';

            this._action = action;

            if (this._action !== 'copy' && this._action !== 'cut') {
                throw new Error('Invalid "action" value, use either "copy" or "cut"');
            }
        }

        /**
         * Gets the `action` property.
         * @return {String}
         */
        ,
        get: function get() {
            return this._action;
        }

        /**
         * Sets the `target` property using an element
         * that will be have its content copied.
         * @param {Element} target
         */

    }, {
        key: 'target',
        set: function set(target) {
            if (target !== undefined) {
                if (target && (typeof target === 'undefined' ? 'undefined' : _typeof(target)) === 'object' && target.nodeType === 1) {
                    if (this.action === 'copy' && target.hasAttribute('disabled')) {
                        throw new Error('Invalid "target" attribute. Please use "readonly" instead of "disabled" attribute');
                    }

                    if (this.action === 'cut' && (target.hasAttribute('readonly') || target.hasAttribute('disabled'))) {
                        throw new Error('Invalid "target" attribute. You can\'t cut text from elements with "readonly" or "disabled" attributes');
                    }

                    this._target = target;
                } else {
                    throw new Error('Invalid "target" value, use a valid Element');
                }
            }
        }

        /**
         * Gets the `target` property.
         * @return {String|HTMLElement}
         */
        ,
        get: function get() {
            return this._target;
        }
    }]);

    return ClipboardAction;
}();

module.exports = ClipboardAction;

/***/ }),
/* 2 */
/***/ (function(module, exports) {

function select(element) {
    var selectedText;

    if (element.nodeName === 'SELECT') {
        element.focus();

        selectedText = element.value;
    }
    else if (element.nodeName === 'INPUT' || element.nodeName === 'TEXTAREA') {
        var isReadOnly = element.hasAttribute('readonly');

        if (!isReadOnly) {
            element.setAttribute('readonly', '');
        }

        element.select();
        element.setSelectionRange(0, element.value.length);

        if (!isReadOnly) {
            element.removeAttribute('readonly');
        }

        selectedText = element.value;
    }
    else {
        if (element.hasAttribute('contenteditable')) {
            element.focus();
        }

        var selection = window.getSelection();
        var range = document.createRange();

        range.selectNodeContents(element);
        selection.removeAllRanges();
        selection.addRange(range);

        selectedText = selection.toString();
    }

    return selectedText;
}

module.exports = select;


/***/ }),
/* 3 */
/***/ (function(module, exports) {

function E () {
  // Keep this empty so it's easier to inherit from
  // (via https://github.com/lipsmack from https://github.com/scottcorgan/tiny-emitter/issues/3)
}

E.prototype = {
  on: function (name, callback, ctx) {
    var e = this.e || (this.e = {});

    (e[name] || (e[name] = [])).push({
      fn: callback,
      ctx: ctx
    });

    return this;
  },

  once: function (name, callback, ctx) {
    var self = this;
    function listener () {
      self.off(name, listener);
      callback.apply(ctx, arguments);
    };

    listener._ = callback
    return this.on(name, listener, ctx);
  },

  emit: function (name) {
    var data = [].slice.call(arguments, 1);
    var evtArr = ((this.e || (this.e = {}))[name] || []).slice();
    var i = 0;
    var len = evtArr.length;

    for (i; i < len; i++) {
      evtArr[i].fn.apply(evtArr[i].ctx, data);
    }

    return this;
  },

  off: function (name, callback) {
    var e = this.e || (this.e = {});
    var evts = e[name];
    var liveEvents = [];

    if (evts && callback) {
      for (var i = 0, len = evts.length; i < len; i++) {
        if (evts[i].fn !== callback && evts[i].fn._ !== callback)
          liveEvents.push(evts[i]);
      }
    }

    // Remove event from queue to prevent memory leak
    // Suggested by https://github.com/lazd
    // Ref: https://github.com/scottcorgan/tiny-emitter/commit/c6ebfaa9bc973b33d110a84a307742b7cf94c953#commitcomment-5024910

    (liveEvents.length)
      ? e[name] = liveEvents
      : delete e[name];

    return this;
  }
};

module.exports = E;


/***/ }),
/* 4 */
/***/ (function(module, exports, __webpack_require__) {

var is = __webpack_require__(5);
var delegate = __webpack_require__(6);

/**
 * Validates all params and calls the right
 * listener function based on its target type.
 *
 * @param {String|HTMLElement|HTMLCollection|NodeList} target
 * @param {String} type
 * @param {Function} callback
 * @return {Object}
 */
function listen(target, type, callback) {
    if (!target && !type && !callback) {
        throw new Error('Missing required arguments');
    }

    if (!is.string(type)) {
        throw new TypeError('Second argument must be a String');
    }

    if (!is.fn(callback)) {
        throw new TypeError('Third argument must be a Function');
    }

    if (is.node(target)) {
        return listenNode(target, type, callback);
    }
    else if (is.nodeList(target)) {
        return listenNodeList(target, type, callback);
    }
    else if (is.string(target)) {
        return listenSelector(target, type, callback);
    }
    else {
        throw new TypeError('First argument must be a String, HTMLElement, HTMLCollection, or NodeList');
    }
}

/**
 * Adds an event listener to a HTML element
 * and returns a remove listener function.
 *
 * @param {HTMLElement} node
 * @param {String} type
 * @param {Function} callback
 * @return {Object}
 */
function listenNode(node, type, callback) {
    node.addEventListener(type, callback);

    return {
        destroy: function() {
            node.removeEventListener(type, callback);
        }
    }
}

/**
 * Add an event listener to a list of HTML elements
 * and returns a remove listener function.
 *
 * @param {NodeList|HTMLCollection} nodeList
 * @param {String} type
 * @param {Function} callback
 * @return {Object}
 */
function listenNodeList(nodeList, type, callback) {
    Array.prototype.forEach.call(nodeList, function(node) {
        node.addEventListener(type, callback);
    });

    return {
        destroy: function() {
            Array.prototype.forEach.call(nodeList, function(node) {
                node.removeEventListener(type, callback);
            });
        }
    }
}

/**
 * Add an event listener to a selector
 * and returns a remove listener function.
 *
 * @param {String} selector
 * @param {String} type
 * @param {Function} callback
 * @return {Object}
 */
function listenSelector(selector, type, callback) {
    return delegate(document.body, selector, type, callback);
}

module.exports = listen;


/***/ }),
/* 5 */
/***/ (function(module, exports) {

/**
 * Check if argument is a HTML element.
 *
 * @param {Object} value
 * @return {Boolean}
 */
exports.node = function(value) {
    return value !== undefined
        && value instanceof HTMLElement
        && value.nodeType === 1;
};

/**
 * Check if argument is a list of HTML elements.
 *
 * @param {Object} value
 * @return {Boolean}
 */
exports.nodeList = function(value) {
    var type = Object.prototype.toString.call(value);

    return value !== undefined
        && (type === '[object NodeList]' || type === '[object HTMLCollection]')
        && ('length' in value)
        && (value.length === 0 || exports.node(value[0]));
};

/**
 * Check if argument is a string.
 *
 * @param {Object} value
 * @return {Boolean}
 */
exports.string = function(value) {
    return typeof value === 'string'
        || value instanceof String;
};

/**
 * Check if argument is a function.
 *
 * @param {Object} value
 * @return {Boolean}
 */
exports.fn = function(value) {
    var type = Object.prototype.toString.call(value);

    return type === '[object Function]';
};


/***/ }),
/* 6 */
/***/ (function(module, exports, __webpack_require__) {

var closest = __webpack_require__(7);

/**
 * Delegates event to a selector.
 *
 * @param {Element} element
 * @param {String} selector
 * @param {String} type
 * @param {Function} callback
 * @param {Boolean} useCapture
 * @return {Object}
 */
function _delegate(element, selector, type, callback, useCapture) {
    var listenerFn = listener.apply(this, arguments);

    element.addEventListener(type, listenerFn, useCapture);

    return {
        destroy: function() {
            element.removeEventListener(type, listenerFn, useCapture);
        }
    }
}

/**
 * Delegates event to a selector.
 *
 * @param {Element|String|Array} [elements]
 * @param {String} selector
 * @param {String} type
 * @param {Function} callback
 * @param {Boolean} useCapture
 * @return {Object}
 */
function delegate(elements, selector, type, callback, useCapture) {
    // Handle the regular Element usage
    if (typeof elements.addEventListener === 'function') {
        return _delegate.apply(null, arguments);
    }

    // Handle Element-less usage, it defaults to global delegation
    if (typeof type === 'function') {
        // Use `document` as the first parameter, then apply arguments
        // This is a short way to .unshift `arguments` without running into deoptimizations
        return _delegate.bind(null, document).apply(null, arguments);
    }

    // Handle Selector-based usage
    if (typeof elements === 'string') {
        elements = document.querySelectorAll(elements);
    }

    // Handle Array-like based usage
    return Array.prototype.map.call(elements, function (element) {
        return _delegate(element, selector, type, callback, useCapture);
    });
}

/**
 * Finds closest match and invokes callback.
 *
 * @param {Element} element
 * @param {String} selector
 * @param {String} type
 * @param {Function} callback
 * @return {Function}
 */
function listener(element, selector, type, callback) {
    return function(e) {
        e.delegateTarget = closest(e.target, selector);

        if (e.delegateTarget) {
            callback.call(element, e);
        }
    }
}

module.exports = delegate;


/***/ }),
/* 7 */
/***/ (function(module, exports) {

var DOCUMENT_NODE_TYPE = 9;

/**
 * A polyfill for Element.matches()
 */
if (typeof Element !== 'undefined' && !Element.prototype.matches) {
    var proto = Element.prototype;

    proto.matches = proto.matchesSelector ||
                    proto.mozMatchesSelector ||
                    proto.msMatchesSelector ||
                    proto.oMatchesSelector ||
                    proto.webkitMatchesSelector;
}

/**
 * Finds the closest parent that matches a selector.
 *
 * @param {Element} element
 * @param {String} selector
 * @return {Function}
 */
function closest (element, selector) {
    while (element && element.nodeType !== DOCUMENT_NODE_TYPE) {
        if (typeof element.matches === 'function' &&
            element.matches(selector)) {
          return element;
        }
        element = element.parentNode;
    }
}

module.exports = closest;


/***/ })
/******/ ]);
});
(function ($) {
    var _languageCode;
    var pagingInfoText = {
        'en-US': 'Page {0} of {1}',
        'bn-BD': 'পৃষ্ঠা {0} এর {1}',
        'hi-IN': 'पेज {0} का {1}',
        'km-KH': 'ទំព័រ {0} នៃ {1}',
        'ms-MY': 'Halaman {0} dari {1}',
        'id-ID': 'Halaman {0} dari {1}',
        'zh-CN': '从 {0} 页到 {1} 页',
        'zh-TW': '從 {0} 頁到 {1} 頁',
        'th-TH': 'หน้า {0} จาก {1}',
        'vi-VN': 'Trang {0} của {1}',
        'ko-KR': '페이지 {0} 중 {1}'
    };

    var defaultSupportedLangs = ['en-US', 'bn-BD', 'hi-IN', 'km-KH', 'ms-MY', 'id-ID', 'zh-CN', 'zh-TW', 'th-TH', 'vi-VN', 'ko-KR'];
    var defaultLang = 'en-US';

    var DEFAULT_PAGESIZE_LIST = [10, 20, 50, 100, 200];
    var defaultOption = {
        pageIndex: 0,
        pageSize: 20,
        totalItems: 0,
        pageSizeList: DEFAULT_PAGESIZE_LIST,
        supportedLangs: defaultSupportedLangs,
        languageCode: defaultLang,
        onChange: function (pageIndex, pageSize) { },
        onClickPrev: function () { },
        onClickLastPrev: function () { },
        onClickNext: function () { },
        onClickLastNext: function () { },
        onChangePageSize: function () { },
    };

    function getPagingInfoText(pageIndex, totalPage) {
        return (pagingInfoText[_languageCode] || pagingInfoText[defaultLang]).replace('{0}', pageIndex).replace('{1}', (totalPage || 1));
    }

    function buildPrevButton() {
        var html = ['<a href="javascript:;" title="First" class="icon glyphicon glyphicon-fast-backward page-first disable"></a>'];
        html.push('<a href="javascript:;" title="Prev" class="icon glyphicon glyphicon-triangle-left page-prev disable"></a>');

        return html.join('');
    }

    function buildNextButton(isDisabled) {
        var disable = isDisabled ? "disable" : "";
        var html = ['<a href="javascript:;" title="Next" class="icon glyphicon glyphicon-triangle-right page-next ' + disable + '"></a>'];
        html.push('<a href="javascript:;" title="Last" class="icon glyphicon glyphicon-fast-forward page-last ' + disable + '"></a>');

        return html.join('');
    }

    function buildPageInfo(totalPage) {
        var html = ['<div class="page-input">'];
        html.push('<span class="text-pageInfo">' + getPagingInfoText(1, totalPage) + '</span>')
        html.push('</div>');

        return html.join('');
    }

    function buildPageSizeList(pageSizeList, selectedItem) {
        var htmlPageSize = ['<select class="pagesize-select" >'];
        for (var i = 0; i < pageSizeList.length; i++) {
            var pageSize = pageSizeList[i];
            var selected = pageSize === selectedItem ? "selected" : "";
            htmlPageSize.push('<option value="' + pageSize + '" ' + selected + '>' + pageSize + '</option>');
        }
        htmlPageSize.push('</select>');

        return htmlPageSize.join('');
    }

    function updatePageInfo($pagerContainer, pageIndex, totalPage) {
        $pagerContainer.find('.text-pageInfo').text(getPagingInfoText(pageIndex, totalPage));
    }

    function togglePrevButton($pagerContainer, isEnable) {
        $pagerContainer.find('.page-first').toggleClass('disable', !isEnable);
        $pagerContainer.find('.page-prev').toggleClass('disable', !isEnable);
    }

    function toggleNextButton($pagerContainer, isEnable) {
        $pagerContainer.find('.page-next').toggleClass('disable', !isEnable);
        $pagerContainer.find('.page-last').toggleClass('disable', !isEnable);
    }

    function resetToInitialState($pagerContainer, isDisabledNextButton) {
        togglePrevButton($pagerContainer, false);
        toggleNextButton($pagerContainer, !isDisabledNextButton);
    }

    function calculateTotalPage(totalItems, pageSize) {
        return parseInt(totalItems / pageSize) + (totalItems % pageSize === 0 ? 0 : 1);
    }

    function isDisableNextButton(pageIndex, totalPage) {
        return totalPage === 0 || pageIndex === totalPage - 1;
    }

    var _methods = {
        init: function (option) {
            return this.each(function () {
                var mainOption = $.extend(defaultOption, option || {});

                var $self = $(this);
                $self.find("*").off();
                $self.html("");

                _languageCode = mainOption.supportedLangs.includes(mainOption.languageCode) ? mainOption.languageCode : defaultLang;
                var totalPage = calculateTotalPage(mainOption.totalItems, mainOption.pageSize);
                var pageInfo = {
                    pageIndex: mainOption.pageIndex,
                    pageSize: mainOption.pageSize,
                    totalItems: mainOption.totalItems,
                    totalPage: totalPage
                };

                $self.data('pageInfo', pageInfo);

                var htmlPrevButton = buildPrevButton();
                var htmlPagingInfo = buildPageInfo(totalPage);

                var disableNextButton = pageInfo.totalPage === 0 || pageInfo.pageIndex === pageInfo.totalPage - 1;
                var htmlNextButton = buildNextButton(disableNextButton);
                var htmlPageSizeList = buildPageSizeList(mainOption.pageSizeList, mainOption.pageSize);

                var $pagerContainer = $('<div class="pager-container"></div>');
                $pagerContainer.append(htmlPrevButton + htmlPagingInfo + htmlNextButton + htmlPageSizeList);
                $self.append($pagerContainer);

                $pagerContainer.on('click', '.page-first', function () {
                    var pageInfo = $self.data('pageInfo');
                    if (pageInfo.pageIndex === 0) {
                        return;
                    }

                    pageInfo.pageIndex = 0;
                    updatePageInfo($pagerContainer, pageInfo.pageIndex + 1, pageInfo.totalPage);
                    var disableNextButton = isDisableNextButton(pageInfo.pageIndex, pageInfo.totalPage);
                    resetToInitialState($pagerContainer, disableNextButton);

                    $self.data('pageInfo', pageInfo);

                    mainOption.onChange(pageInfo.pageIndex, pageInfo.pageSize);
                });

                $pagerContainer.on('click', '.page-prev', function () {
                    var pageInfo = $self.data('pageInfo');
                    if (pageInfo.pageIndex === 0) {
                        return;
                    }

                    pageInfo.pageIndex--;
                    if (pageInfo.pageIndex === 0) {
                        togglePrevButton($pagerContainer, false);
                    }

                    toggleNextButton($pagerContainer, true);
                    updatePageInfo($pagerContainer, pageInfo.pageIndex + 1, pageInfo.totalPage);

                    $self.data('pageInfo', pageInfo);
                    mainOption.onChange(pageInfo.pageIndex, pageInfo.pageSize);
                });

                $pagerContainer.on('click', '.page-next', function () {
                    var pageInfo = $self.data('pageInfo');
                    if (pageInfo.pageIndex === pageInfo.totalPage - 1) {
                        return;
                    }

                    pageInfo.pageIndex++;
                    if (pageInfo.pageIndex === pageInfo.totalPage - 1) {
                        toggleNextButton($pagerContainer, false);
                    }

                    togglePrevButton($pagerContainer, true);
                    $self.data('pageInfo', pageInfo);
                    updatePageInfo($pagerContainer, pageInfo.pageIndex + 1, pageInfo.totalPage);

                    mainOption.onChange(pageInfo.pageIndex, pageInfo.pageSize);
                });

                $pagerContainer.on('click', '.page-last', function () {
                    var pageInfo = $self.data('pageInfo');
                    if (pageInfo.pageIndex === pageInfo.totalPage - 1) {
                        return;
                    }

                    pageInfo.pageIndex = pageInfo.totalPage - 1;
                    updatePageInfo($pagerContainer, pageInfo.pageIndex + 1, pageInfo.totalPage);
                    togglePrevButton($pagerContainer, true);
                    toggleNextButton($pagerContainer, false);

                    $self.data('pageInfo', pageInfo);
                    mainOption.onChange(pageInfo.pageIndex, pageInfo.pageSize);
                });

                $pagerContainer.on('change', '.pagesize-select', function () {
                    var pageInfo = $self.data('pageInfo');
                    pageInfo.pageIndex = 0;
                    pageInfo.pageSize = parseInt($(this).val());

                    var totalPage = calculateTotalPage(pageInfo.totalItems, pageInfo.pageSize);
                    pageInfo.totalPage = totalPage;

                    updatePageInfo($pagerContainer, pageInfo.pageIndex + 1, pageInfo.totalPage);
                    var disableNextButton = isDisableNextButton(pageInfo.pageIndex, pageInfo.totalPage);
                    resetToInitialState($pagerContainer, disableNextButton);

                    $self.data('pageInfo', pageInfo);
                    mainOption.onChange(pageInfo.pageIndex, pageInfo.pageSize);
                });
            });
        },

        update: function (pageInfo) {
            var $self = $(this);
            var oldPageInfo = $self.data('pageInfo');
            var newPageInfo = $.extend({}, oldPageInfo, pageInfo);
            newPageInfo.totalPage = calculateTotalPage(newPageInfo.totalItems, newPageInfo.pageSize);

            var $pagerContainer = $self.find('.pager-container');
            updatePageInfo($pagerContainer, newPageInfo.pageIndex + 1, newPageInfo.totalPage);
            var disableNextButton = isDisableNextButton(newPageInfo.pageIndex, newPageInfo.totalPage);
            resetToInitialState($pagerContainer, disableNextButton);
            $self.data('pageInfo', newPageInfo);
        }
    }

    $.fn.pager = function (methodOrOptions) {
        if (_methods[methodOrOptions]) {
            return _methods[methodOrOptions].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof methodOrOptions === 'object' || !methodOrOptions) {
            // Default to "init"
            return _methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + methodOrOptions + ' does not exist on jQuery.pager');
        }
    };
})(jQuery)
var MarketingBase = (function ($) {
    isCancelConfirm = false;

    function getDownloadParam(trigger) {
        return {
            selectedLanguage: $(trigger).data("selected-language"),
            selectedDomainId: $(trigger).data("selected-domain-id"),
            selectedPage: $(trigger).data("selected-page"),
            selectedChannel: $(trigger).data("selected-channel"),
            selectedLinkType: $(trigger).data("selected-link-type")
        };
    }

    function getToggleBtnStatus(cheked) {
        return cheked ? "on" : "off";
    }

    function handleFalseResponse(response) {
        let data;
        switch (response.status) {
            case 403:
                data = response.json();
                alert(data.ErrMsg);
                location.reload();
                break;
            default:
                alert("Internal Error");
                break;
        }
    }

    var _me = {
        resiterQrcodeEvent: function () {
            new ClipboardJS('.btn-copy-link', {
                text: function (trigger) {
                    let parent = $(trigger).parents(".qr-item").first();
                    return parent.data("link");
                }
            });

            new ClipboardJS('.btn-copy-qr', {
                text: function (trigger) {
                    let imgTag = $(trigger).parents(".qr-action").prev().children("img.qrcode")[0];
                    return imgTag.outerHTML;
                }
            });

            $('.btn-save-img').click(function () {
                let downloadTag = document.createElement("a");
                let param = getDownloadParam($(this).parents(".qr-item")[0]);
                var query = $.param(param);
                let url = `${Site.resolveRelativeUrl("UserPanel/Marketing/DownloadQrCode")}?${query}`;
                downloadTag.href = url;
                downloadTag.click();
            });
        },

        initChannelSelectEvent: function () {
            if ($("#Channel").children().length > 1) {
                document.getElementById("Channel").selectedIndex = 1;
            }

            $("#Channel").focusin(function () {
                window.previousChannelIndex = this.selectedIndex;
                this.selectedIndex = -1;
            })

            $("#Channel").focusout(function () {
                if (this.selectedIndex <= 0) {
                    this.selectedIndex = window.previousChannelIndex === 0 ? 1 : window.previousChannelIndex;
                }
            })

            $("#Channel").change(function () {
                if ($(this).val() === 'add-new') {
                    let text = prompt(window.dic.enterChannel);
                    if (text) {
                        let addPromise = MarketingBase.addChannel(text);
                        addPromise.then(result => {
                            if (result) {
                                if (result.IsOk) {
                                    alert(window.dic.addChannelSuccess)
                                    $(this).append(new Option(result.Data.ChannelName, result.Data.ChannelCode, false, true));
                                    $("button .btn-gen").removeAttr("disabled");
                                }
                                else {
                                    alert(result.Message);
                                }
                            }
                        })
                    }
                }
                $(this).blur();
            })
        },

        addChannel: function (text) {
            Site.showLoader();
            let url = Site.resolveRelativeUrl("UserPanel/Marketing/AddChannel");
            let param = { channelName: text };
            let fetchData = fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8',
                    'X-Requested-With': 'XMLHttpRequest'
                },
                credentials: 'same-origin',
                body: new URLSearchParams(param)
            })
                .then((response => {
                    Site.hideLoader();
                    data = response.json();
                    if (response.ok) {
                        return data;
                    }
                    else {
                        handleFalseResponse(response);
                    }
                }))
            return Promise.resolve(fetchData);
        },

        renderLinkChannelStatisticPage: function () {
            let url = Site.resolveRelativeUrl("UserPanel/Marketing/RenderUsingMarketLinkPage");
            return fetch(url, {
                method: 'GET',
                credentials: 'same-origin',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8',
                    'X-Requested-With': 'XMLHttpRequest'
                },
            })
        },

        renderQrCodePage: function (param) {
            let url = new URL(Site.resolveRelativeUrl("UserPanel/Marketing/RenderQrCodePage"), window.location.origin);
            let query = new URLSearchParams(param);
            url.search = query.toString();
            return fetch(url, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8',
                    'X-Requested-With': 'XMLHttpRequest'
                },
                credentials: 'same-origin',
            })
        },

        toggleMarketingLink: function (param) {
            let url = Site.resolveRelativeUrl("UserPanel/Marketing/ToggleMarketingLink");
            return fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8',
                    'X-Requested-With': 'XMLHttpRequest'
                },
                credentials: 'same-origin',
                body: new URLSearchParams(param)
            })
        },

        registerToggleLinkEvent: function (callback) {
            $checkBox = $(".toggle-link");
            $checkBox.off("change");

            $checkBox.on("change", function () {
                if (isCancelConfirm) {
                    isCancelConfirm = false;
                    return;
                }
                let checked = $(this).prop('checked');
                let confirmText = checked
                    ? window.dic.confirmActiveLink
                    : window.dic.confirmDeactiveLink

                if (confirm(confirmText)) {
                    let parent = $(this).parents(".qr-item").first();
                    let param = {
                        ChannelCode: parent.data("selected-channel"),
                        LanguageCode: parent.data("selected-language"),
                        AffRefLink: parent.data("link"),
                        IsOn: checked
                    }

                    _me.toggleMarketingLink(param)
                        .then(response => {
                            if (response.ok) {
                                $(parent).find("button").prop('disabled', !checked);
                                $(parent).find(".qr-img").toggleClass("disabled");
                                callback && callback();
                            }
                            else {
                                handleFalseResponse(response);
                                isCancelConfirm = true;
                                $(this).bootstrapToggle(getToggleBtnStatus(!checked));
                            }
                        })


                }
                else {
                    isCancelConfirm = true;
                    $(this).bootstrapToggle(getToggleBtnStatus(!checked));
                }

            })
        },

        initialize: function () {
            this.resiterQrcodeEvent();
        },
    };

    return _me;
})(jQuery);
var AffiliateLink = (function ($) {

    function handleFalseResponse(response) {
        let data;
        switch (response.status) {
            case 403:
                data = response.json();
                alert(data.ErrMsg);
                location.reload();
                break;
            default:
                alert('Internal error');
                break;
        }
    }


    var _me = {
        initialize: function () {
            _me.initEvent();
            _me.renderLinkChannelStatisticPage();
        },

        initEvent: function () {
            $("#AffLinkType").change(function () {
                let selectedVal = $(this).val();
                let selectedItem = window.searchOption.RefLinks.find(r => r.AffLinkType == selectedVal);

                let domains = selectedItem.Domains.map(item => new Option(item.DomainName, item.DomainId));
                let pages = selectedItem.SitePages.map(item => new Option(item.PageName, item.PageCode));

                $("#Page").empty().append(pages);
                $("#Domain").empty().append(domains);
            })

            $("#btnGenQRCode").click(function () {
                Site.showLoader();
                Promise.all([_me.renderLinkChannelStatisticPage(), _me.renderQrCodePage()])
                    .then(() => {
                        Site.hideLoader();
                        MarketingBase.registerToggleLinkEvent(_me.renderLinkChannelStatisticPage);
                        MarketingBase.resiterQrcodeEvent();
                    })
            });

            MarketingBase.initChannelSelectEvent();
        },

        renderLinkChannelStatisticPage: function () {
             return MarketingBase.renderLinkChannelStatisticPage()
                .then((response) => {
                    if (response.ok)
                        return response.text()
                    else {
                        handleFalseResponse(response);
                    }
                })
                .then(html => {
                    $("#link-statistic").empty().html(html);
                })
        },

        renderQrCodePage: function () {
            let param = {
                selectedLanguage: $('#Language').val(),
                selectedPage: $('#Page').val(),
                selectedChannel: $('#Channel').val(),
                selectedDomainId: $('#Domain').val(),
                selectedLinkType: $('#AffLinkType').val()
            }
            return MarketingBase.renderQrCodePage(param)
                .then((response) => {
                    if (response.ok)
                        return response.text()
                    else {
                        handleFalseResponse(response);
                    }
                })
                .then(html => {
                    $("#qr-code").empty().html(html);
                });
        },
    };

    return _me;
})(jQuery);

AffiliateLink.initialize();