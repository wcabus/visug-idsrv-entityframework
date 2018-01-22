using System;

namespace Sprotify.WebApi.Models
{
    public class SearchItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }

        public TimeSpan? Duration { get; set; }

        public string Album { get; set; }
        public Guid? AlbumId { get; set; }

        public string Band { get; set; }
        public Guid? BandId { get; set; }
    }
}
