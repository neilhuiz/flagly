namespace Microsoft.Extensions.DependencyInjection {

    using System;
    using System.Threading;
    using Flagly;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Options;

    public static class ServiceCollectionExtensions {

        public const string DefaultConfigurationSection = "featureFlags";

        /// <summary>
        /// Adds the feature flags services and their dependencies
        /// and configures them using configuration from the default
        /// section.
        /// </summary>
        /// <param name="services"></param>
        /// <returns>
        /// An <see cref="OptionsBuilder{FeatureFlagOptions}"/> instance
        /// that you can use to customize the feature flag configuration.
        /// </returns>
        public static OptionsBuilder<FeatureFlagOptions> AddFeatureFlags(this IServiceCollection services) =>
            AddFeatureFlags(services, DefaultConfigurationSection);

        /// <summary>
        /// Adds the the feature flags services and their dependencies
        /// and configures them using the provided configuration section.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurationSection">
        /// The path to a configuration section (i.e. "my:feature:flags")
        /// that will be used to configure feature flags.
        /// </param>
        /// <returns>
        /// An <see cref="OptionsBuilder{FeatureFlagOptions}"/> instance
        /// that you can use to customize the feature flag configuration.
        /// </returns>
        public static OptionsBuilder<FeatureFlagOptions> AddFeatureFlags(
                this IServiceCollection services,
                string configurationSection
            ) {

            services.TryAddScoped<IFeatureFlagsProvider, FeatureFlagsProvider>();

            return services
                .AddOptions<FeatureFlagOptions>()
                .BindConfiguration(configurationSection);
        }
    }
}
