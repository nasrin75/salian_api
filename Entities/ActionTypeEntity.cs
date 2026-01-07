using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace salian_api.Entities
{
    [Table("ActionTypes")]
    public class ActionTypeEntity
    {
        [Key]
        public long Id { get; set; }
        public string FaName { get; set; }
        public string EnName { get; set; }
        public bool IsShow { get; set; }
        public DateTime? DeletedAt { get; set; }

    }
}
