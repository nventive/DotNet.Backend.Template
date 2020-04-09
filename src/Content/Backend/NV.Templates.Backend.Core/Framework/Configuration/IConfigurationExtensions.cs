using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// <see cref="IConfiguration"/> extension methods.
    /// </summary>
    public static class IConfigurationExtensions
    {
        /// <summary>
        /// Returns the <see cref="IConfigurationSection"/> for the options represented by <typeparamref name="T"/>.
        /// The section name is either the options type name (minus the -Options prefix) or <paramref name="key"/>, if provided.
        /// <see cref="DefaultOptionsName{T}"/> as well.
        /// </summary>
        /// <typeparam name="T">The options type.</typeparam>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <param name="key">The section name key, if any.</param>
        /// <returns>The <see cref="IConfigurationSection"/>.</returns>
        public static IConfigurationSection GetSectionForOptions<T>(
            this IConfiguration configuration,
            string? key = null)
            => configuration.GetSection(key ?? DefaultOptionsName<T>());

        /// <summary>
        /// Reads the current value for options and performs validation.
        /// The section name is either the options type name (minus the -Options prefix) or <paramref name="key"/>, if provided.
        /// <see cref="DefaultOptionsName{T}"/> as well.
        /// </summary>
        /// <typeparam name="T">The options type.</typeparam>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <param name="key">The section name key, if any.</param>
        /// <returns>The current value for <typeparamref name="T"/>.</returns>
        /// <exception cref="ValidationException">If the validation fails.</exception>
        public static T ReadOptionsAndValidate<T>(this IConfiguration configuration, string? key = null)
            where T : new()
        {
            var options = configuration.GetSection(key ?? DefaultOptionsName<T>()).Get<T>() ?? new T();

            Validator.ValidateObject(options, new ValidationContext(options));
            return options;
        }

        /// <summary>
        /// Gets the default name for options of type <paramref name="optionsType"/>.
        /// Removes the -Options prefix if any.
        /// </summary>
        public static string DefaultOptionsName(Type optionsType) => Regex.Replace(optionsType.Name, @"Options$", string.Empty);

        /// <summary>
        /// Gets the default name for options of type <typeparamref name="T"/>.
        /// Removes the -Options prefix if any.
        /// </summary>
        /// <typeparam name="T">The Options type.</typeparam>
        public static string DefaultOptionsName<T>() => DefaultOptionsName(typeof(T));
    }
}
