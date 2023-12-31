// <auto-generated/>
using ComponentHub.ApiClients.Api.Auth.ExternalLogin;
using ComponentHub.ApiClients.Api.Auth.ExternalLoginCallback;
using ComponentHub.ApiClients.Api.Auth.Getuserinfo;
using ComponentHub.ApiClients.Api.Auth.LoginBattlenet;
using ComponentHub.ApiClients.Api.Auth.Logout;
using ComponentHub.ApiClients.Api.Auth.Register;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
namespace ComponentHub.ApiClients.Api.Auth {
    /// <summary>
    /// Builds and executes requests for operations under \api\auth
    /// </summary>
    public class AuthRequestBuilder : BaseRequestBuilder {
        /// <summary>The externalLogin property</summary>
        public ExternalLoginRequestBuilder ExternalLogin { get =>
            new ExternalLoginRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The externalLoginCallback property</summary>
        public ExternalLoginCallbackRequestBuilder ExternalLoginCallback { get =>
            new ExternalLoginCallbackRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The getuserinfo property</summary>
        public GetuserinfoRequestBuilder Getuserinfo { get =>
            new GetuserinfoRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The loginBattlenet property</summary>
        public LoginBattlenetRequestBuilder LoginBattlenet { get =>
            new LoginBattlenetRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The logout property</summary>
        public LogoutRequestBuilder Logout { get =>
            new LogoutRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The register property</summary>
        public RegisterRequestBuilder Register { get =>
            new RegisterRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>
        /// Instantiates a new AuthRequestBuilder and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public AuthRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/api/auth", pathParameters) {
        }
        /// <summary>
        /// Instantiates a new AuthRequestBuilder and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public AuthRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/api/auth", rawUrl) {
        }
    }
}
