using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer;
using EPiServer.Cms.Shell.UI.Rest.ContentQuery;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ContentQuery;

namespace EPiServerContentSearch.Business.Content.Tree
{
    [ServiceConfiguration(typeof(IContentQuery))]
    public class TreeChildrenQuery : GetChildrenQuery
    {
        private readonly IContentRepository _contentRepository;

        public TreeChildrenQuery(IContentQueryHelper queryHelper, IContentRepository contentRepository, LanguageSelectorFactory languageSelectorFactory) 
            : base(queryHelper, contentRepository, languageSelectorFactory)
        {
            _contentRepository = contentRepository;
        }

        protected override IEnumerable<IContent> GetContent(ContentQueryParameters parameters)
        {
            IContent contentData = null;
            if (_contentRepository.TryGet(parameters.ReferenceId, out contentData))
            {
                if (contentData is ContentSearchItem)
                {
                    return Enumerable.Empty<IContent>();
                }
                if (contentData is ISearchableContent && parameters.AllParameters["query"].Equals("getchildren", StringComparison.InvariantCultureIgnoreCase))
                {
                    return new[] { new ContentSearchItem(contentData.ContentLink.ID) };
                }
            }
            return base.GetContent(parameters);
        }

        protected override IEnumerable<IContent> Filter(IEnumerable<IContent> items, ContentQueryParameters parameters)
        {
            var enumerable = items as IList<IContent> ?? items.ToList();
            if (enumerable.Any(c => c is ContentSearchItem))
            {
                return enumerable;
            }
            return base.Filter(enumerable, parameters);
        }

        public override int Rank { get { return 100;  } }
    }
}