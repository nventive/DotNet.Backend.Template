using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NodaTime;
using NodaTime.Serialization.JsonNet;

namespace NV.Templates.Backend.Functions
{
    /// <summary>
    /// Holds default <see cref="JsonSerializerSettings"/> for Azure Functions.
    /// </summary>
    public static class SerializerSettings
    {
        /// <summary>
        /// Gets the default <see cref="JsonSerializerSettings"/>.
        /// </summary>
        public static JsonSerializerSettings JsonSerializerSettings { get; } = CreateJsonSerializerSettings();

        private static JsonSerializerSettings CreateJsonSerializerSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy(),
            };
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

            return settings;
        }
    }
}
