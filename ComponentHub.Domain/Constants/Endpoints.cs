namespace ComponentHub.Domain.Constants;

public static class Endpoints
{
    private const string BasePath = "api/";
    public static class Auth
    {
        private const string AuthBasePath = BasePath + "auth/";
        public const string Authenticate = AuthBasePath + "login-battlenet";
        public const string GetUserInfo = AuthBasePath + "getuserinfo";
        public const string ExternalLogin = AuthBasePath + "external-login";
        public const string Logout = AuthBasePath + "logout";
        public const string Register = AuthBasePath + "register";
        public const string ExternalLoginCallback = AuthBasePath + "external-login-callback";
    }
    
    public static class Components
    {
        private const string ComponentsBasePath = BasePath + "components/";
        public const string Create = ComponentsBasePath + "create";
        public const string Update = ComponentsBasePath + "update";
        public const string Delete = ComponentsBasePath + "delete";
        public const string Get = ComponentsBasePath + "get/{UserName}/{ComponentName}";
        public const string GetByUser = ComponentsBasePath + "{UserName}";
        public static string FormatGetByUser(string userName) => 
            GetByUser.Replace("{UserName}", userName);
        
        
        public const string Query = ComponentsBasePath;

        public static string FormatGet(string userName, string componentName)
        {
            return Get.Replace("{UserName}", userName).Replace("{ComponentName}", componentName);
        }
    }

    public static class Users
    {
        private const string UsersBasePath = BasePath + "users/";
        public const string GetDetailedInfo = UsersBasePath + "detailedInfo";
        public const string Delete = UsersBasePath + "delete";
        public const string ChangeUsername = UsersBasePath + "changeName";
        public const string DownloadUserdata = UsersBasePath + "download";
    }
    
    
    /// <summary>
    /// This endpoint group contains various endpoints that are only used for development and testing
    /// </summary>
    public static class Testing
    {
        public const string TestingBasePath = BasePath + "testing/";
        public const string RedirectMe = TestingBasePath + "redirect";
    }
}