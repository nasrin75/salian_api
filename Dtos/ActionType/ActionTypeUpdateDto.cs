using salian_api.Entities;
using System.ComponentModel.DataAnnotations;

namespace salian_api.Dtos.ActionType
{
    public class ActionTypeUpdateDto
    {
        [Key]
        public required long Id { get; set; }
        public string? FaName { get; set; }
        public string? EnName { get; set; }
        public bool? IsShow { get; set; }
    }
}
