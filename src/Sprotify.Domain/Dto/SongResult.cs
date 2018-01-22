using Sprotify.Domain.Models;

namespace Sprotify.Domain.Dto
{
    public class SongResult
    {
        public Song Song { get; set; }
        public Album Album { get; set; }
        public Band Band { get; set; }

        public int Position { get; set; }
    }
}
