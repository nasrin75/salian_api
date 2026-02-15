namespace salian_api.Dtos.User
{
    public class AssignUserPermissionDto
    {
        public long UserId { get; set; }
        public List<long> PermissionIds { get; set; }
    }
}
