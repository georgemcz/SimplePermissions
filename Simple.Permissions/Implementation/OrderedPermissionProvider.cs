using System.Collections.Generic;
using System.Linq;

namespace Simple.Permissions
{
    /// <summary>
    /// Ordered permissions provider
    /// </summary>
    public class OrderedPermissionProvider<TIdentity> : IOrderedPermissionProvider<TIdentity>
    {
        /// <summary>
        /// The acl provider
        /// </summary>
        private readonly IAclProvider<TIdentity> _aclProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderedPermissionProvider{TIdentity}" /> class.
        /// </summary>
        /// <param name="aclProvider">The acl provider.</param>
        public OrderedPermissionProvider(IAclProvider<TIdentity> aclProvider)
        {
            _aclProvider = aclProvider;
        }

        /// <summary>
        /// Gets the ordered permissions.
        /// Permissions are ordered in
        /// 1) membership order (distance)
        /// 2) on the same distance, denies goes first, then allows
        /// 3) when there are more groups in the same distance, all denies from groups in that distance goes first
        /// </summary>
        /// <param name="membership">The membership.</param>
        /// <returns>An ordered permission set.</returns>
        public IEnumerable<IPermission<TIdentity>> GetOrderedPermissions(IEnumerable<IObjectIdentity<TIdentity>[]> membership)
        {
            // here is the place where the most magical stuff happens
            // it orders permission into the order of:
            // me (denied, allowed), my parent (denied, allowed), parents parent (denied, allowed)
            // where there can be more parents on the single level - e.g. groups.
            // Then it goes like that: (gr1:denied, gr2:denied, gr1:allowed, gr2:allowed)
            // it also ignores all non-inherited permissions for levels other than 0 = direct me
            return membership
                .Select(dist => dist.Select(s => _aclProvider.GetById(s.Id)).Where(distAcl => distAcl != null).ToArray())
                .SelectMany((distAcl, i) => distAcl.SelectMany(acl => acl.DeniedPermissions.Where(a => a.IsInherited || i == 0)) // take denied everywhere and denied non-inherited only on first level
                    .Concat(distAcl.SelectMany(acl => acl.AllowedPermissions.Where(a => a.IsInherited || i == 0)))); // take allowed everywhere and allowed non-inherited only on first level
        }
    }
}