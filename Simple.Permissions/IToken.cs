namespace Simple.Permissions
{
    /// <summary>
    /// Token represents the current (typically logged-in) entity as an user or so.
    /// </summary>
    /// <typeparam name="TIdentity">The type of the identity.</typeparam>
    /// <seealso cref="IObjectIdentity{TIdentity}" />
    public interface IToken<TIdentity> : IObjectIdentity<TIdentity>
    {
        /// <summary>
        /// Gets and caches the result of the permissions for object.
        /// </summary>
        /// <param name="objectId">The object id.</param>
        /// <returns>A permission set.</returns>
        Right GetPermissions(TIdentity objectId);

        /// <summary>
        /// Gets the permissions for object. (not cached)
        /// </summary>
        /// <param name="objectId">The object id.</param>
        /// <returns>A permission set.</returns>
        Right PeekPermissions(TIdentity objectId);

        /// <summary>
        /// Determines whether there are already cached permissions for defined object
        /// </summary>
        /// <param name="objectId">The object identifier.</param>
        /// <returns><c>true</c> when permissions were already cached; otherwise <c>false</c>.</returns>
        Right GetOrPeekPermissions(TIdentity objectId);
    }
}