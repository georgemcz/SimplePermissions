using System.Collections.Concurrent;

namespace Simple.Permissions
{
    /// <summary>
    /// Caches permissions calculated for a session.
    /// </summary>
    public class SessionPermissions<TIdentity> : ISessionPermissions<TIdentity>
    {
        /// <summary>
        /// The cached session permissions are held here
        /// </summary>
        private readonly ConcurrentDictionary<TIdentity, Right> _cachedPermissions
            = new ConcurrentDictionary<TIdentity, Right>();

        /// <summary>
        /// The permission calculator.
        /// </summary>
        private readonly IPermissionCalculator<TIdentity> _calculator;

        /// <summary>
        /// The current user membership.
        /// </summary>
        private readonly IObjectIdentity<TIdentity>[][] _currentUserMembership;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionPermissions{TIdentity}" /> class.
        /// </summary>
        /// <param name="currentUserMembership">The current user membership.</param>
        /// <param name="membershipProvider">The membership provider.</param>
        /// <param name="aclProvider">The acl provider.</param>
        public SessionPermissions(
            IObjectIdentity<TIdentity>[][] currentUserMembership,
            IMembershipProvider<TIdentity> membershipProvider,
            IAclProvider<TIdentity> aclProvider)
        {
            _currentUserMembership = currentUserMembership;
            _calculator = new PermissionCalculator<TIdentity>(membershipProvider, new OrderedPermissionProvider<TIdentity>(aclProvider));
        }

        /// <summary>
        /// Gets the maximal allowed access for specified entity.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns>A permissions for object.</returns>
        public Right GetAccess(TIdentity entityId)
        {
            // get permissions from cache or calculate
            return _cachedPermissions.GetOrAdd(
                entityId,
                PeekAccess);
        }

        /// <summary>
        /// Peeks the maximal allowed access for specified entity.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns>
        /// A permission mask valid for the queried object.
        /// </returns>
        public Right PeekAccess(TIdentity entityId)
        {
            return _calculator.GetPermissions(_currentUserMembership, entityId);
        }

        /// <summary>
        /// Gets the permission when cached or peek them when not.
        /// </summary>
        /// <param name="objectId">The object identifier.</param>
        /// <returns>
        /// A permission mask valid for the queried object.
        /// </returns>
        public Right GetOrPeekPermissions(TIdentity objectId)
        {
            return _cachedPermissions.TryGetValue(objectId, out var permissions)
                ? permissions // return what is cached
                : PeekAccess(objectId); // peek the permissions only otherwise
        }
    }
}