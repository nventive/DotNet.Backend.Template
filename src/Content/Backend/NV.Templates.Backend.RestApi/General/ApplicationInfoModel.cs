using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using NV.Templates.Backend.Core.General;

namespace NV.Templates.Backend.RestApi.General
{
    public class ApplicationInfoModel
    {
        [JsonConstructor]
        public ApplicationInfoModel(
            string name,
            string version,
            string environment)
        {
            Name = name;
            Version = version;
            Environment = environment;
        }

        public ApplicationInfoModel(IApplicationInfo appInfo)
            : this(appInfo.Name, appInfo.Version, appInfo.Environment)
        {
        }

        [Required]
        [Description("The system name")]
        public string Name { get; }

        [Required]
        [Description("The system version")]
        public string Version { get; }

        [Required]
        [Description("The name of the environment")]
        public string Environment { get; }
    }
}
