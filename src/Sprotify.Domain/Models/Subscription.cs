using System;

namespace Sprotify.Domain.Models
{
    public class Subscription
    {
        public Subscription() { }

        public Subscription(string title, string description, decimal pricePerMonth, bool hasAdvertisements, bool canOnlyShuffle, bool canPlayOffline, bool hasHighQualityStreams)
        {
            Title = title;
            Description = description;
            PricePerMonth = pricePerMonth;
            HasAdvertisements = hasAdvertisements;
            CanOnlyShuffle = canOnlyShuffle;
            CanPlayOffline = canPlayOffline;
            HasHighQualityStreams = hasHighQualityStreams;
        }

        public Guid Id { get; set; }
        public decimal PricePerMonth { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public bool HasAdvertisements { get; set; }
        public bool CanOnlyShuffle { get; set; }
        public bool CanPlayOffline { get; set; }
        public bool HasHighQualityStreams { get; set; }
    }
}