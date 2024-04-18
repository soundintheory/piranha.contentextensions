using Piranha.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundInTheory.Piranha.ContentExtensions.Singletons.Models
{
    public class SingletonType
    {
        public BaseContentType Type { get; set; }

        public string Id { get; set; }

        public string Title { get; set; }

        public string Icon { get; set; }

        public string Slug { get; set; }

        public bool ShowInMenu { get; set; }

        public string CLRType { get; set; }

        public string Route => $"~/manager/instance/{Type.ToString().ToLower()}/{Id}";

        public string IdCacheKey => SingletonsModule.CacheKey($"{Id}.Id");

        public SingletonType()
        {
        }

        public SingletonType(ContentTypeBase contentType)
        {
            Id = contentType.Id;
            Title = contentType.Title;
            CLRType = contentType.CLRType;
            Type = contentType switch
            {
                ContentType => BaseContentType.Content,
                _ => BaseContentType.Page
            };
        }
    }

    public enum BaseContentType
    {
        Page,
        Content,
        Post
    }
}
