namespace Microsoft.Extensions.DependencyInjection {

    using System;
    using System.Threading;
    using Flagly;
    using Microsoft.Extensions.Configuration;

    public static class ServiceCollectionExtensions {

        private const string DefaultConfigurationSection = "featureFlags";

        /// <summary>
        /// Adds the the feature flags services and their dependencies.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="cancellationSource"></param>
        /// <param name="config"></param>
        /// <param name="configOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddFeatureFlags(
                this IServiceCollection services,
                CancellationTokenSource cancellationSource,
                IConfiguration config,
                Action<FeatureFlagOptions> configOptions
            ) {

            return services.Configure<FeatureFlagOptions>(config)
                .PostConfigure<FeatureFlagOptions>(o => {
                    configOptions.Invoke(o);
                    o.TokenSource = cancellationSource;
                })
                .AddScoped<IFeatureFlagsProvider>();
        }
    }
}
