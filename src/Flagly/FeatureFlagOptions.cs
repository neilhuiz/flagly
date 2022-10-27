using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Flagly {

    /// <summary>
    /// Represents feature flags configuration.
    /// </summary>
    public class FeatureFlagOptions {

        /// <summary>
        /// Gets or sets the mapping of feature flag names to their status.
        /// </summary>
        public Dictionary<string, bool> Flags { get; set; } = new Dictionary<string, bool>();

        /// <summary>
        /// Gets or sets the list of feature flags that have been deprecated.
        /// Deprecated flags are always considered to be set even if they are
        /// missing or marked as disabled in <see cref="Flags"/>.
        /// </summary>
        public List<string> DeprecatedFlags { get; set; } = new List<string>();
    }
}
