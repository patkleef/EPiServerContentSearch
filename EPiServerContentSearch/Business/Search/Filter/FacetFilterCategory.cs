using System.Collections.Generic;
using EPiServer.Find;
using EPiServerContentSearch.Business.Content.Store;
using EPiServerContentSearch.Models.Pages;

namespace EPiServerContentSearch.Business.Search.Filter
{
    public abstract class FacetFilterCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<FacetFilter> Facets { get; set; }
        public string SelectedValue { get; set; }

        public abstract ITypeSearch<NewsItemPage> SetFilter(ITypeSearch<NewsItemPage> search, string selectedValue);
        public abstract ITypeSearch<NewsItemPage> SetFilterFacet(ITypeSearch<NewsItemPage> search);
    }
}