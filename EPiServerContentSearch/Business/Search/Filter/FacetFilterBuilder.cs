using System.Globalization;
using EPiServer.Find.Api.Facets;
using EPiServer.Find.Cms;

namespace EPiServerContentSearch.Business.Search.Filter
{
    public class FacetFilterBuilder
    {
        public FacetFilter Create(DateHistogramFacet.IntervalCount dateFacet)
        {
            var item = new FacetFilter();
            item.Key = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateFacet.Key.Month);
            item.Count = dateFacet.Count;
            item.Value = dateFacet.Key.Month.ToString();

            return item;
        }

        public FacetFilter Create(CategoryCount categoryFacet)
        {
            var item = new FacetFilter();
            item.Key = categoryFacet.Category.Name;
            item.Count = categoryFacet.Count;
            item.Value = categoryFacet.Category.ID.ToString();

            return item;
        }
    }
}