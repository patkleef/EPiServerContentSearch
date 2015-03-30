using EPiServer.Shell;

namespace EPiServerContentSearch.Business.Content.UI
{
    [UIDescriptorRegistration]
    public class ContentSearchItemUIDescriptor : UIDescriptor<ContentSearchItem>
    {
        public ContentSearchItemUIDescriptor() 
        {
            DefaultView = "searchContent"; 
            AddDisabledView(CmsViewNames.OnPageEditView);
            AddDisabledView(CmsViewNames.AllPropertiesView);
        }
    }
}