using Autofac;
using Microsoft.Extensions.Configuration;
using SaveApis.Common.Domains.Core.Application.DI;
using SaveApis.Common.Domains.Core.Infrastructure.DI;
using SaveApis.Common.Domains.Core.Infrastructure.Helper;
using SaveApis.Common.Domains.EfCore.Application.DI;
using SaveApis.Common.Domains.FileSystem.Application.DI;
using SaveApis.Common.Domains.Hangfire.Application.DI;
using SaveApis.Common.Domains.Hangfire.Domain.Types;
using SaveApis.Common.Domains.Logging.Application.DI;
using SaveApis.Common.Domains.Mapper.Application.DI;
using SaveApis.Common.Domains.Mediator.Application.DI;

namespace SaveApis.Common.Domains.Core.Infrastructure.Extensions;

public static class ContainerBuilderExtensions
{
    public static ContainerBuilder WithModule<TModule>(this ContainerBuilder builder, Action<TModule>? postRegistration = null, params object?[] args) where TModule : BaseModule
    {
        try
        {
            var module = (TModule)Activator.CreateInstance(typeof(TModule), args)!;

            builder.RegisterModule(module);
            postRegistration?.Invoke(module);

            return builder;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create instance of module {typeof(TModule).Name}", ex);
        }
    }

    public static ContainerBuilder WithCommonModules(this ContainerBuilder builder, IAssemblyHelper assemblyHelper, IConfiguration configuration, HangfireType hangfireType, Action<ContainerBuilder>? additionalModules = null)
    {
        builder.WithModule<CoreModule>(null, assemblyHelper);
        builder.WithModule<MediatorModule>(null, assemblyHelper);
        builder.WithModule<HangfireModule>(null, assemblyHelper, configuration, hangfireType);
        builder.WithModule<EfCoreModule>(null, assemblyHelper);
        builder.WithModule<MapperModule>(null, assemblyHelper);
        builder.WithModule<FileSystemModule>();
        builder.WithModule<SerilogModule>(null, configuration);

        additionalModules?.Invoke(builder);

        return builder;
    }
}
