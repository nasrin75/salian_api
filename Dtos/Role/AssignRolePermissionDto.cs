namespace salian_api.Dtos.Role
{
    public class AssignRolePermissionDto
    {
        public long RoleId { get; set; }
        public List<long> PermissionIds { get; set; }
    }
}
