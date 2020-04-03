using System;
using System.Text.RegularExpressions;
using NJsonSchema.Generation;

namespace NV.Templates.Backend.Web.Framework.OpenApi
{
    /// <summary>
    /// Custom <see cref="ISchemaNameGenerator"/> that removes the -Model suffix from OpenApi generated schemas.
    /// Also handles the specific case of <see cref="ContinuationEnumerableModel"/>.
    /// </summary>
    internal class CustomSchemaNameGenerator : DefaultSchemaNameGenerator
    {
        public override string Generate(Type type)
        {
            var name = base.Generate(type);
            name = Regex.Replace(name, "Model$", string.Empty);
            name = name.Replace("ContinuationEnumerableModelOf", "PartialEnumerationOf", StringComparison.Ordinal);

            return name;
        }
    }
}
