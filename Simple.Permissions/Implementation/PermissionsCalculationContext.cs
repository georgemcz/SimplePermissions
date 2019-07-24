using System.Collections.Generic;
using System.Linq;

namespace Simple.Permissions
{
    /// <summary>
    /// Storage for calculations
    /// </summary>
    public class PermissionsCalculationContext<TIdentity>
    {
        /// <summary>
        /// The permission cache
        /// </summary>
        private readonly Dictionary<TIdentity, List<IPermission<TIdentity>>> _permissionCache 
            = new Dictionary<TIdentity, List<IPermission<TIdentity>>>();

        /// <summary>
        /// Gets the permissions.
        /// </summary>
        /// <param name="protectedObject">The protected object.</param>
        /// <param name="protectedObjectMembership">The protected object membership.</param>
        /// <param name="orderedPermissionProvider">The ordered permission provider.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        internal static List<IPermission<TIdentity>> GetPermissions(
            TIdentity protectedObject,
            IObjectIdentity<TIdentity>[][] protectedObjectMembership,
            IOrderedPermissionProvider<TIdentity> orderedPermissionProvider,
            PermissionsCalculationContext<TIdentity> context)
        {
            if (context == null)
                return orderedPermissionProvider.GetOrderedPermissions(protectedObjectMembership).ToList();

            if (context._permissionCache.TryGetValue(protectedObject, out List<IPermission<TIdentity>> result))
                return result;

            result = orderedPermissionProvider.GetOrderedPermissions(protectedObjectMembership).ToList();

            context._permissionCache.Add(protectedObject, result);
            return result;
        }
    }
}