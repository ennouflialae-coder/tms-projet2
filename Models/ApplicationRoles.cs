namespace TMS_Project.Models
{
    public static class ApplicationRoles
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public const string AdminOrUser = Admin + "," + User;
    }
}
