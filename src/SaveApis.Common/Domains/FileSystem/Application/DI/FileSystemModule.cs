using System.IO.Abstractions;
using Autofac;
using SaveApis.Common.Domains.Core.Infrastructure.DI;

namespace SaveApis.Common.Domains.FileSystem.Application.DI;

public class FileSystemModule : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<System.IO.Abstractions.FileSystem>().As<IFileSystem>();
    }
}
