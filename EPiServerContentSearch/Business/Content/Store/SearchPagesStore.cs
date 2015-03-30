using System;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.Shell.Services.Rest;
using EPiServer.Web.Routing;

namespace EPiServerContentSearch.Business.Content.Store
{
    [RestStore("searchpages")]
    public class SearchPagesStore : RestControllerBase
    {
        private readonly IContentRepository _contentRepository;
        private readonly UrlResolver _urlResolver;

        public SearchPagesStore(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
            _urlResolver = UrlResolver.Current;
        }

        public RestResult Get(int id, string value)
        {
            if (id == 0)
            {
                id = ContentReference.StartPage.ID;
            }
            var references = _contentRepository.GetDescendents(new ContentReference(id));

            var pages = _contentRepository.GetItems(references, LanguageSelector.AutoDetect());

            var result = pages.Where(p => ((string.IsNullOrEmpty(value) || p.Name.IndexOf(value, StringComparison.InvariantCultureIgnoreCase) != -1) && p is PageData))
                .OrderByDescending(p => ((PageData)p).Created)
                .Select(p => new
                {
                    Name = p.Name,
                    Url = string.Format("/EPiServer/Cms/#context=epi.cms.contentdata:///{0}", p.ContentLink.ID),
                    Created = ((PageData)p).Created.ToString("dd-MM-yyyy hh:mm"),
                    CreatedBy = ((PageData)p).CreatedBy
                });

            return Rest(result);
        }
    }
}