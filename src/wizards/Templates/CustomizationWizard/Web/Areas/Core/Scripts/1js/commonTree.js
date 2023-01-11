'use strict';

// have to use 'var'; prevent loading multiple times; 
var commonTree = commonTree;

if (apputils.isUndefined(commonTree)) {
    commonTree = baseTree.extend({
        treeId: undefined,
        selectedRowIndex: -1,
        businessObject: undefined,

        /**Load data for the tree view */
        loadData: function () {
            return this.businessObject.loadDataForTree();
        },

        /** tree view is not editable */
        editable: function () {
            return false;
        }

    });
}


