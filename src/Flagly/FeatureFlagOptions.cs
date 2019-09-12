using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Flagly {
    public class FeatureFlagOptions {

        /// <summary>
        /// Gets or sets the mapping of feature flag names to their status.
        /// </summary>
        public Dictionary<string, bool> Flags { get; set; } = new Dictionary<string, bool>();

        /// <summary>
        /// Gets or sets the list of feature flags that have been deprecated.
        /// Deprecated flags are always considered to be set even if they are
        /// marked as disabled in <see cref="Flags"/>.
        /// </summary>
        public List<string> DeprecatedFlags { get; set; } = new List<string>();

        /// <summary>
        /// Gets the cancellation token that indicates that feature flags have
        /// been updated and the application needs to take appropriate action.
        /// </summary>
        public CancellationToken RestartToken => TokenSource.Token;

        /// <summary>
        /// Gets or sets the token source.
        /// </summary>
        internal CancellationTokenSource TokenSource { get; set; } = new CancellationTokenSource();
    }
}
