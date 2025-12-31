namespace salian_api.Models
{
    public class Employee
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int LocationID { get; set; }
        public Location Location { get; set; } // one to one relation
    }
}
