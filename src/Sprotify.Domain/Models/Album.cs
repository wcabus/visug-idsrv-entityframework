using System;
using System.Collections.Generic;

namespace Sprotify.Domain.Models
{
    public class Album
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Art { get; set; }
        
        public Guid BandId { get; set; }
        public virtual Band Band { get; set; }

        public virtual ICollection<AlbumSong> Songs { get; set; }
    }
}