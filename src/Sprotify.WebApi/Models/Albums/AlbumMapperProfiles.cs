namespace Sprotify.WebApi.Models.Albums
{
    public class AlbumMapperProfiles : AutoMapper.Profile
    {
        public AlbumMapperProfiles()
        {
            CreateMap<Domain.Models.Album, Album>()
                .ForMember(x => x.Band, opt => opt.MapFrom(src => src.Band.Name));

            CreateMap<Domain.Models.Album, AlbumWithSongs>();

            CreateMap<Domain.Models.AlbumSong, AlbumSong>()
                .ForMember(x => x.Title, opt => opt.MapFrom(src => src.Song.Title))
                .ForMember(x => x.Duration, opt => opt.MapFrom(src => src.Song.Duration))
                .ForMember(x => x.ReleaseDate, opt => opt.MapFrom(src => src.Song.ReleaseDate))
                .ForMember(x => x.ContainsExplicitLyrics, opt => opt.MapFrom(src => src.Song.ContainsExplicitLyrics));

            CreateMap<AlbumToUpdate, Domain.Models.Album>();

            CreateMap<Domain.Models.Album, SearchItem>()
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Title))
                .ForMember(x => x.Band, opt => opt.MapFrom(src => src.Band.Name))
                .ForMember(x => x.BandId, opt => opt.MapFrom(src => src.BandId))
                .ForMember(x => x.Image, opt => opt.MapFrom(src => src.Art));
        }
    }
}
