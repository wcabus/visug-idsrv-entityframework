using System.ComponentModel.DataAnnotations;

namespace Sprotify.WebApi.Models.Bands
{
    public class BandToCreate
    {
        [Required, StringLength(100)]
        public string Name { get; set; }
    }

    public class BandToUpdate : BandToCreate { }
}
