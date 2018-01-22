using System;
using System.Collections.Generic;

namespace Sprotify.Web.Models.Player
{
    public class PlaylistWithSongs
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public Guid CreatorId { get; set; }
        public string CreatorName { get; set; }

        public string Image { get; set; }

        public IEnumerable<Song> Songs { get; set; }
    }
}
