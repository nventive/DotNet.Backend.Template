#nullable disable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using Microsoft.Extensions.Localization;
using NV.Templates.Backend.Core.Framework.Internationalization;
using NV.Templates.Resources;

namespace NV.Templates.Backend.Core.Framework.Internationalization.Services.Impl
{
    public class StringLocalizerEx : ResourceManager, IStringLocalizerEx
    {
        private readonly IStringLocalizer _localizer;

        public StringLocalizerEx(IStringLocalizer localizer)
            : base(typeof(SharedResources))
        {
            _localizer = localizer;
        }

        public LocalizedString this[string name] => _localizer[name];

        public LocalizedString this[string name, params object[] arguments] => _localizer[name, arguments];

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) => GetAllStrings(includeParentCultures, Array.Empty<object>());

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures, params object[] arguments)
            => _localizer.GetAllStrings(includeParentCultures).Select(localizedString =>
            {
                var value = localizedString.Value;
                if (!localizedString.ResourceNotFound)
                {
                    value = string.Format(CultureInfo.CurrentCulture, value, arguments);
                }

                return new LocalizedString(localizedString.Name, value, localizedString.ResourceNotFound, localizedString.SearchedLocation);
            });

        public string GetString(string name, CultureInfo culture, params object[] arguments)
            => string.Format(culture, base.GetString(name, culture) ?? $"[{name}]", arguments);

        public IDictionary<string, string> GetStrings(string name, GetStringsMode getStringsMode = GetStringsMode.OnePerCulture, params object[] arguments)
        {
            switch (getStringsMode)
            {
                case GetStringsMode.OnePerLanguage:
                    return CultureConfig
                        .SupportedLanguages
                        .ToDictionary(
                            language => language.CultureName,
                            language => CultureConfig.TryMatchCulture(language.CultureName, out var culture) ?
                                GetString(name, culture, arguments) :
                                string.Format(culture, $"[{name}]", arguments));
                case GetStringsMode.OnePerCulture:
                    return CultureConfig
                        .SupportedCultures
                        .ToDictionary(
                            culture => culture.Name,
                            culture => GetString(name, culture, arguments));
                default:
                    throw new ArgumentOutOfRangeException(nameof(getStringsMode));
            }
        }
    }
}
