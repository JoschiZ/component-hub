using Riok.Mapperly.Abstractions;

namespace ComponentHub.Domain.Features.Users;


public sealed record PublicUserDto(string UserName);


[Mapper]
public static partial class ApplicationUserMapper
{
    public static partial PublicUserDto ToDto(this ApplicationUser applicationUser);

    public static partial IQueryable<PublicUserDto> ProjectToDto(this IQueryable<ApplicationUser> applicationUsers);
}