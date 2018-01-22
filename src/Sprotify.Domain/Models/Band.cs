using System;
using System.Collections.Generic;

namespace Sprotify.Domain.Models
{
    public class Band
    {
        protected internal Band()
        {

        }

        public Band(string name)
        {
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Album> Albums { get; set; } = new List<Album>();
        public virtual ICollection<Song> Songs { get; set; } = new List<Song>();

        public Album AddAlbum(string title, DateTime? releaseDate, string art)
        {
            var album = new Album
            {
                Title = title,
                ReleaseDate = releaseDate,
                Art = art
            };

            Albums.Add(album);
            return album;
        }
    }
}