using System.ComponentModel.DataAnnotations;

namespace salian_api.Models
{
    public class Role
    {
        [Key]
        public long Id { get; set; }
        public string FaName { get; set; }
        public string EnName { get; set; }
    }
}
