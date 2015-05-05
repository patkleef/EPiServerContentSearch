using System.Collections.Generic;
using System.Linq;
using EPiServer;
using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServer.Shell.Services.Rest;
using EPiServer.Web.Routing;
using EPiServerContentSearch.Business.Search;
using EPiServerContentSearch.Business.Search.Filter;
using EPiServerContentSearch.Models.Pages;
using Newtonsoft.Json;

namespace EPiServerContentSearch.Business.Content.Store
{
    [RestStore("searchpages")]
    public class SearchPagesStore : RestControllerBase
    {
        private readonly IClient _client; 
        private readonly IContentRepository _contentRepository;
        private readonly UrlResolver _urlResolver;
        private readonly FacetFilterBuilder _facetFilterBuilder;
        private readonly SearchResultItemBuilder _searchResultItemBuilder;

        /// <summary>
        /// Public constructor
        /// </summary>
        /// <param name="contentRepository"></param>
        public SearchPagesStore(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
            _urlResolver = UrlResolver.Current;
            _client = Client.CreateFromConfig();
            _facetFilterBuilder = new FacetFilterBuilder();
            _searchResultItemBuilder = new SearchResultItemBuilder();
        }

        /// <summary>
        /// Get news filter categories
        /// </summary>
        /// <param name="selectedFilters"></param>
        /// <returns></returns>
        private IEnumerable<FacetFilterCategory> GetNewsFilterCategories(IEnumerable<SelectedFilter> selectedFilters)
        {
            SelectedFilter dateSelectedFilter = null;
            SelectedFilter categorySelectedFilter = null;
            if (selectedFilters != null)
            {
                dateSelectedFilter = selectedFilters.FirstOrDefault(s => s.Id == 1);
                categorySelectedFilter = selectedFilters.FirstOrDefault(s => s.Id == 2);
            }

            var dateFilter = new DateNewsFacetFilterCategory();
            dateFilter.Name = "Filter on month";
            dateFilter.Facets = null;
            dateFilter.Id = 1;
            dateFilter.SelectedValue = (dateSelectedFilter != null ? dateSelectedFilter.Value : string.Empty);

            var categoryFilter = new CategoryNewsFacetFilterCategory();
            categoryFilter.Name = "Filter on category";
            categoryFilter.Facets = null;
            categoryFilter.Id = 2;
            categoryFilter.SelectedValue = (categorySelectedFilter != null ? categorySelectedFilter.Value : string.Empty);

            var list = new List<FacetFilterCategory> {dateFilter, categoryFilter};
            return list;
        }

        /// <summary>
        /// Get search result
        /// </summary>
        /// <param name="result"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        private SearchResult GetSearchResult(IContentResult<NewsItemPage> result, IEnumerable<FacetFilterCategory> filters)
        {
            var searchResult = new SearchResult();

            var dateFilterItems = new List<FacetFilter>();
            dateFilterItems.Add(new FacetFilter { Key = "All", Value = "-1" });

            foreach (var dateFacet in result.HistogramFacetFor(x => x.NewsDate).Entries)
            {
                dateFilterItems.Add(_facetFilterBuilder.Create(dateFacet));
            }
            filters.First(f => f.Id == 1).Facets = dateFilterItems;

            var categoryFilterItems = new List<FacetFilter>();
            categoryFilterItems.Add(new FacetFilter { Key = "All", Value = "-1" });

            foreach (var categoryFacet in result.CategoriesFacet())
            {
                categoryFilterItems.Add(_facetFilterBuilder.Create(categoryFacet));
            }
            filters.First(f => f.Id == 2).Facets = categoryFilterItems;

            searchResult.Facets = filters;
            searchResult.Items = result.Select(_searchResultItemBuilder.Create);

            return searchResult;
        }

        /// <summary>
        /// Do search
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <param name="selectedFilters"></param>
        /// <returns></returns>
        public RestResult Get(int id, string value, string selectedFilters)
        {
            var sFilters = Enumerable.Empty<SelectedFilter>();
            if (!string.IsNullOrEmpty(selectedFilters))
            {
                sFilters = JsonConvert.DeserializeObject<IEnumerable<SelectedFilter>>(selectedFilters);
            }
            
            var searchCriteria = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                searchCriteria = value;
            }
            var filters = GetNewsFilterCategories(sFilters).ToList();

            var search = _client.Search<NewsItemPage>()
                .Filter(p => p.Name.AnyWordBeginsWith(searchCriteria))
                .Filter(p => p.Ancestors().Match(id.ToString()));

            foreach (var filter in filters)
            {
                search = filter.SetFilter(search, filter.SelectedValue);
                search = filter.SetFilterFacet(search);
            }

            var result = search.Take(1000).GetContentResult();

            var searchResult = GetSearchResult(result, filters);

            return Rest(searchResult);
        }
    }
}