using ComponentHub.Domain.Features.Components;
using ComponentHub.Domain.Features.Users;

namespace Server.Tests.Features.Components;

public class UpdateComponentTest
{
    [Fact]
    public void Should_Update_Component()
    {
        // arrange
        var updateRequest = new UpdateComponentRequest(ComponentId.New())
        {
            Name = "New Name",
            SourceCode = "New Source",
            Width = 5,
            Height = 5,
            WclComponentId = Guid.NewGuid()
        };
        
        var componentResult = Component.TryCreate("abcd", new ApplicationUser(), "MyTestComp");
        
        Assert.True(componentResult.IsSuccess);
        
        var component = componentResult.ResultObject.UpdateComponent(updateRequest);

        component.Switch(
            component1 =>
            {
                Assert.Equal(updateRequest.Name, component1.Name);
                Assert.Equal(component1.Source.Height, updateRequest.Height);
            },
            error => Assert.Fail(error.ErrorCode),
            list => Assert.Fail(string.Join(", ", list)));
    }

}