namespace Sprotify.WebApi.Models.Subscriptions
{
    public class SubscriptionMapperProfiles : AutoMapper.Profile
    {
        public SubscriptionMapperProfiles()
        {
            CreateMap<Domain.Models.Subscription, Subscription>();
        }
    }
}