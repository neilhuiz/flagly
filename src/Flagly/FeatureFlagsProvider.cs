using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Flagly
{
    public class FeatureFlagsProvider : IFeatureFlagsProvider, IDisposable
    {
        // Static delegate instances for use with the methods that only
        // have a "ifEnabled" action parameter.
        private static readonly Action _noop = () => { };
        private static readonly Func<Task> _noopTask = () => Task.CompletedTask;

        private readonly IOptionsMonitor<FeatureFlagOptions> _options;
        private readonly IDisposable _optionsChangeListenerHandle;

        private bool _isDisposed;
        private IDictionary<string, bool> _flags = new Dictionary<string, bool>();
        private IEnumerable<string> _deprecatedFlags = Enumerable.Empty<string>();

        /// <summary>
        /// Gets the current options instance.
        /// </summary>
        protected FeatureFlagOptions Options => _options.CurrentValue;

        /// <summary>
        /// Gets the <see cref="ILogger"/> instance.
        /// </summary>
        protected ILogger Logger { get; }        

        /// <summary>
        /// Creates a new instance of <see cref="FeatureFlagsProvider"/>.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public FeatureFlagsProvider(
                IOptionsMonitor<FeatureFlagOptions> options,
                ILogger<FeatureFlagsProvider> logger
            )
        {
            _options = options;
            Logger = logger;

            OptionsChangeListener(_options.CurrentValue, string.Empty);
            _optionsChangeListenerHandle = _options.OnChange(OptionsChangeListener);
        }

        /// <inheritdoc />
        public void Execute(string flagName, Action ifEnabled) =>
            Execute(flagName, ifEnabled, _noop);

        /// <inheritdoc />
        public void Execute(string flagName, Action ifEnabled, Action ifDisabled)
        {
            if (IsEnabled(flagName))
            {
                Logger.LogDebug("Flag '{flagName}' is enabled", flagName);
                ifEnabled.Invoke();
            }

            Logger.LogDebug("Flag '{flagName}' is NOT enabled", flagName);
            ifDisabled?.Invoke();
        }

        /// <inheritdoc />
        public Task ExecuteAsync(string flagName, Func<Task> ifEnabled) =>
            ExecuteAsync(flagName, ifEnabled, _noopTask);

        /// <inheritdoc />
        public async Task ExecuteAsync(string flagName, Func<Task> ifEnabled, Func<Task> ifDisabled)
        {
            if (IsEnabled(flagName))
            {
                Logger.LogDebug("Flag '{flagName}' is enabled", flagName);
                await ifEnabled.Invoke();
            }

            Logger.LogDebug("Flag '{flagName}' is NOT enabled", flagName);
            await ifDisabled.Invoke();
        }

        /// <inheritdoc />
        public async Task<T> ExecuteAsync<T>(string flagName, Func<Task<T>> ifEnabled, Func<Task<T>> ifDisabled)
        {
            if (IsEnabled(flagName))
            {
                Logger.LogDebug("Flag '{flagName}' is enabled", flagName);
                return await ifEnabled.Invoke();
            }

            Logger.LogDebug("Flag '{flagName}' is NOT enabled", flagName);
            return await ifDisabled.Invoke();
        }

        /// <inheritdoc />
        public bool IsEnabled(string flagName)
        {
            // When a feature flag is deprecated, it is the same as
            // having it be enabled at all times.
            if (_deprecatedFlags.Contains(flagName))
            {
                return true;
            }
            // Otherwise, we look up the flag to see if it is enabled or not.
            if (_flags.TryGetValue(flagName, out bool enabled))
            {
                return enabled;
            }
            //If we don't find an entry for the flag, we assume it is disabled.
            return false;
        }

        private void OptionsChangeListener(FeatureFlagOptions options, string name)
        {
            Logger.LogDebug("Reloading feature flags ...");
            // Configuration keys are assumed to be case-insensitive so we
            // will use a case-insensitive dictionary instance to store
            // the feature flags.
            var flags = new Dictionary<string, bool>(options.Flags, StringComparer.OrdinalIgnoreCase);
            Interlocked.Exchange(ref _flags, flags);

            // Again, since configuration is assumed to be case-insensitive,
            // we convert to a case-insensitive HashSet<string>. This also
            // helps with performance in comparison to a List<string>.
            var deprecatedFlags = new HashSet<string>(options.DeprecatedFlags, StringComparer.OrdinalIgnoreCase);
            Interlocked.Exchange(ref _deprecatedFlags, deprecatedFlags);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _optionsChangeListenerHandle.Dispose();
                }

                _isDisposed = true;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
