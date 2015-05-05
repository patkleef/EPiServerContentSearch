using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServerContentSearch.Models.Pages;

namespace EPiServerContentSearch.Business.Search.Filter
{
    public class CategoryNewsFacetFilterCategory : FacetFilterCategory
    {
        public override ITypeSearch<NewsItemPage> SetFilter(ITypeSearch<NewsItemPage> search, string selectedValue)
        {
            if (string.IsNullOrEmpty(selectedValue) || selectedValue == "-1")
            {
                return search;
            }
            return search.Filter(f => f.Category.Match(int.Parse(selectedValue)));
        }

        public override ITypeSearch<NewsItemPage> SetFilterFacet(ITypeSearch<NewsItemPage> search)
        {
            return search.CategoriesFacet();
        }
    }
}