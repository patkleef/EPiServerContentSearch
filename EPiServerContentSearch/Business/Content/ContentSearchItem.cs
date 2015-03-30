using System;
using EPiServer.Core;

namespace EPiServerContentSearch.Business.Content
{
    public class ContentSearchItem : IContent
    {
        public ContentSearchItem(int id)
        {
            var contentLink = new ContentReference(id);
            contentLink.ProviderName = "contentSearchableProvider";
            contentLink.WorkID = 1;

            Name = "[Search]";
            ContentLink = contentLink;
            ParentLink = ContentReference.EmptyReference;
            ContentGuid = Guid.Empty;
            ContentTypeID = 3;
            IsDeleted = false;
            
        }
        public PropertyDataCollection Property
        {
            get { return new PropertyDataCollection(); }
        }

        public string Name { get; set; }

        public ContentReference ContentLink { get; set; }

        public ContentReference ParentLink { get; set; }

        public Guid ContentGuid { get; set; }

        public int ContentTypeID { get; set; }

        public bool IsDeleted { get; set; }
    }
}