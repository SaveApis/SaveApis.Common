﻿using Autofac;
using SaveApis.Common.Domains.Core.Infrastructure.DI;

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
}
