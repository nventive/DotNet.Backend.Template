using Newtonsoft.Json;
using NV.Templates.Backend.Core.General;

namespace NV.Templates.Backend.Web.General
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

        public string Name { get; }

        public string Version { get; }

        public string Environment { get; }
    }
}
