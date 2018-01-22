
namespace Sprotify.WebApi.Models.Bands
{
    public class BandMapperProfiles : AutoMapper.Profile
    {
        public BandMapperProfiles()
        {
            CreateMap<Domain.Models.Band, Band>();
            CreateMap<BandToUpdate, Domain.Models.Band>();
        }
    }
}
