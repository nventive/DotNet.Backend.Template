using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NV.Templates.Backend.Core.Framework.Internationalization
{
    public static class CultureConfig
    {
        public static readonly CultureInfo DefaultCulture = new CultureInfo("en");

        public static readonly IEnumerable<CultureInfo> SupportedCultures = new[]
        {
            DefaultCulture,
            new CultureInfo("fr-CA"),
        };

        public static readonly IEnumerable<LanguageModel> SupportedLanguages = SupportedCultures
            .Select(culture => new LanguageModel
            {
                DisplayName = culture.NativeName,
                CultureName = culture.Name,
            })
            .Distinct();

        public static bool TryMatchCulture(string languageOrCultureName, out CultureInfo culture, CultureInfo? defaultCulture = null)
        {
            CultureInfo? match = null;
            if (!string.IsNullOrWhiteSpace(languageOrCultureName))
            {
                match = SupportedCultures
                    // The exact matching culture first
                    .OrderByDescending(culture => culture.Name.Equals(languageOrCultureName, StringComparison.OrdinalIgnoreCase))

                    // Then the other matching languages
                    .ThenByDescending(culture => culture.TwoLetterISOLanguageName.StartsWith(languageOrCultureName, StringComparison.OrdinalIgnoreCase) ||
                        languageOrCultureName.StartsWith(culture.TwoLetterISOLanguageName, StringComparison.OrdinalIgnoreCase))

                    // Get the first that either exactly matches the culture, of the first culture that matches the language.
                    .FirstOrDefault(culture => culture.Name.Equals(languageOrCultureName, StringComparison.OrdinalIgnoreCase) ||
                        culture.TwoLetterISOLanguageName.StartsWith(languageOrCultureName, StringComparison.OrdinalIgnoreCase) ||
                        languageOrCultureName.StartsWith(culture.TwoLetterISOLanguageName, StringComparison.OrdinalIgnoreCase));
            }

            culture = match ?? defaultCulture ?? DefaultCulture;

            return match != null;
        }

        public static void SetCulture(string languageOrCultureName, CultureInfo? fallback = null)
        {
            TryMatchCulture(languageOrCultureName, out var culture, fallback);
            SetCulture(culture);
        }

        public static void SetCulture(CultureInfo culture)
        {
            CultureInfo.CurrentUICulture = culture;
            CultureInfo.CurrentCulture = culture;
        }

        public class LanguageModel
        {
            public string? DisplayName { get; set; }

            public string? CultureName { get; set; }
        }
    }
}
