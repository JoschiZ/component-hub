namespace ComponentHub.Domain.Constants;

public static class Routes
{
    public static class Users
    {
        public const string PrivateProfile = "/profile";
    }
    
    
    public static class Components
    {
        public const string Upload = "/CreateNewComponent";
        public const string ComponentPage = "/{userName}/components/{componentName}";
    }
}