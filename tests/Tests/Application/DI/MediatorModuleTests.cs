using System.Reflection;
using Autofac;
using Autofac.Core.Registration;
using MediatR;
using SaveApis.Common.Application.DI;
using SaveApis.Common.Application.Helper;
using SaveApis.Common.Infrastructure.Extensions;
using Tests.Application.DI.Mediator.Commands;
using Tests.Application.DI.Mediator.Queries;

namespace Tests.Application.DI;

[TestFixture]
public class MediatorModuleTests
{
    [Test]
    public void Can_Resolve_IMediator_If_Registered()
    {
        // Arrange
        var helper = new AssemblyHelper(Assembly.GetExecutingAssembly());
        var container = new ContainerBuilder()
            .WithModule<MediatorModule>(args: helper)
            .Build();

        // Act & Assert
        Assert.DoesNotThrow(() => container.Resolve<IMediator>());
    }
    
    [Test]
    public void Cannot_Resolve_IMediator_If_Not_Registered()
    {
        // Arrange
        var container = new ContainerBuilder().Build();

        // Act & Assert
        Assert.Throws<ComponentNotRegisteredException>(() => container.Resolve<IMediator>());
    }

    [Test]
    public async Task Can_Execute_Query()
    {
        // Arrange
        var helper = new AssemblyHelper(Assembly.GetExecutingAssembly());
        var container = new ContainerBuilder()
            .WithModule<MediatorModule>(args: helper)
            .Build();

        var mediator = container.Resolve<IMediator>();

        // Act
        var result = await mediator.Send(new TestQuery());

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess,  Is.True);
            Assert.That(result.Value, Is.EqualTo("Test")); 
        });
    }

    [Test]
    public async Task Can_Execute_Command()
    {
        // Arrange
        var helper = new AssemblyHelper(Assembly.GetExecutingAssembly());
        var container = new ContainerBuilder()
            .WithModule<MediatorModule>(args: helper)
            .Build();

        var mediator = container.Resolve<IMediator>();

        // Act
        var result = await mediator.Send(new TestCommand());

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess,  Is.True);
            Assert.That(result.Value, Is.EqualTo("Test")); 
        });
    }
}
