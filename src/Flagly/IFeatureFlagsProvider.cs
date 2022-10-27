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

        /// <summary>
        /// Gets whether a feature flag is enabled or not.
        /// </summary>
        /// <param name="flagName">The name of the feature flag.</param>
        /// <returns>True if the flag is enabled and not deprecated; false otherwise.</returns>
        bool IsEnabled(string flagName);

        /// <summary>
        /// Execute an action if a feature flag is enabled.
        /// </summary>
        /// <param name="flagName">The name of the feature flag.</param>
        /// <param name="ifEnabled">The action to execute if the flag is enabled.</param>
        void Execute(string flagName, Action ifEnabled);

        /// <summary>
        /// Execute one of two actions, based on the status
        /// of a feature flag.
        /// </summary>
        /// <param name="flagName">The name of the feature flag.</param>
        /// <param name="ifEnabled">The action to execute if the flag is enabled.</param>
        /// <param name="ifDisabled">The action to execute if the flag is NOT enabled.</param>
        void Execute(string flagName, Action ifEnabled, Action ifDisabled);

        /// <summary>
        /// Asynchronously execute an action if a feature flag is enabled.
        /// </summary>
        /// <param name="flagName">The name of the feature flag.</param>
        /// <param name="ifEnabled">The action to execute if the flag is enabled.</param>
        Task ExecuteAsync(string flagName, Func<Task> ifEnabled);

        /// <summary>
        /// Asynchronously execute one of two actions, based on the status
        /// of a feature flag.
        /// </summary>
        /// <param name="flagName">The name of the feature flag.</param>
        /// <param name="ifEnabled">The action to execute if the flag is enabled.</param>
        /// <param name="ifDisabled">The action to execute if the flag is NOT enabled.</param>
        Task ExecuteAsync(string flagName, Func<Task> ifEnabled, Func<Task> ifDisabled);

        /// <summary>
        /// Asynchronously execute one of two actions, based on the status
        /// of a feature flag and return the result of the action.
        /// </summary>
        /// <param name="flagName">The name of the feature flag.</param>
        /// <param name="ifEnabled">The action to execute if the flag is enabled.</param>
        /// <param name="ifDisabled">The action to execute if the flag is NOT enabled.</param>
        Task<T> ExecuteAsync<T>(string flagName, Func<Task<T>> ifEnabled, Func<Task<T>> ifDisabled);
    }
}
