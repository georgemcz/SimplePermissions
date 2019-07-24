using System.Collections.Generic;

namespace Simple.Permissions
{
    /// <summary>
    /// API for ordered permissions
    /// </summary>
    public interface IOrderedPermissionProvider<TIdentity>
    {
        /// <summary>
        /// Gets the ordered permissions.
        /// Permissions are ordered in
        /// 1) membership order (distance)
        /// 2) on the same distance, denies goes first, then allows
        /// 3) when there are more groups in the same distance, all denies from groups in that distance goes first
        /// </summary>
        /// <param name="membership">The membership.</param>
        /// <returns>An ordered permission set.</returns>
        IEnumerable<IPermission<TIdentity>> GetOrderedPermissions(IEnumerable<IObjectIdentity<TIdentity>[]> membership);
    }
}