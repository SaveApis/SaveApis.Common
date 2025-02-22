using System.IO.Abstractions;
using Autofac;
using SaveApis.Common.Infrastructure.DI;

namespace SaveApis.Common.Application.DI;

public class FileSystemModule : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<FileSystem>().As<IFileSystem>().InstancePerLifetimeScope();
    }
}
