// The MIT License (MIT) 
// Copyright (c) 1994-2018 The Sage Group plc or its licensors.  All rights reserved.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of 
// this software and associated documentation files (the "Software"), to deal in 
// the Software without restriction, including without limitation the rights to use, 
// copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the 
// Software, and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

(function ($, window, document, undefined) {
    $.widget("sageuiwidgets.RecordNavigation", {
        maxItemsinCache: 5,
        items: [],
        itemIndex: -1,
        options: {
            previousUrl: '',
            nextUrl: '',
            First: null,
            currentPos: 0,
            totalRecords: 100,
            DataRetrieveFailure: $.noop,
            DataRetrieveSucceful: $.noop,
            GetNext: $.noop,
            GetPrevious: $.noop
        },
        _create: function () {
            this._addRecordNavigationUI(this);
        },
        _addRecordNavigationUI: function () {
            var that = this;
            var rcontainer = $('<ul class="recnav" />');
            var leftArrow = $('<li><a href="javascript:void(0)" class="recNavLeftArrow"></a></li>');
            var rightArrow = $('<li><a href="javascript:void(0)" class="recNavRightArrow"></a></li>');
            //var navcontent = $('<li><label class="crecord" /><label class="recnumseperator" />/</label><label class="totalrec" /></li>');

            leftArrow.appendTo(rcontainer);
            //navcontent.appendTo(rcontainer);
            rightArrow.appendTo(rcontainer);
            rcontainer.appendTo(that.element);
            $('.crecord', rcontainer).text(that.options.currentPos);
            $('.totalrec', rcontainer).text(that.options.totalRecords);

            if (that.options.first != null) {
                that.items.push(that.options.first);
                that.itemIndex = 0;
            }

            $('.recNavLeftArrow', that.element).bind('click', function (e) {
                that._navigatePrevious(that, e);
            });

            $('.recNavRightArrow', that.element).bind('click', function (e) {
                that._navigateNext(that, e);
            });
        },

        _canNavigateNext: function (that) {
            return (that.options.currentPos < that.options.totalRecords);
        },

        _canNavigatePrevious: function (that) {
            return (that.options.currentPos >= 1);
        },

        _navigatePrevious: function (that, e) {
            if (that._canNavigatePrevious(that)) {
                that._naviagateToPreviousItem(that);
            }
        },

        _navigateNext: function (that, e) {
            if (that._canNavigateNext(that)) {
                that.__naviagateToNextItem(that);
            }
        },

        _naviagateToPreviousItem: function (that) {
            var previousItem = that._getPreviousItem(that);

            if (previousItem != null) {
                that._trigger("DataRetrieveSucceful");
                that.options.currentPos = that.options.currentPos - 1;
                //$('.crecord', that.element).text(that.options.currentPos);
                //$('.crecord', that.element).text(previousItem);
            } else {
                that.options.currentPos = that.options.currentPos - 1;
                //$(".crecord").text(that.options.currentPos);
                that._trigger("DataRetrieveFailure");
            }
        },
        __naviagateToNextItem: function (that) {
            var nextItem = that._getNextItem(that);

            if (nextItem != null) {
                that._trigger("DataRetrieveSucceful", null, nextItem);
                that.options.currentPos = that.options.currentPos + 1;
                //$('.crecord', that.element).text(that.options.currentPos);
                $('.crecord', that.element).text(nextItem);
            } else {
                that.options.currentPos = that.options.currentPos + 1;
                $(".crecord").text(that.options.currentPos);
                that._trigger("DataRetrieveFailure");
            }
        },

        _getPreviousItem: function (that) {
            var previousItem = that._getPreviousItemFromCache(that);

            if (previousItem == null) {
                previousItem = that._getItemFromServer(that, false);
                if (previousItem != null) {
                    that._insertPreviousItemToCache(that, previousItem);
                }
            }
            return previousItem;
        },
        _getNextItem: function (that) {
            var nextItem = that._getNextItemFromCache(that);
            if (nextItem == null) {

                nextItem = that._getItemFromServer(that, true);
                if (nextItem != null) {
                    that._insertNextItemToCache(that, nextItem);
                }
            }
            return nextItem;
        },
        _getPreviousItemFromCache: function (that) {
            return that._getItemFromCache(that, that.itemIndex - 1);
        },
        _getNextItemFromCache: function (that) {
            return that._getItemFromCache(that, that.itemIndex + 1);
        },
        _getItemFromCache: function (that, itemNumberToFetch) {
            var item = null;
            if (that.itemIndex < itemNumberToFetch) {
                // Check if Item is in  cache,  if the current index is at the last item in array then the item has to be fetched from server
                if (itemNumberToFetch < that.maxItemsinCache && itemNumberToFetch < that.items.length) {
                    item = that.items[itemNumberToFetch];
                    that.itemIndex = itemNumberToFetch;
                }
            } else if (that.itemIndex > itemNumberToFetch) {
                if (itemNumberToFetch >= 0) {
                    item = that.items[itemNumberToFetch];
                    that.itemIndex = itemNumberToFetch;
                }
            } else if (that.itemIndex == itemNumberToFetch) {
                item = that.current;
            }
            return item;
        },
        _getItemFromServer: function (that, next) {
            var jsonItem = null;
            if (next) {
                jsonItem = that.options.GetNext();
            } else {
                jsonItem = that.options.GetPrevious();
            }
            return jsonItem;
        },
        _insertNextItemToCache: function (that, nextItem) {
            var localItems = [];
            var skipFirst = (that.itemIndex + 1 == that.maxItemsinCache);
            $.each(that.items, function (index, value) {
                if (index <= that.itemIndex) {
                    if (skipFirst == false) {
                        localItems.push(value);
                    } else {
                        skipFirst = false;
                    }

                } else if (index > that.itemIndex) {
                    return false;
                }
            });
            localItems.push(nextItem);
            that.items = localItems;
            that.itemIndex = localItems.length - 1;
        },
        _insertPreviousItemToCache: function (that, previousItem) {
            var localItems = [];
            var skipLast = (that.items.length == that.maxItemsinCache);
            localItems.push(previousItem);

            $.each(that.items, function (index, value) {
                if (index + 1 < that.items.length) {
                    localItems.push(that.items[index + 1]);
                }
            });
            that.items = localItems;
            that.itemIndex = 0;
        },

        destroy: function () {
            $('ul .recnav', this.element).remove();
            $.Widget.prototype.destroy.call(this);
        }
    });
})(jQuery, window, document);