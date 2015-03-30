using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServerContentSearch.Business.Content;

namespace EPiServerContentSearch.Models.Pages
{
    [ContentType(DisplayName = "News overview", GUID = "6c266c22-145d-4513-b435-99256a36e885", Description = "")]
    public class NewsOverviewPage : PageData, ISearchableContent
    {

    }
}