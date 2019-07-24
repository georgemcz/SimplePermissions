using System.Diagnostics;

namespace Simple.Permissions
{
    /// <summary>
    /// Allow permission with delegation.
    /// </summary>
    [DebuggerDisplay("Allow {Trustee} {PermissionMask} Inherited:{IsInherited}")]
    public class AllowNonInherited<TIdentity> : Allow<TIdentity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllowNonInherited{TIdentity}" /> class.
        /// </summary>
        public AllowNonInherited()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllowNonInherited{TIdentity}"/> class.
        /// </summary>
        /// <param name="trustee">The trustee id.</param>
        /// <param name="permissionMask">The permission mask.</param>
        public AllowNonInherited(TIdentity trustee, Right permissionMask)
            : base(trustee, permissionMask)
        { }

        /// <summary>
        /// Gets a value indicating whether this instance is inherited.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is inherited; otherwise, <c>false</c>.
        /// </value>
        public override bool IsInherited => false;
    }
}