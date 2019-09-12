using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Flagly {
    /// <summary>
    /// Provides methods to help with implementing conditional execution
    /// based on whether feature flags are enabled or disabled.
    /// </summary>
    public interface IFeatureFlagsProvider {

        bool IsEnabled(string flagName);

        void Execute(string flagName, Action ifEnabled, Action? ifDisabled = null);

        Task ExecuteAsync(string flagName, Func<Task> ifEnabled, Func<Task>? ifDisabled = null);

        Task<T> ExecuteAsync<T>(string flagName, Func<Task<T>> ifEnabled, Func<Task<T>>? ifDisabled = null);
    }
}
