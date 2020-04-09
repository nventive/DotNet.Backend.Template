using System;
using System.Collections.Generic;

namespace NV.Templates.Backend.Core.Framework.HttpDependencies
{
    /// <summary>
    /// Base class for options related to <see cref="HttpClient"/>.
    /// </summary>
    public abstract class HttpClientOptions
    {
        /// <summary>
        /// Gets the default <see cref="Timeout"/> (10 seconds).
        /// </summary>
        public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Gets the default <see cref="NumberOfRetries"/> (3).
        /// </summary>
        public static readonly int DefaultNumberOfRetries = 3;

        /// <summary>
        /// Gets the default <see cref="RetriesSleepDuration"/> (300 ms).
        /// </summary>
        public static readonly TimeSpan DefaultRetriesSleepDuration = TimeSpan.FromMilliseconds(300);

        /// <summary>
        /// Gets the default <see cref="RetriesMaximumSleepDuration"/> (3 seconds).
        /// </summary>
        public static readonly TimeSpan DefaultRetriesMaximumSleepDuration = TimeSpan.FromSeconds(3);

        /// <summary>
        /// Gets the default <see cref="ErrorsAllowedBeforeBreaking"/> (10).
        /// </summary>
        public static readonly int DefaultErrorsAllowedBeforeBreaking = 10;

        /// <summary>
        /// Gets the default <see cref="BreakDuration"/> (1 minute).
        /// </summary>
        public static readonly TimeSpan DefaultBreakDuration = TimeSpan.FromMinutes(1);

        /// <summary>
        /// Gets the default <see cref="MaxParallelization"/>. (0 = disabled).
        /// </summary>
        public static readonly int DefaultMaxParallelization = 0;

        /// <summary>
        /// Gets or sets the <see cref="HttpClient.BaseAddress"/> value.
        /// </summary>
        public Uri? BaseAddress { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="HttpClient.Timeout"/> value.
        /// </summary>
        public TimeSpan Timeout { get; set; } = DefaultTimeout;

        /// <summary>
        /// Gets or sets the default headers.
        /// </summary>
        public Dictionary<string, string?>? Headers { get; set; }

        /// <summary>
        /// Gets or sets the number of automatic retries in case of transient HTTP failures.
        /// Set to 0 to disable automatic retries.
        /// </summary>
        public int NumberOfRetries { get; set; } = DefaultNumberOfRetries;

        /// <summary>
        /// Gets or sets the sleep duration in-between retries in case of transient HTTP failures.
        /// This is the minimum sleep duration that will be applied in case of Jitter retries.
        /// </summary>
        public TimeSpan RetriesSleepDuration { get; set; } = DefaultRetriesSleepDuration;

        /// <summary>
        /// Gets or sets the maximum sleep duration in-between retries in case of transient HTTP failures.
        /// Set to 00:00:00 to disable Jittered retries and have a constant retry sleep duration applied (using <see cref="RetriesSleepDuration"/>).
        /// </summary>
        public TimeSpan RetriesMaximumSleepDuration { get; set; } = DefaultRetriesMaximumSleepDuration;

        /// <summary>
        /// Gets or sets the number of errors to allow before the Circuit Breaker opens.
        /// Set to 0 to disable the Circuit Breaker.
        /// </summary>
        public int ErrorsAllowedBeforeBreaking { get; set; } = DefaultErrorsAllowedBeforeBreaking;

        /// <summary>
        /// Gets or sets the duration of a break when the circuit breaker opens.
        /// </summary>
        public TimeSpan BreakDuration { get; set; } = DefaultBreakDuration;

        /// <summary>
        /// Gets or sets the maximum number of parallel calls allowed.
        /// Set to 0 for unlimited parallel requests.
        /// </summary>
        public int MaxParallelization { get; set; } = DefaultMaxParallelization;
    }
}
