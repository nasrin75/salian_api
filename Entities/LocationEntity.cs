using System.ComponentModel.DataAnnotations.Schema;

namespace salian_api.Entities
{
    [Table("Locations")]
    public class LocationEntity
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Abbreviation { get; set; }
        public bool IsShow { get; set; }

        public bool IsDeleted { get; set; }
       // public EmployeeEntity Employee { get; set; }
    }
}
