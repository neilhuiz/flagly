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

        void Execute(string flagName, Action ifEnabled);
        void Execute(string flagName, Action ifEnabled, Action ifDisabled);
        void Execute(Func<IEnumerable<KeyValuePair<string, bool>>, bool> predicate, Action ifEnabled);
        void Execute(Func<IEnumerable<KeyValuePair<string, bool>>, bool> predicate, Action ifEnabled, Action ifDisabled);

        Task ExecuteAsync(string flagName, Func<Task> ifEnabled);
        Task ExecuteAsync(string flagName, Func<Task> ifEnabled, Func<Task> ifDisabled);
        Task ExecuteAsync(Func<IEnumerable<KeyValuePair<string, bool>>, bool> predicate, Func<Task> ifEnabled);
        Task ExecuteAsync(Func<IEnumerable<KeyValuePair<string, bool>>, bool> predicate, Func<Task> ifEnabled, Func<Task> ifDisabled);

        Task<T> ExecuteAsync<T>(string flagName, Func<Task<T>> ifEnabled);
        Task<T> ExecuteAsync<T>(string flagName, Func<Task<T>> ifEnabled, Func<Task<T>> ifDisabled);
        Task<T> ExecuteAsync<T>(Func<IEnumerable<KeyValuePair<string, bool>>, bool> predicate, Func<Task<T>> ifEnabled);
        Task<T> ExecuteAsync<T>(Func<IEnumerable<KeyValuePair<string, bool>>, bool> predicate, Func<Task<T>> ifEnabled, Func<Task<T>> ifDisabled);

    }
}
