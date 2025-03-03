using Autofac;
using Microsoft.Extensions.Configuration;
using SaveApis.Common.Application.DI;
using SaveApis.Common.Domain.Types;
using SaveApis.Common.Infrastructure.DI;
using SaveApis.Common.Infrastructure.Helper;

namespace SaveApis.Common.Infrastructure.Extensions;

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

    public static ContainerBuilder WithCommonModules(this ContainerBuilder builder, IConfiguration configuration,
        IAssemblyHelper helper, ApplicationType applicationType,
        Action<ContainerBuilder, IConfiguration, IAssemblyHelper, ApplicationType>? additionalModules = null)
    {
        builder.WithModule<CoreModule>(args: [helper, applicationType]);
        builder.WithModule<FileSystemModule>();
        builder.WithModule<MediatorModule>(args: [helper]);
        builder.WithModule<HangfireModule>(args: [configuration, helper, applicationType]);
        builder.WithModule<EfCoreModule>(args: [helper]);
        builder.WithModule<MapperModule>(args: [helper]);
        builder.WithModule<SerilogModule>();

        additionalModules?.Invoke(builder, configuration, helper, applicationType);

        return builder;
    }
}
