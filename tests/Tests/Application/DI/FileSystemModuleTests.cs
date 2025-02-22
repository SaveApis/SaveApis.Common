using System.IO.Abstractions;
using Autofac;
using Autofac.Core.Registration;
using SaveApis.Common.Application.DI;
using SaveApis.Common.Infrastructure.Extensions;

namespace Tests.Application.DI;

[TestFixture]
public class FileSystemModuleTests
{
    [Test]
    public void Can_Resolve_FileSystem_If_Registered()
    {
        // Arrange
        var container = new ContainerBuilder()
            .WithModule<FileSystemModule>()
            .Build();

        // Act & Assert
        Assert.DoesNotThrow(() => container.Resolve<IFileSystem>());
    }
    
    [Test]
    public void Cannot_Resolve_FileSystem_If_Not_Registered()
    {
        // Arrange
        var container = new ContainerBuilder().Build();

        // Act & Assert
        Assert.Throws<ComponentNotRegisteredException>(() => container.Resolve<IFileSystem>());
    }
}
