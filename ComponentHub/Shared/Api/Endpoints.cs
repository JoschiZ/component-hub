namespace ComponentHub.Shared.Api;

public static class Endpoints
{
    private const string BasePath = "api/";
    public static class Auth
    {
        private const string AuthBasePath = BasePath + "auth/";
        public const string Authenticate = AuthBasePath + "login-battlenet";
        public const string GetUserInfo = AuthBasePath + "getuserinfo";
        public const string Challenge = AuthBasePath + "challenge";
        public const string Logout = AuthBasePath + "logout";
        public const string Register = AuthBasePath + "register";
    }
}