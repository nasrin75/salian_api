using salian_api.Entities;
using System.ComponentModel.DataAnnotations;

namespace salian_api.Dtos.Location
{
    public class LocationUpdateDto
    {
        [Required]
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Abbreviation { get; set; }
        public bool? IsShow { get; set; }
    }
}
