using System.ComponentModel.DataAnnotations;

namespace Sprotify.Web.Areas.Administration.Models
{
    public class EditBand
    {
        [Required, StringLength(100)]
        public string Name { get; set; }
    }
}
