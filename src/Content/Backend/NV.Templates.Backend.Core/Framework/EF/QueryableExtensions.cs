using System;
using System.Linq;
using System.Threading.Tasks;
using NV.Templates.Backend.Core.Framework;
using NV.Templates.Backend.Core.Framework.Continuation;
using NV.Templates.Backend.Core.Framework.Exceptions;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// Extension methods for <see cref="IQueryable{T}"/>.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Asynchronously returns the first element of a sequence by its id or
        /// throw a <see cref="NotFoundException"/> if it is not found.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="queryable">The <see cref="IQueryable{T}"/>.</param>
        /// <param name="id">The id to search for.</param>
        /// <returns>The entity if found.</returns>
        /// <exception cref="NotFoundException">If the entity is not found.</exception>
        public static async Task<TEntity> FirstByIdOrNotFoundAsync<TEntity>(this IQueryable<TEntity> queryable, string id)
            where TEntity : class, IIdentifiable
        {
            if (queryable is null)
            {
                throw new ArgumentNullException(nameof(queryable));
            }

            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var result = await queryable.FirstOrDefaultAsync(x => x.Id == id);
            if (result is null)
            {
                throw new NotFoundException($"{typeof(TEntity).Name}/{id}");
            }

            return result;
        }

        /// <summary>
        /// Asynchronously runs the <paramref name="queryable"/> and apply pagination based on the <paramref name="continuationQuery"/>.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="queryable">The <see cref="IQueryable{T}"/>.</param>
        /// <param name="continuationQuery">The <see cref="IContinuationQuery"/>.</param>
        /// <returns>The <see cref="IContinuationEnumerable{T}"/> with the next continuation token if any.</returns>
        public static async Task<IContinuationEnumerable<TEntity>> ToContinuationEnumerableAsync<TEntity>(this IQueryable<TEntity> queryable, IContinuationQuery continuationQuery)
            where TEntity : class
        {
            if (queryable is null)
            {
                throw new ArgumentNullException(nameof(queryable));
            }

            if (continuationQuery is null)
            {
                throw new ArgumentNullException(nameof(continuationQuery));
            }

            var pagination = LimitOffsetContinuationToken.FromContinuationQuery(continuationQuery);

            var paginatedResults = await queryable
                .Select(x => new
                {
                    Item = x,
                    TotalCount = queryable.Count(),
                })
                .Skip(pagination.Offset)
                .Take(pagination.Limit)
                .ToArrayAsync();

            var totalCount = paginatedResults.FirstOrDefault()?.TotalCount ?? 0;
            var items = paginatedResults.Select(x => x.Item).ToList();

            return new ContinuationEnumerableDecorator<TEntity>(
                items,
                (items.Count + pagination.Offset) < totalCount ? pagination.GetNextPageContinuationToken() : null);
        }
    }
}
