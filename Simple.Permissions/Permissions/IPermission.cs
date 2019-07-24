namespace Simple.Permissions
{
    /// <summary>
    /// A generic permission.
    /// </summary>
    public interface IPermission<TIdentity>
    {
        /// <summary>
        /// Gets or sets the object id the permission is defined for.
        /// </summary>
        /// <value>
        /// The object id.
        /// </value>
        TIdentity Trustee { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is inherited.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is inherited; otherwise, <c>false</c>.
        /// </value>
        bool IsInherited { get; }

        /// <summary>
        /// Gets or sets the permission mask.
        /// </summary>
        /// <value>
        /// The permission mask.
        /// </value>
        Right PermissionMask { get; set; }

        /// <summary>
        /// Checks the access of trustee to the object.
        /// </summary>
        /// <param name="trustee">The trustee.</param>
        /// <param name="accumulator">The accumulator.</param>
        /// <param name="result">The result.</param>
        /// <returns>A permission result.</returns>
        Right CheckAccess(TIdentity trustee, ref Right accumulator, ref Right result);
    }
}
