using EPiServer.Find;
using EPiServer.Find.Api.Facets;
using EPiServerContentSearch.Models.Pages;

namespace EPiServerContentSearch.Business.Search.Filter
{
    public class DateNewsFacetFilterCategory : FacetFilterCategory
    {
        public override ITypeSearch<NewsItemPage> SetFilter(ITypeSearch<NewsItemPage> search, string selectedValue)
        {
            if (string.IsNullOrEmpty(selectedValue) || selectedValue == "-1")
            {
                return search;
            }
            return search.Filter(f => f.NewsDate.MatchMonth(2015, int.Parse(selectedValue)));
        }

        public override ITypeSearch<NewsItemPage> SetFilterFacet(ITypeSearch<NewsItemPage> search)
        {
            return search.HistogramFacetFor(x => x.NewsDate, DateInterval.Month);
        }
    }
}