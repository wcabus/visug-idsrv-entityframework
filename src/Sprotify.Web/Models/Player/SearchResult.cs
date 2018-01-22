using System.Collections.Generic;

namespace Sprotify.Web.Models.Player
{
    public class SearchResult
    {
        public SearchResult()
        {

        }

        public SearchResult(string filter, IEnumerable<SearchItem> bands, IEnumerable<SearchItem> albums, IEnumerable<SearchItem> songs)
        {
            Filter = filter;
            Bands = bands;
            Albums = albums;
            Songs = songs;
        }

        public string Filter { get; set; }
        public IEnumerable<SearchItem> Bands { get; private set; }
        public IEnumerable<SearchItem> Albums { get; private set; }
        public IEnumerable<SearchItem> Songs { get; private set; }
    }
}
