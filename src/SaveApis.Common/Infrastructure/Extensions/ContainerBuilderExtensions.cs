using Autofac;
using SaveApis.Common.Application.DI;
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

    public static ContainerBuilder WithCommonModules(this ContainerBuilder builder, IAssemblyHelper helper)
    {
        builder.RegisterInstance(helper).As<IAssemblyHelper>().SingleInstance();

        builder.WithModule<FileSystemModule>();
        builder.WithModule<MediatorModule>(args: helper);
        builder.WithModule<SerilogModule>();

        return builder;
    }
}
