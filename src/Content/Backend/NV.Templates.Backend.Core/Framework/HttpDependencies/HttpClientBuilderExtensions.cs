using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NV.Templates.Backend.Core.General;
using Polly;

namespace NV.Templates.Backend.Core.Framework.HttpDependencies
{
    /// <summary>
    /// <see cref="IHttpClientBuilder"/> extensions.
    /// </summary>
    public static class HttpClientBuilderExtensions
    {
        public static IHttpClientBuilder AddHttpClient<TApi, TOptions>(
            this IServiceCollection services,
            IConfiguration configuration)
            where TOptions : HttpDependencyOptions, new()
        {
            var httpClientName = $"{typeof(TApi).Name}Client";

            services.BindOptionsToConfigurationAndValidate<TOptions>(configuration);

            var builder = services.AddHttpClient(
                httpClientName,
                (sp, client) =>
                {
                    var options = sp.GetRequiredService<IOptionsMonitor<TOptions>>().CurrentValue;
                    if (options.BaseAddress != null)
                    {
                        client.BaseAddress = options.BaseAddress;
                    }

                    if (options.Headers != null)
                    {
                        foreach (var header in options.Headers.Where(x => !string.IsNullOrEmpty(x.Value)))
                        {
                            client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                        }
                    }

                    if (client.DefaultRequestHeaders.UserAgent.Count == 0)
                    {
                        var appInfo = sp.GetRequiredService<IApplicationInfo>();
                        client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", $"{appInfo.Name}/{appInfo.Version} ({appInfo.Environment})");
                    }
                });

            var options = configuration
                .GetSection(RegistrationServiceCollectionExtensions.DefaultOptionsName<TOptions>())
                .Get<TOptions>()
                ?? new TOptions();

            if (options.Timeout != TimeSpan.Zero)
            {
                builder = builder.AddPolicyHandler(
                    Policy.TimeoutAsync(options.Timeout).AsAsyncPolicy<HttpResponseMessage>());
            }

            if (options.ErrorsAllowedBeforeBreaking > 0)
            {
                builder = builder.AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(options.ErrorsAllowedBeforeBreaking, options.BreakDuration));
            }

            if (options.NumberOfRetries > 0)
            {
                if (options.RetriesMaximumSleepDuration == TimeSpan.FromTicks(0))
                {
                    builder = builder.AddTransientHttpErrorPolicy(
                        p => p.WaitAndRetryAsync(options.NumberOfRetries, _ => options.RetriesSleepDuration));
                }
                else
                {
                    builder = builder.AddTransientHttpErrorPolicy(
                        p => p.WaitAndRetryAsync(
                            DecorrelatedJitter(options.NumberOfRetries, options.RetriesSleepDuration, options.RetriesMaximumSleepDuration)));
                }
            }

            if (options.MaxParallelization > 0)
            {
                builder = builder.AddPolicyHandler(
                    Policy.BulkheadAsync(options.MaxParallelization).AsAsyncPolicy<HttpResponseMessage>());
            }

            builder = builder.AddHttpTracing(bufferRequests: true);

            return builder;
        }

        private static IEnumerable<TimeSpan> DecorrelatedJitter(int maxRetries, TimeSpan seedDelay, TimeSpan maxDelay)
        {
            var jitterer = new Random();
            var retries = 0;

            var seed = seedDelay.TotalMilliseconds;
            var max = maxDelay.TotalMilliseconds;
            var current = seed;

            while (++retries <= maxRetries)
            {
                // Adopting the 'Decorrelated Jitter' formula from https://www.awsarchitectureblog.com/2015/03/backoff.html.
                // Can be between seed and previous * 3.  Mustn't exceed max.
                current = Math.Min(max, Math.Max(seed, current * 3 * jitterer.NextDouble()));
                yield return TimeSpan.FromMilliseconds(current);
            }
        }
    }
}
