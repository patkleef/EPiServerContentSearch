define([
     "dijit",
     "dojo",
     "dojo/_base/declare",
     "dojo/when",
     "dojo/dom-construct",
     "dojo/_base/array",
     "dojo/_base/lang",
     "dojo/query",
     "dojo/dom-class",
     "dojo/dom-attr",
     "dojo/on",
     "dojo/json",
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
    lang,
    query,
    domClass,
    domAttr,
    on,
    JSON,
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

        postCreate: function() {
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

            this._getResults();
        },

        _onShowAllLinkClicked: function(event) {
            event.preventDefault();
            this.inputWidget.value = "";
            this._getResults();
        },

        // Event handler for the changed event of the input widget         
        _onInputWidgetChanged: function(value) {
            this._getResults();
        },

        _filterClicked: function (e) {
            query("li a", e.currentTarget.parentElement.parentElement).removeClass("active-filter");
            domClass.toggle(e.currentTarget, "active-filter");

            this._getResults();
        },

        _getResults: function() {
            var selectedFilters = [];
            query(".filter-category").forEach(function (filterCat) {
                var filters = query("li a.active-filter", filterCat);
                if (filters.length > 0) {
                    filters.forEach(function (filter) {
                        selectedFilters.push({ Id: filterCat.id, Value: dojo.attr(filter, "data-val") });
                    });
                }
            });
            dojo.when(this.store.query(
            {
                id: this.currentContentId,
                value: this.inputWidget.value,
                selectedFilters: JSON.stringify(selectedFilters)
            }), lang.hitch(this, this._buildResult));
        },

        _buildResult: function (result) {
            this._buildRows(result.items);
            this._buildFilters(result.facets);
        },

        _buildFilters: function (filters) {
            this._clearFilters();
            var ulFilters = domConstruct.create("ul");

            array.forEach(filters, function (item) {
                var filterDiv = domConstruct.create("div", {
                    className: "filter-category",
                    id: item.id
                });
                var filterTitle = domConstruct.create("strong", {
                    innerHTML: item.name
                });

                var ul = domConstruct.create("ul");

                array.forEach(item.facets, function (filterItem) {
                    var li = domConstruct.create("li");
                    var a = domConstruct.create("a", {
                        href: "#",
                        'data-val': filterItem.value
                    });
                    if (filterItem.value == "-1") {
                        a.innerHTML = filterItem.key;
                    } else {
                        a.innerHTML = filterItem.key + " (" + filterItem.count + ")";
                    }
                    if (item.selectedValue == filterItem.value || (filterItem.value == "-1" && item.selectedValue == "")) {
                        a.className = "active-filter";
                    }

                    on(a, "click", lang.hitch(this, this._filterClicked));
                    li.appendChild(a);
                    ul.appendChild(li);
                }, this);

                filterDiv.appendChild(filterTitle);
                filterDiv.appendChild(ul);

                this.facetsContainer.appendChild(filterDiv);
            }, this);
        },

        _buildRows: function (items) {
            this._clearTableBody();

            var tableNode = dojo.byId("pagesTable");
            var tableBody = query("tbody", tableNode)[0];

            array.forEach(items, function (item) {
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
        },

        _clearTableBody: function() {
            var tableBody = query("tbody", this.tableNode)[0];

            domConstruct.empty(tableBody);
        },

        _clearFilters: function () {
            var filterContainer = query(".filter-container")[0];

            domConstruct.empty(filterContainer);
        }
    });
});