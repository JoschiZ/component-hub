// <auto-generated/>
using ComponentHub.ApiClients.Api.Testing.Redirect;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
namespace ComponentHub.ApiClients.Api.Testing {
    /// <summary>
    /// Builds and executes requests for operations under \api\testing
    /// </summary>
    public class TestingRequestBuilder : BaseRequestBuilder {
        /// <summary>The redirect property</summary>
        public RedirectRequestBuilder Redirect { get =>
            new RedirectRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>
        /// Instantiates a new TestingRequestBuilder and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public TestingRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/api/testing", pathParameters) {
        }
        /// <summary>
        /// Instantiates a new TestingRequestBuilder and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public TestingRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/api/testing", rawUrl) {
        }
    }
}
