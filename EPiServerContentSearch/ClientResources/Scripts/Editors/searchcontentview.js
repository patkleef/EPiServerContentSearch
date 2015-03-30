define([
     "dijit",
     "dojo",
     "dojo/_base/declare",
     "dojo/when",
     "dojo/dom-construct",
     "dojo/_base/array",
     "dojo/query",
     "dojo/text!./templates/searchcontenttemplate.html",

     "dijit/_Widget",
     "dijit/_TemplatedMixin",
     "dijit/_WidgetsInTemplateMixin",

     "epi/dependency",
     "epi/routes"
], function (
    dijit,

    dojo,
    declare,
    when,
    domConstruct,
    array,
    query,
    template,

    _Widget,
    _TemplatedMixin,
    _WidgetsInTemplateMixin,

    dependency,
    routes
) {
    return declare("app.editors.searchcontentview", [
        _Widget, _TemplatedMixin, _WidgetsInTemplateMixin
    ], {
        _tableNode: null,
        templateString: template,
        intermediateChanges: false,
        store: null,
        contentStore: null,
        currentContentId: 1,

        postCreate: function () {
            this.tableNode = domConstruct.create("table", {
                className: "pagesTable",
                id: "pagesTable"
            });

            var colgroup = domConstruct.create("colgroup");
            colgroup.appendChild(domConstruct.create("col", {
                width: "400"
            }));
            colgroup.appendChild(domConstruct.create("col", {
                width: "150"
            }));
            colgroup.appendChild(domConstruct.create("col", {
                width: "150"
            }));
            this.tableNode.appendChild(colgroup);

            var tableHead = domConstruct.create("thead");
            var tableBody = domConstruct.create("tbody");
            var headerRow = domConstruct.create("tr");
            headerRow.appendChild(domConstruct.create("th", { innerHTML: "Page" }));
            headerRow.appendChild(domConstruct.create("th", { innerHTML: "Created by" }));
            headerRow.appendChild(domConstruct.create("th", { innerHTML: "Created" }));

            tableHead.appendChild(headerRow);
            this.tableNode.appendChild(tableHead);
            this.tableNode.appendChild(tableBody);
            this.resultList.appendChild(this.tableNode);

            this.inputWidget.set("intermediateChanges", true);

            var registry = dependency.resolve("epi.storeregistry");
            registry.create("app.searchpages", routes.getRestPath({ moduleArea: "app", storeName: "searchpages" }));
            this.store = registry.get("app.searchpages");

            this.contentStore = registry.get("epi.cms.content.light");

            this.connect(this.inputWidget, "onChange", this._onInputWidgetChanged);
            this.connect(this.showAllLink, "onclick", this._onShowAllLinkClicked);

            var contextService = epi.dependency.resolve("epi.shell.ContextService");
            var currentContext = contextService.currentContext;
            var res = currentContext.id.split("_");

            this.currentContentId = res[0];
        },

        _clearTableBody: function() {
            var tableBody = query("tbody", this.tableNode)[0];

            domConstruct.empty(tableBody);
        },

        _onShowAllLinkClicked: function (event) {
            event.preventDefault();
            this._clearTableBody();
            
            dojo.when(this.store.query({id: this.currentContentId, value: "" }), this._buildRows);
        },

        // Event handler for the changed event of the input widget         
        _onInputWidgetChanged: function (value) {
            this._clearTableBody();

            dojo.when(this.store.query({ id: this.currentContentId, value: value }), this._buildRows);
        },

        _buildRows: function (result) {
            var tableNode = dojo.byId("pagesTable");
            var tableBody = query("tbody", tableNode)[0];

            array.forEach(result, function (item) {
                var newRow = domConstruct.create("tr");
                var pageColumn = domConstruct.create("td", {
                    innerHTML: "<a class='page-item-link' href='" + item.url + "'>" + item.name + "</a>"
                });
                var createdByColumn = domConstruct.create("td", {
                    innerHTML: item.createdBy
                });
                var createdAtColumn = domConstruct.create("td", {
                    innerHTML: item.created
                });

                newRow.appendChild(pageColumn);
                newRow.appendChild(createdByColumn);
                newRow.appendChild(createdAtColumn);

                tableBody.appendChild(newRow);
            }, this);
        }
    });
});