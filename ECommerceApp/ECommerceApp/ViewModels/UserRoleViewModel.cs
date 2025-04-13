namespace ECommerceApp.ViewModels
{
    public class UserRoleViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<string> UnassignedRoles { get; set; }
        public List<string> AssignedRoles { get; set; }
    }
}
