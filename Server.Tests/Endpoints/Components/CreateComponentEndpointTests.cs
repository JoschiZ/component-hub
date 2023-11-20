using System.Collections;
using System.Security.Claims;
using ComponentHub.DB.BaseClasses;
using ComponentHub.DB.Features.Components;
using ComponentHub.DB.Features.User;
using ComponentHub.Server.Features.Components;
using FastEndpoints;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Server.Tests.MockingHelpers;
using Xunit.Abstractions;
using Xunit.Priority;

namespace Server.Tests.Endpoints.Components;

public sealed class CreateComponentEndpointTests
{
    private readonly ITestOutputHelper _outputHelper;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly UserManager<ApplicationUser> _userManagerMock;
    private readonly ILogger<CreateComponentEndpoint> _logger;
    private readonly Guid UserId = Guid.NewGuid();
    private readonly ClaimsPrincipal _fakeIdentity;
    
    
    public CreateComponentEndpointTests(ITestOutputHelper outputHelper)
    {
        Assert.True(true);
        return;
        var idClaim = new Claim(ClaimTypes.NameIdentifier, UserId.ToString());
        var identity = new ClaimsIdentity(new []{idClaim});
        _fakeIdentity = new ClaimsPrincipal(identity);
        _outputHelper = outputHelper;
        
        _userManagerMock = MockUserManager.CreateUserManager<ApplicationUser>();
        //_userManagerMock.GetUserId(_fakeIdentity).ReturnsForAnyArgs(UserId.ToString());
        _userManagerMock.FindByIdAsync(UserId.ToString()).Returns(new ApplicationUser() {Id = UserId});

        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        
        _logger = new XunitLogger<CreateComponentEndpoint>(outputHelper);
    }

    [Theory, Priority(1)]
    [ClassData(typeof(InvalidCreateComponentRequestData))]
    public async Task Invalid_Request(CreateComponentRequest request)
    {
        // Arrange
        var validator = new CreateComponentRequestValidator();

        // Act
        var validation = await validator.ValidateAsync(request);

        // Assert
        Assert.False(validation.IsValid);
    }

    private sealed class InvalidCreateComponentRequestData: IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {

            yield return new[]
            {
                new CreateComponentRequest()
                {
                    Language = Language.JS,
                    SourceCode = "",
                    Name = ""
                }
            };
            yield return new[]
            {
                new CreateComponentRequest()
                {
                    Language = Language.TS,
                    SourceCode = "asdadsadasd",
                    Name = "asdadasdsaddsadsaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
                }
            };
            yield return new[]
            {
                new CreateComponentRequest()
                {
                    Language = Language.JS,
                    SourceCode = " ",
                    Name = " "
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}