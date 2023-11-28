using ComponentHub.Domain.Features.Components;

namespace Server.Tests.Features.Components;

public class ExportImportTest
{
    private readonly ComponentSource _testSource = ComponentSource.TryCreate
    (
        """
        getComponent = () => {
          return {
            component: 'EnhancedMarkdown',
            props: {
              content: 'Hello world!'
            }
          }
        }
        """,
        2,
        1,
        Guid.Parse("9850c88d-fce7-488b-a012-0a84d3777dd1")
    ).ResultObject!;

    private const string TestExportString =
        "N4Igxg9gtgDhB2BTeAXEAuUBnMAnAljGuiAOaIoDC0cSqABALz0AUAlEwDoCuADLwGYAovWCdcnePXq4K3XFLESp0+pFgJkKdPR78ATAHYh8ABYBDeGEQATALLncAaxsQA7vD28jAGnGTVehhcCBgsHSUAwLUEFC0dLyMACUQAG1SIejcIXFSbAEJEw38VaQBfEvKSspAynxBTDH16twwARnr8DBAATgAOAFZeMD6+mwBaADNrQ3GAFlGAI3HzXjb9cd5zPrmbAUMDmxs22qA===";
    


    [Fact]
    public void Should_Generate_Export_String()
    {
        var exportString = Component.EncodeExportString(_testSource);
        Assert.Equal(TestExportString, exportString);
    }

    [Fact]
    public void Should_Decode_Export_String()
    {
        var component = Component.DecodeExportString(TestExportString);
        Assert.Equal(_testSource, component.ResultObject);
    }
}