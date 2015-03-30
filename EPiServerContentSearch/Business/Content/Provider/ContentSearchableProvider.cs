using System.Collections.Generic;
using EPiServer;
using EPiServer.Core;

namespace EPiServerContentSearch.Business.Content.Provider
{
    public class ContentSearchableProvider : DefaultContentProvider
    {
        protected override IContent LoadContent(ContentReference contentLink, ILanguageSelector languageSelector)
        {
            return new ContentSearchItem(contentLink.ID);
        }

        protected override IList<GetChildrenReferenceResult> LoadChildrenReferencesAndTypes(ContentReference contentLink, string languageID, out bool languageSpecific)
        {
            languageSpecific = true;
            return new List<GetChildrenReferenceResult>();
        }
    }
}