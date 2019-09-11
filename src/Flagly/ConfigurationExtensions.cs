namespace Microsoft.Extensions.Configuration {

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using Microsoft.Extensions.Primitives;

    public static class ConfigurationExtensions {

        public static CancellationTokenSource CreateCancellationToken(this IConfigurationRoot config) {
            if (!config.GetReloadToken().ActiveChangeCallbacks) {
                throw new NotSupportedException("Current configuration does not support change callbacks.");
            }

            var source = new CancellationTokenSource();
            ChangeToken.OnChange(
                    () => config.GetReloadToken(),
                    cts => cts.Cancel(),
                    source);
            return source;
        }
    }
}
