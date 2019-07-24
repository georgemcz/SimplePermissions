using System;

namespace Simple.Permissions
{
    /// <summary>
    /// Initialization helper
    /// </summary>
    /// <typeparam name="TIdentity">The type of the identity.</typeparam>
    public class PermissionEngine<TIdentity>
    {
        /// <summary>
        /// The membership provider
        /// </summary>
        private readonly IMembershipProvider<TIdentity> _membershipProvider;

        /// <summary>
        /// The acl provider
        /// </summary>
        private readonly IAclProvider<TIdentity> _aclProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionEngine{TIdentity}"/> class.
        /// </summary>
        /// <param name="membershipProvider">The membership provider.</param>
        /// <param name="aclProvider">The acl provider.</param>
        /// <exception cref="ArgumentNullException">
        /// membershipProvider
        /// or
        /// aclProvider
        /// </exception>
        public PermissionEngine(IMembershipProvider<TIdentity> membershipProvider, IAclProvider<TIdentity> aclProvider)
        {
            _membershipProvider = membershipProvider ?? throw new ArgumentNullException(nameof(membershipProvider));
            _aclProvider = aclProvider ?? throw new ArgumentNullException(nameof(aclProvider));
        }

        /// <summary>
        /// Starts the session for user (entity) defined by the unique identity.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Unknown membership for creating token (userId:{userId})</exception>
        public IToken<TIdentity> StartSession(TIdentity userId)
        {
            // lookup the current users membership
            var currentUserMembership = _membershipProvider.GetMembership(userId);
            if (currentUserMembership == null)
                throw new InvalidOperationException($"Unknown membership for creating token (userId:{userId})");

            var session = new SessionPermissions<TIdentity>(currentUserMembership, _membershipProvider, _aclProvider);

            return new Token<TIdentity>(userId, session);
        }
    }
}