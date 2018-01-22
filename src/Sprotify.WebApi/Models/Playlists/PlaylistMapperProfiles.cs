
using Sprotify.WebApi.Models.Songs;

namespace Sprotify.WebApi.Models.Playlists
{
    public class PlaylistMapperProfiles : AutoMapper.Profile
    {
        public PlaylistMapperProfiles()
        {
            CreateMap<Domain.Models.Playlist, Playlist>();
            CreateMap<Domain.Models.Playlist, PlaylistWithSongs>();

            CreateMap<Domain.Models.PlaylistSong, Song>()
                .ForMember(x => x.Title, opt => opt.MapFrom(src => src.Song.Title))
                .ForMember(x => x.Duration, opt => opt.MapFrom(src => src.Song.Duration))
                .ForMember(x => x.ReleaseDate, opt => opt.MapFrom(src => src.Song.ReleaseDate))
                .ForMember(x => x.ContainsExplicitLyrics, opt => opt.MapFrom(src => src.Song.ContainsExplicitLyrics))
                .ForMember(x => x.Band, opt => opt.MapFrom(src => src.Song.Band.Name))
                .ForMember(x => x.BandId, opt => opt.MapFrom(src => src.Song.BandId));
        }
    }
}
