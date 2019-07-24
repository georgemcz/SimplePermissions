namespace Simple.Permissions
{
    /// <summary>
    /// Calculates the maximum allowed permission set for object.
    /// </summary>
    public class PermissionCalculator<TIdentity> : IPermissionCalculator<TIdentity>
    {
        /// <summary>
        /// The membership provider
        /// </summary>
        private readonly IMembershipProvider<TIdentity> _membershipProvider;

        /// <summary>
        /// The ordered permissions provider
        /// </summary>
        private readonly IOrderedPermissionProvider<TIdentity> _orderedPermissionProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionCalculator{TIdentity}" /> class.
        /// </summary>
        /// <param name="membershipProvider">The membership provider.</param>
        /// <param name="orderedPermissionProvider">The ordered permissions provider.</param>
        public PermissionCalculator(
            IMembershipProvider<TIdentity> membershipProvider,
            IOrderedPermissionProvider<TIdentity> orderedPermissionProvider)
        {
            _membershipProvider = membershipProvider;
            _orderedPermissionProvider = orderedPermissionProvider;
        }

        /// <summary>
        /// Gets available permissions for current user and checked object.
        /// </summary>
        /// <param name="trusteesMembership">The trustees membership.</param>
        /// <param name="protectedObject">The checked object.</param>
        /// <returns>
        /// An enumeration of all allowed permissions for user and object combination.
        /// </returns>
        public Right GetPermissions(
            IObjectIdentity<TIdentity>[][] trusteesMembership,
            TIdentity protectedObject)
        {
            return GetPermissions(trusteesMembership, protectedObject, _membershipProvider.GetMembership(protectedObject));
        }

        /// <summary>
        /// Gets the permissions available for the trustee on protected object.
        /// </summary>
        /// <param name="trusteesMembership">The trustees membership.</param>
        /// <param name="protectedObject">The protected object Id.</param>
        /// <param name="protectedObjectMembership">The protected object's membership.</param>
        /// <param name="context">The permission calculation context - use for repeated calculations.</param>
        /// <returns>
        /// A enumeration of available permissions.
        /// </returns>
        public Right GetPermissions(
            IObjectIdentity<TIdentity>[][] trusteesMembership,
            TIdentity protectedObject,
            IObjectIdentity<TIdentity>[][] protectedObjectMembership,
            PermissionsCalculationContext<TIdentity> context = null)
        {
            // just renaming for the convenience
            var leftTreeMembership = trusteesMembership;
            var rightTreeMembership = protectedObjectMembership;

            Right accumulator = Right.None;
            Right result = Right.None;

            // get ordered permission set on the right tree - cache results for performance
            var rightPermissionsTree = PermissionsCalculationContext<TIdentity>.GetPermissions(
                protectedObject,
                rightTreeMembership,
                _orderedPermissionProvider,
                context);

            // calculate access for parents (different levels)
            for (int l = 0; l < leftTreeMembership.Length; l++)
            {
                var trusteeLevel = leftTreeMembership[l];

                // store the values we have before we start processing the membership groups
                Right groupResult = result;
                Right groupAccumulator = accumulator;
                Right denied = Right.None;

                // calculate access for the groups on the same level
                for (int index = 0; index < trusteeLevel.Length; index++)
                {
                    var trusteeGroup = trusteeLevel[index];
                    // for every node in the same group we started everytime from the beginning
                    Right localResult = result;
                    Right localAccumulator = accumulator;

                    // walk up in distances, getting their permission definitions
                    for (int i = 0; i < rightPermissionsTree.Count; i++)
                    {
                        var orderedPermission = rightPermissionsTree[i];

                        // walk through permissions, accumulating collected permissions
                        orderedPermission.CheckAccess(trusteeGroup.Id, ref localAccumulator, ref localResult);
                        if (localAccumulator == Right.FullControl)
                            break; // we accumulated all available permissions already, no need to continue
                    }

                    // mark the values as accumulated for next groups - but not for members of this group
                    groupAccumulator = groupAccumulator | localAccumulator;
                    groupResult = groupResult | localResult;

                    // we need to remove bits that were allowed by previous groups on the same level
                    // also all denied bits are summed to always remove denied bits once ther're set
                    denied = denied | (localAccumulator & ~localResult);
                    groupResult = groupResult & ~denied; // remove denied bits from group grant
                }

                // now re-consolidate after processing the members on the same level
                accumulator = accumulator | groupAccumulator;
                result = result | groupResult;
            }

            return result;
        }
    }
}