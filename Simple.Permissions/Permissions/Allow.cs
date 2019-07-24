using System.Collections.Generic;
using System.Diagnostics;

namespace Simple.Permissions
{
    /// <summary>
    /// Allowed permission.
    /// </summary>
    [DebuggerDisplay("Allow {Trustee} {PermissionMask} Inherited:{IsInherited}")]
    public class Allow<TIdentity> : IPermission<TIdentity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Allow{TIdentity}" /> class.
        /// </summary>
        public Allow()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Allow{TIdentity}"/> class.
        /// </summary>
        /// <param name="trustee">The trustee id.</param>
        /// <param name="permissionMask">The permission mask.</param>
        public Allow(TIdentity trustee, Right permissionMask)
            : this()
        {
            Trustee = trustee;
            PermissionMask = permissionMask;
        }

        /// <summary>
        /// Gets or sets the trustee id.
        /// </summary>
        /// <value>
        /// The trustee id.
        /// </value>
        public TIdentity Trustee { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is inherited.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is inherited; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsInherited => true;

        /// <summary>
        /// Gets or sets the permission mask.
        /// </summary>
        /// <value>
        /// The permission mask.
        /// </value>
        public Right PermissionMask { get; set; }

        /// <summary>
        /// Checks the access of trustee to the object.
        /// </summary>
        /// <param name="trustee">The trustee.</param>
        /// <param name="accumulator">The accumulator.</param>
        /// <param name="result">The result.</param>
        /// <returns>
        /// A permission result.
        /// </returns>
        public Right CheckAccess(TIdentity trustee, ref Right accumulator, ref Right result)
        {
            // is this permission relevant for the trustee in check?
            if (EqualityComparer<TIdentity>.Default.Equals(trustee,Trustee))
            {
                // is our mask still relevant?
                var effectivePermissions = PermissionMask & ~accumulator;

                // yes, we're apply
                if (effectivePermissions != 0)
                {
                    accumulator |= effectivePermissions; // accumulate our impact
                    result |= effectivePermissions; // and add it also to result
                }
            }

            return result; // irrelevant
        }
    }
}