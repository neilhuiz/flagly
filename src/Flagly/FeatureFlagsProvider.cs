using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Extensions.Options;

namespace Flagly {
    public class FeatureFlagsProvider : IFeatureFlagsProvider {

        private readonly IOptions<FeatureFlagOptions> _options;

        protected FeatureFlagOptions Options => _options.Value;

        public FeatureFlagsProvider(IOptions<FeatureFlagOptions> options) {
            _options = options;
        }

        public void Execute(string flagName, Action ifEnabled, Action? ifDisabled = null) {
            if(IsEnabled(flagName)) {
                ifEnabled.Invoke();
            } else {
                ifDisabled?.Invoke();
            }
        }

        public async Task ExecuteAsync(string flagName, Func<Task> ifEnabled, Func<Task>? ifDisabled = null) {
            if (IsEnabled(flagName)) {
                await ifEnabled.Invoke();
            } else if (ifDisabled != null) {
                await ifDisabled.Invoke();
            }
        }

        public async Task<T> ExecuteAsync<T>(string flagName, Func<Task<T>> ifEnabled, Func<Task<T>>? ifDisabled = null) {
            if (IsEnabled(flagName)) {
                return await ifEnabled.Invoke();
            } else if(ifDisabled != null) {
                return await ifDisabled.Invoke();
            }
#pragma warning disable CS8653 // A default expression introduces a null value for a type parameter.
            return default;
#pragma warning restore CS8653 // A default expression introduces a null value for a type parameter.
        }

        public bool IsEnabled(string flagName) {

            if(Options.DeprecatedFlags.Contains(flagName)) {
                return true;
            }

            if(Options.Flags.TryGetValue(flagName, out bool enabled)) {
                return enabled;
            }

            return false;
        }
    }
}
