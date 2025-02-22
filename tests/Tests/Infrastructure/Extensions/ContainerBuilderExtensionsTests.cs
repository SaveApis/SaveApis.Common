using Autofac;
using SaveApis.Common.Infrastructure.Extensions;

namespace Tests.Infrastructure.Extensions;

[TestFixture]
public class ContainerBuilderExtensionsTests
{
    [Test]
    public void Can_Register_Module()
    {
        // Arrange
        var builder = new ContainerBuilder();

        // Act & Assert
        Assert.DoesNotThrow(() => builder.WithModule<TestModule>(args: string.Empty));
    }

    [Test]
    public void Cannot_Register_Module_Wrong_Parameter_Count()
    {
        // Arrange
        var builder = new ContainerBuilder();

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => builder.WithModule<TestModule>());

        Assert.That(exception.Message, Is.EqualTo($"Failed to create instance of module {nameof(TestModule)}"));
    }

    [Test]
    public void Can_Register_Common_Modules()
    {
        // Arrange
        var builder = new ContainerBuilder();

        // Act & Assert
        Assert.DoesNotThrow(() => builder.WithCommonModules());
    }
}
