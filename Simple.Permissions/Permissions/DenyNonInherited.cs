using System.Diagnostics;

namespace Simple.Permissions
{
    /// <summary>
    /// Deny permission with delegation.
    /// </summary>
    /// <typeparam name="TIdentity">The type of the identity.</typeparam>
    /// <seealso cref="Simple.Permissions.Deny{TIdentity}" />
    [DebuggerDisplay("Deny {Trustee} {PermissionMask} Inherited:{IsInherited}")]
    public class DenyNonInherited<TIdentity> : Deny<TIdentity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DenyNonInherited{TIdentity}" /> class.
        /// </summary>
        public DenyNonInherited()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DenyNonInherited{TIdentity}"/> class.
        /// </summary>
        /// <param name="trustee">The trustee id.</param>
        /// <param name="permissionMask">The permission mask.</param>
        public DenyNonInherited(TIdentity trustee, Right permissionMask)
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