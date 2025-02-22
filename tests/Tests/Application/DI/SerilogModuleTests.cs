using Autofac;
using Autofac.Core.Registration;
using SaveApis.Common.Application.DI;
using SaveApis.Common.Infrastructure.Extensions;
using Serilog;

namespace Tests.Application.DI;

[TestFixture]
public class SerilogModuleTests
{
    [Test]
    public void Can_Resolve_ILogger_If_Registered()
    {
        // Arrange
        var container = new ContainerBuilder()
            .WithModule<SerilogModule>()
            .Build();

        // Act & Assert
        Assert.DoesNotThrow(() => container.Resolve<ILogger>());
    }

    [Test]
    public void Cannot_Resolve_ILogger_If_Not_Registered()
    {
        // Arrange
        var container = new ContainerBuilder().Build();

        // Act & Assert
        Assert.Throws<ComponentNotRegisteredException>(() => container.Resolve<ILogger>());
    }
}
