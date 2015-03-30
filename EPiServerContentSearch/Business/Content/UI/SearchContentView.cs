using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Shell;

namespace EPiServerContentSearch.Business.Content.UI
{
    [ServiceConfiguration(typeof(ViewConfiguration))]
    public class SearchContentView : ViewConfiguration<IContentData>
    {
        public SearchContentView()
        {
            Key = "searchContent";
            Name = "Search content view";
            Description = "Search content view";
            ControllerType = "app/editors/searchcontentview";
            HideFromViewMenu = true;
        }
    }
}