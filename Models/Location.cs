namespace salian_api.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Abbreviation { get; set; }
        public bool IsShow { get; set; }
        public Employee Employee { get; set; }
    }
}
