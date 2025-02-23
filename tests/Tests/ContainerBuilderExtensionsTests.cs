using System.Reflection;
using Autofac;
using Microsoft.Extensions.Configuration;
using SaveApis.Common.Application.Helper;
using SaveApis.Common.Infrastructure.Extensions;

namespace Tests;

[TestFixture]
public class ContainerBuilderExtensionsTests
{
    [Test]
    public void Can_Register_Common_Modules()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().AddInMemoryCollection().Build();
        var helper = new AssemblyHelper(Assembly.GetExecutingAssembly());
        var builder = new ContainerBuilder();
        
        // Act
        Assert.DoesNotThrow(() => builder.WithCommonModules(configuration, helper));
    }
}
