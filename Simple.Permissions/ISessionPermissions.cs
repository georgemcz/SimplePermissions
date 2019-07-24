namespace Simple.Permissions
{
    /// <summary>
    /// Cached session permissions
    /// </summary>
    public interface ISessionPermissions<in TIdentity>
    {
        /// <summary>
        /// Gets the maximal allowed access for specified entity.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns>
        /// A permission mask valid for the queried object.
        /// Value is cached for subsequent calls.
        /// </returns>
        Right GetAccess(TIdentity entityId);

        /// <summary>
        /// Peeks the maximal allowed access for specified entity.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns>A permission mask valid for the queried object.</returns>
        Right PeekAccess(TIdentity entityId);

        /// <summary>
        /// Gets the permission when cached or peek them when not.
        /// </summary>
        /// <param name="objectId">The object identifier.</param>
        /// <returns>A permission mask valid for the queried object.</returns>
        Right GetOrPeekPermissions(TIdentity objectId);
    }
}