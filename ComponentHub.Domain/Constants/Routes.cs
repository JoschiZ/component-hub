using System.Diagnostics.Contracts;

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
        public const string PublicProfile = "users/{UserName}";

        [Pure]
        public static string GetPublicProfile(string userName)
        {
            return PublicProfile.Replace("{UserName}", userName);
        }
    }
    
    public static class Components
    {
        public const string Upload = "/CreateNewComponent";
        
        public const string ComponentPage = "/{userName}/{componentName}";
        
        [Pure]
        public static string GetComponentPage(string userName, string componentName)
        {
            return ComponentPage
                .Replace("{userName}", userName)
                .Replace("{componentName}", componentName);
        }
    }
}