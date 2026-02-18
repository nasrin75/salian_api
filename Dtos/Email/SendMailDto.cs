namespace salian_api.Dtos.Email
{
    public class SendMailDto
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        //public List<IFormFile>? Attachments { get; set; } // Use IFormFile for file uploads in ASP.NET Core
    }
}
