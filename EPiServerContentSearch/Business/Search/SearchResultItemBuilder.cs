using EPiServerContentSearch.Models.Pages;

namespace EPiServerContentSearch.Business.Search
{
    public class SearchResultItemBuilder
    {
        public SearchResultItem Create(NewsItemPage page)
        {
            var item = new SearchResultItem
            {
                Name = page.Name,
                Url = string.Format("/EPiServer/Cms/#context=epi.cms.contentdata:///{0}", page.ContentLink.ID),
                Created = page.Created.ToString("dd-MM-yyyy hh:mm"),
                CreatedBy = page.CreatedBy
            };

            return item;
        }
    }
}