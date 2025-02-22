using System.Reflection;
using SaveApis.Common.Application.Helper;

namespace Tests.Application.Helper;

[TestFixture]
public class AssemblyHelperTests
{
    [Test]
    public void Can_Return_Assemblies()
    {
        // Arrange
        var assemblyHelper = new AssemblyHelper(Assembly.GetExecutingAssembly());

        // Act
        var assemblies = assemblyHelper.GetAssemblies().ToList();

        // Assert
        Assert.That(assemblies, Has.Count.EqualTo(2));
        Assert.That(assemblies, Contains.Item(Assembly.GetExecutingAssembly()));
        Assert.That(assemblies, Contains.Item(typeof(AssemblyHelper).Assembly));
    }

    [Test]
    public void Can_Add_Assembly()
    {
        // Arrange
        var assemblyHelper = new AssemblyHelper(Assembly.GetExecutingAssembly());

        // Act
        assemblyHelper.AddAssembly(typeof(AssemblyHelper).Assembly);

        // Assert
        var assemblies = assemblyHelper.GetAssemblies().ToList();
        Assert.That(assemblies, Has.Count.EqualTo(2));
        Assert.That(assemblies, Contains.Item(Assembly.GetExecutingAssembly()));
        Assert.That(assemblies, Contains.Item(typeof(AssemblyHelper).Assembly));
    }
    
    [Test]
    public void Can_Add_Assemblies()
    {
        // Arrange
        var assemblyHelper = new AssemblyHelper(Assembly.GetExecutingAssembly());

        // Act
        assemblyHelper.AddAssemblies([typeof(AssemblyHelper).Assembly]);

        // Assert
        var assemblies = assemblyHelper.GetAssemblies().ToList();
        Assert.That(assemblies, Has.Count.EqualTo(2));
        Assert.That(assemblies, Contains.Item(Assembly.GetExecutingAssembly()));
        Assert.That(assemblies, Contains.Item(typeof(AssemblyHelper).Assembly));
    }
}
