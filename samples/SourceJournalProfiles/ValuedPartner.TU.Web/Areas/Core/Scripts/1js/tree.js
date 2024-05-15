(function() {
    'use strict';

    let defaultTree = {
        treeId: undefined,
        tree: undefined,
        preventBinding: false,
        checkedIds: {},
        selectedRowIndex: -1,
        needsCustomProcessing: false,
        businessObject: undefined,
        self: this,

        load: function () {          

            this.tree = $("#" + this.treeId).kendoTreeView({
                read: {
                    cache: false
                },
                dataSource:
                {
                    data: this.loadData()
                },
                dataBinding: function(e) {
                    if (this.preventBinding) {
                        e.preventDefault();
                    }
                    this.preventBinding = false;
                },
                dragAndDrop: false,
                checkboxes: false,
                loadOnDemand: false,
                dataSpriteCssClassField: "sprite"
                //select: this.selectNode //function (e) { alert(e.node.textContent); }
                /*checkboxes: {//left as example
                    checkChildren: false
                },*/
                //expand: this.OnExpand //left as example

            }).data("kendoTreeView");

            return this;
        },

        //left as example
        //this collapses expanded nodes before expanding another;
        //Note: kendo doesn't seem to have this very stable
        /*
        OnExpand: function (e) {
            if (this.collapsingOthers) {
                this.collapsingOthers = false;
            } else {
                this.collapse('.k-item');
                this.collapsingOthers = true;
                this.expand(e.node);
            }
        },
        */

        expand: function (text, parentText = "") {
            //let item = this.tree.findByText(text); Not using the default kendo func as it does exact text match and not contains
            let item;
            let nodes = this.tree.items();
            if (!apputils.isUndefined(nodes) && nodes.length > 0 && !apputils.isUndefined(text) && !apputils.isEmpty(text)) {
                for (let i = 0; i < nodes.length; i++) {
                    let matchPattern = new RegExp('\\b' + text + '\\b');
                    if (matchPattern.test(nodes[i].innerText)) {
                        if (!apputils.isEmpty(parentText) && !apputils.isUndefined(this.tree.parent(nodes[i])[0])) {
                            let parentPattern = new RegExp('\\b' + parentText + '\\b');
                            if (parentPattern.test(this.tree.parent(nodes[i])[0].innerText)) {
                                item = nodes[i];
                                break;
                            }
                        }
                        else {
                            item = nodes[i];
                            break;
                        }
                    }
                }
            }

            if (!apputils.isUndefined(item)) {
                this.tree.select(item);
                let parent = this.tree.parent(item);
                while (parent && parent.length > 0) {
                    this.tree.expand(parent[0]);
                    parent = this.tree.parent(parent[0]);
                }
            }
        },

        /*left as example
        collapseNodes: function () {
            this.tree.items().each((index) => {
                let item = this.tree.findByText(this.tree.items()[index].innerText);
                
                if (item.length > 0) {
                    //this.tree.select(item);
                    let parent = this.tree.parent(item);
                    while (parent && parent.length > 0) {
                        this.tree.collapse(parent[0]);
                        parent = this.tree.parent(parent[0]);
                    }
                }
                
            });
        },
        */

        loadData: function() {
            trace.log("loadData");
        },

        updateTreeData: function(data, resetNode) {
            
            if (apputils.isUndefined(this.tree)){
                return;
            }

            this.tree.dataSource.options.data = data;
            this.tree.dataSource.transport.data = data;
            this.tree.dataSource.read();

            if (resetNode) {
                this.resetNode();
            } else {
                this.tree.expand(".k-item");
                //treeView.expand("> .k-group > .k-item");
            }
        },
		
        resetNode: function () {
            this.tree.remove(".k-item");
            this.tree.expand("li:first");
        },

        resetTreeData: function() {
            this.resetNode();
            this.selectedRowIndex = -1;
        },

        selectRow: function() {
            let dataItem = this.tree.dataItem(this.tree.select());

            if (dataItem && dataItem.RowIndex > -1){
                this.selectedObj = dataItem;
            }
            else {
                throw "RowIndex not defined";
            }
        },

        initLoad: function() {
            this.load();
            this.initEvents();
        },

        initLoad2: function(treeViewId, businessObj) {
            this.initLoad();
            this.treeViewid = treeViewId;
            this.businessObject = businessObj;
        },

        dblclickOnSelected: function (event) {
            //console.log(event.currentTarget.innerText);
        },

        singleClickOnSelected: function (event) {
            console.log(event);
        },

        initEvents: function () {
            let self = this;

            $("#" + this.treeId).on('dblclick', '.k-state-selected', function (event) {
                self.selectRow();
                self.dblclickOnSelected(event);
            });
        }
    };

    this.baseTree = helpers.View.extend(defaultTree);

}).call(this);