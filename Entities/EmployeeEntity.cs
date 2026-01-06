using System.ComponentModel.DataAnnotations.Schema;

namespace salian_api.Entities
{
    [Table("Employees")]
    public class EmployeeEntity
    { 
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public long LocationID { get; set; }
        public LocationEntity Location { get; set; } // one to one relation
    }
}
