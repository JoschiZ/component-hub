namespace ComponentHub.Domain.Api;

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
        public const string Create = ComponentsBasePath + "create/";
        public const string Update = ComponentsBasePath + "update/";
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