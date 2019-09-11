namespace Microsoft.Extensions.DependencyInjection {

    using System;
    using System.Threading;
    using Flagly;
    using Microsoft.Extensions.Configuration;

    public static class ServiceCollectionExtensions {

        private const string DefaultConfigurationSection = "featureFlags";

        /// <summary>
        /// Adds feature flags based on configuration.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddFeatureFlags(
                this IServiceCollection services,
                IConfigurationRoot config
            ) {

            return services.AddFeatureFlags(
                config.CreateCancellationToken(),
                config.GetSection(DefaultConfigurationSection)
            );
        }

        public static IServiceCollection AddFeatureFlags(
                this IServiceCollection services,
                Action<FeatureFlagOptions> configOptions
            ) {

            return services.AddFeatureFlags(new CancellationTokenSource(), configOptions);
        }

        public static IServiceCollection AddFeatureFlags(
                this IServiceCollection services,
                IConfiguration config
            ) {

            return services.AddFeatureFlags(new CancellationTokenSource(), config);
        }

        public static IServiceCollection AddFeatureFlags(
                this IServiceCollection services,
                IConfiguration config,
                Action<FeatureFlagOptions> configOptions
            ) {

            return services.AddFeatureFlags(new CancellationTokenSource(), config, configOptions);
        }

        public static IServiceCollection AddFeatureFlags(
                this IServiceCollection services,
                CancellationTokenSource cancellationSource
            ) {

            return services.AddFeatureFlags(cancellationSource, null, null);
        }

        public static IServiceCollection AddFeatureFlags(
                this IServiceCollection services,
                CancellationTokenSource cancellationSource,
                IConfiguration config                
            ) {

            return services.AddFeatureFlags(cancellationSource, config, null);
        }

        public static IServiceCollection AddFeatureFlags(
                this IServiceCollection services,
                CancellationTokenSource cancellationSource,
                Action<FeatureFlagOptions> configOptions
            ) {

            return services.AddFeatureFlags(cancellationSource, null, configOptions);
        }

        public static IServiceCollection AddFeatureFlags(
                this IServiceCollection services,
                CancellationTokenSource cancellationSource,
                IConfiguration config,
                Action<FeatureFlagOptions> configOptions
            ) {

            if(cancellationSource == null) { throw new ArgumentNullException(nameof(cancellationSource)); }

            if (config != null) { services.Configure<FeatureFlagOptions>(config); }

            services.PostConfigure<FeatureFlagOptions>(o => {
                configOptions?.Invoke(o);
                o.TokenSource = cancellationSource;
            });

            return services.AddScoped<IFeatureFlagsProvider>();
        }
    }
}
