using System;

namespace Sprotify.WebApi.Models.Subscriptions
{
    public class Subscription
    {
        public Guid Id { get; set; }
        
        public string Title { get; set; }
        public string Description { get; set; }

        public decimal PricePerMonth { get; set; }

        public bool HasAdvertisements { get; set; }
        public bool CanOnlyShuffle { get; set; }
        public bool CanPlayOffline { get; set; }
        public bool HasHighQualityStreams { get; set; }
    }
}