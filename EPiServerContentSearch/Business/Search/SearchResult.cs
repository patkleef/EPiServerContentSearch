using System.Collections.Generic;
using EPiServerContentSearch.Business.Search.Filter;

namespace EPiServerContentSearch.Business.Search
{
    public class SearchResult
    {
        public IEnumerable<FacetFilterCategory> Facets { get; set; }
        public IEnumerable<SearchResultItem> Items { get; set; }
    }
}