using System;
using System.Collections.Generic;
using System.Linq;

namespace Dragablz.Core
{
    /// <summary>
    /// A registry that tracks living instances of <typeparamref name="T"/> using weak references,
    /// allowing garbage collection to reclaim instances that are no longer reachable.
    /// Supports registration, unregistration, enumeration of live instances, and cleanup.
    /// </summary>
    /// <typeparam name="T">The type of instances to track. Must be a reference type.</typeparam>
    internal class InstanceRegistry<T> where T : class
    {
        private readonly List<WeakReference<T>> _instances = new List<WeakReference<T>>();
        private readonly object _lock = new object();

        /// <summary>
        /// Registers an instance.
        /// </summary>
        public void Register(T instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            lock (_lock)
            {
                _instances.Add(new WeakReference<T>(instance));
            }
        }

        /// <summary>
        /// Unregisters an instance.
        /// </summary>
        public void Unregister(T instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            lock (_lock)
            {
                _instances.RemoveAll(wr =>
                {
                    return wr.TryGetTarget(out var target) && ReferenceEquals(target, instance);
                });
            }
        }

        /// <summary>
        /// Returns all currently alive registered instances.
        /// </summary>
        public IEnumerable<T> GetAliveInstances()
        {
            lock (_lock)
            {
                // Clean up dead references and collect alive ones
                var alive = new List<T>();
                _instances.RemoveAll(wr =>
                {
                    if (wr.TryGetTarget(out var target))
                    {
                        alive.Add(target);
                        return false;
                    }
                    return true; // remove dead weak references
                });
                return alive;
            }
        }

        /// <summary>
        /// Manually cleans up dead (garbage-collected) weak references.
        /// Called automatically by <see cref="GetAliveInstances"/>, but can also be called
        /// independently if needed.
        /// </summary>
        public void Cleanup()
        {
            lock (_lock)
            {
                _instances.RemoveAll(wr => !wr.TryGetTarget(out _));
            }
        }

        /// <summary>
        /// Clears all registered instances. Useful for test teardown.
        /// </summary>
        public void Clear()
        {
            lock (_lock)
            {
                _instances.Clear();
            }
        }
    }
}
