using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Server.Tests.MockingHelpers;

internal static class MockUserManager
{
    public static UserManager<TUser> CreateUserManager<TUser>() where TUser : class
    {
        var userPasswordStore = Substitute.For<IUserPasswordStore<TUser>>();

        userPasswordStore.CreateAsync(default, default).ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));
        var options = Substitute.For<IOptions<IdentityOptions>>();
        
        var idOptions = new IdentityOptions();

        //this should be keep in sync with settings in ConfigureIdentity in WebApi -> Startup.cs
        idOptions.Lockout.AllowedForNewUsers = false;
        idOptions.SignIn.RequireConfirmedEmail = false;

        // Lockout settings.
        idOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        idOptions.Lockout.MaxFailedAccessAttempts = 5;
        idOptions.Lockout.AllowedForNewUsers = true;
        
        options.Value.Returns(idOptions);
        
        var userValidators = new List<IUserValidator<TUser>>();
        UserValidator<TUser> validator = new UserValidator<TUser>();
        userValidators.Add(validator);

        var passValidator = new PasswordValidator<TUser>();
        var pwdValidators = new List<IPasswordValidator<TUser>>();
        pwdValidators.Add(passValidator);

        var userManager = new UserManager<TUser>(
            userPasswordStore, 
            options, 
            new PasswordHasher<TUser>(),
            userValidators, 
            pwdValidators, 
            new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(), 
            null,
            Substitute.For<ILogger<UserManager<TUser>>>());

        return userManager;
    }
}