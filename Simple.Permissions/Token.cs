using System.Collections.Generic;

namespace Simple.Permissions
{
    /// <summary>
    /// Representation of the security token for current user.
    /// </summary>
    /// <typeparam name="TIdentity">The type of the identity.</typeparam>
    /// <seealso cref="IToken{TIdentity}" />
    public class Token<TIdentity> : IToken<TIdentity>
    {
        /// <summary>
        /// The permissions calculator.
        /// </summary>
        private readonly ISessionPermissions<TIdentity> _permissions;

        /// <summary>
        /// Initializes a new instance of the <see cref="Token{TIdentity}" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="permissions">The permissions.</param>
        public Token(TIdentity id, ISessionPermissions<TIdentity> permissions)
        {
            _permissions = permissions;
            Id = id;
        }

        /// <summary>
        /// Gets the unique identifier for the object identity.
        /// </summary>
        /// <value>
        /// The object identity.
        /// </value>
        public TIdentity Id { get; }

        /// <summary>
        /// Gets the permissions.
        /// </summary>
        /// <param name="objectId">The object id.</param>
        /// <returns>A permission flags for the current object id</returns>
        public Right GetPermissions(TIdentity objectId)
        {
            // get permission
            var access = _permissions.GetAccess(objectId);

            // whether the entity is me, I have always permission to view it
            if (EqualityComparer<TIdentity>.Default.Equals(objectId, Id)) access |= Right.View;

            return access;
        }

        /// <summary>
        /// Gets the permissions for object. (not cached)
        /// </summary>
        /// <param name="objectId">The object id.</param>
        /// <returns>
        /// A permission set.
        /// </returns>
        public Right PeekPermissions(TIdentity objectId)
        {
            return _permissions.PeekAccess(objectId);
        }

        /// <summary>
        /// Gets the permissions when cached, or peeks them when not
        /// </summary>
        /// <param name="objectId">The object identifier.</param>
        /// <returns>
        ///   <c>true</c> when permissions were already cached; otherwise <c>false</c>.
        /// </returns>
        public Right GetOrPeekPermissions(TIdentity objectId)
        {
            return _permissions.GetOrPeekPermissions(objectId);
        }
    }
}