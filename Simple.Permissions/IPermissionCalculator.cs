namespace Simple.Permissions
{
    /// <summary>
    /// Object used for calculating the available permission set for protected object and trustee.
    /// </summary>
    public interface IPermissionCalculator<TIdentity>
    {
        /// <summary>
        /// Gets the permissions available for the trustee on protected object.
        /// </summary>
        /// <param name="trusteesMembership">The trustees membership.</param>
        /// <param name="protectedObject">The protected object.</param>
        /// <returns>
        /// A enumeration of available permissions.
        /// </returns>
        Right GetPermissions(IObjectIdentity<TIdentity>[][] trusteesMembership, TIdentity protectedObject);

        /// <summary>
        /// Gets the permissions available for the trustee on protected object.
        /// </summary>
        /// <param name="trusteesMembership">The trustees membership.</param>
        /// <param name="protectedObject">The protected object.</param>
        /// <param name="protectedObjectMembership">The protected object's membership.</param>
        /// <param name="context">The permission calculation context - use for repeated calculations.</param>
        /// <returns>
        /// A enumeration of available permissions.
        /// </returns>
        Right GetPermissions(
            IObjectIdentity<TIdentity>[][] trusteesMembership,
            TIdentity protectedObject,
            IObjectIdentity<TIdentity>[][] protectedObjectMembership,
            PermissionsCalculationContext<TIdentity> context = null);
    }
}