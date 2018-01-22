using System.ComponentModel.DataAnnotations;

namespace Sprotify.WebApi.Models.Subscriptions
{
    public class SubscriptionToCreate
    {
        [Required, StringLength(50)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required, Range(0, 100)]
        public decimal PricePerMonth { get; set; }

        public bool HasAdvertisements { get; set; }
        public bool CanOnlyShuffle { get; set; }
        public bool CanPlayOffline { get; set; }
        public bool HasHighQualityStreams { get; set; }
    }
}