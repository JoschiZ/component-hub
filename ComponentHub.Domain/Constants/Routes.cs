namespace ComponentHub.Domain.Constants;

public static class Routes
{
    public static class General
    {
        public const string Home = "/";
        public const string NotFound = "/NotFound";
    }
    
    
    public static class Users
    {
        public const string PrivateProfile = "/profile";
    }
    
    public static class Components
    {
        public const string Upload = "/CreateNewComponent";
        public const string ComponentPage = "/{userName}/components/{componentName}";

        public static string GetComponentPage(string userName, string componentName)
        {
            return ComponentPage
                .Replace("{userName}", userName)
                .Replace("{componentName}", componentName);
        }
    }
}