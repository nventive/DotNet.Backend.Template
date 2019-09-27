using System;
using System.Text;
using Newtonsoft.Json;

namespace NV.Templates.Backend.Core.Framework.Continuation
{
    /// <summary>
    /// Helper class to serialize/deserialize continuation tokens.
    /// </summary>
    public static class ContinuationToken
    {
        /// <summary>
        /// Encodes <paramref name="value"/> into a continuation token.
        /// Safe to transmit over HTTP/URI.
        /// </summary>
        /// <param name="value">The value to encode.</param>
        /// <returns>The encoded continuation token.</returns>
        public static string ToContinuationToken(object value)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)));
        }

        /// <summary>
        /// Recompose the <paramref name="continuationToken"/> into a <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The continuation token decoded type.</typeparam>
        /// <param name="continuationToken">The encoded continuation token.</param>
        /// <returns>The decoded continuation token, or default if no continuation token,.</returns>
        public static T? FromContinuationToken<T>(string? continuationToken)
            where T : class
        {
            try
            {
                if (string.IsNullOrEmpty(continuationToken))
                {
                    return default;
                }

                return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(Convert.FromBase64String(continuationToken)));
            }
            catch (Exception ex)
            {
                if (ex is FormatException || ex is JsonReaderException)
                {
                    throw new FormatException($"Malformed continuation token {continuationToken}", ex);
                }

                throw;
            }
        }
    }
}
