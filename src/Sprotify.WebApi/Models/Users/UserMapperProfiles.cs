namespace Sprotify.WebApi.Models.Users
{
    public class UserMapperProfiles : AutoMapper.Profile
    {
        public UserMapperProfiles()
        {
            CreateMap<Domain.Models.User, User>();
            CreateMap<Domain.Models.UserSubscription, UserSubscription>();
        }
    }
}