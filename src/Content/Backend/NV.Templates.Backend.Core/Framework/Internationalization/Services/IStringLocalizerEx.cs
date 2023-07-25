using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Localization;

namespace NV.Templates.Backend.Core.Framework.Internationalization
{
    public interface IStringLocalizerEx : IStringLocalizer
    {
        string GetString(string name, CultureInfo culture, params object[] arguments);

        IDictionary<string, string> GetStrings(string name, GetStringsMode getStringsMode = GetStringsMode.OnePerCulture, params object[] arguments);

        IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures, params object[] arguments);
    }
}
