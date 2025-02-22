using Autofac;
using SaveApis.Common.Infrastructure.DI;

namespace Tests.Infrastructure.Extensions;

public class TestModule(string parameter) : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        Console.WriteLine(parameter);
    }
}
