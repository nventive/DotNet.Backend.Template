using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NV.Templates.Backend.Web.RestApi.General
{
    public class ApplicationInfoModel
    {
        public ApplicationInfoModel()
        {
        }

        public ApplicationInfoModel(IApplicationInfo appInfo)
        {
            Name = appInfo.Name;
            Version = appInfo.Version;
            Environment = appInfo.Environment;
        }

        [Required]
        [Description("The system name")]
        public string? Name { get; set; }

        [Required]
        [Description("The system version")]
        public string? Version { get; set; }

        [Required]
        [Description("The name of the environment")]
        public string? Environment { get; set; }
    }
}
