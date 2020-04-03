﻿using System;
using System.IO;
using System.Linq;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(NV.Templates.Backend.Functions.Startup))]

namespace NV.Templates.Backend.Functions
{
    /// <summary>
    /// Startup class that mimics the ASP.NET Core startup pattern.
    /// </summary>
    public class Startup : FunctionsStartup
    {
        /// <inheritdoc />
        public override void Configure(IFunctionsHostBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var rootProjectPath = Path.GetFullPath(Path.Combine("..", "..", "..", ".."));

            var configuration = new ConfigurationBuilder()
                .AddLocalSettings(rootProjectPath)
                .AddEnvironmentVariables()
                .AddUserSecrets<Startup>()
                .Build();

            builder.Services.AddSingleton<IConfiguration>(configuration);
            builder.Services.AddCore(configuration);

            // TODO: This is a patch for this: https://github.com/dotnet/extensions/issues/2846
            builder.Services.Remove(builder.Services.Single(s => s.ImplementationType?.Name == "HealthCheckPublisherHostedService"));
        }
    }
}
