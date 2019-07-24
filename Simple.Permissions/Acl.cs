using System.Collections.Generic;
using System.Linq;

namespace Simple.Permissions
{
    /// <summary>
    /// Represents an access control list.
    /// </summary>
    public class Acl<TIdentity>
    {
        /// <summary>
        /// The deny permissions
        /// </summary>
        private readonly List<IPermission<TIdentity>> _denyPermissions = new List<IPermission<TIdentity>>();

        /// <summary>
        /// The allow permissions
        /// </summary>
        private readonly List<IPermission<TIdentity>> _allowPermissions = new List<IPermission<TIdentity>>();

        /// <summary>
        /// Gets the denied permissions.
        /// </summary>
        /// <value>
        /// The denied permissions.
        /// </value>
        public IReadOnlyList<IPermission<TIdentity>> DeniedPermissions { get; private set; } = new IPermission<TIdentity>[0];

        /// <summary>
        /// Gets the allowed permissions.
        /// </summary>
        /// <value>
        /// The allowed permissions.
        /// </value>
        public IReadOnlyList<IPermission<TIdentity>> AllowedPermissions { get; private set; } = new IPermission<TIdentity>[0];

        /// <summary>
        /// Sets the specified allows - designated to load the Acl from persistent store.
        /// No additional checks. For setting the permission from standard code, use Add(Allow)
        /// </summary>
        /// <param name="allows">The allowed permission set.</param>
        public void Load(IEnumerable<Allow<TIdentity>> allows)
        {
            lock (_allowPermissions)
            {
                _allowPermissions.Clear();
                _allowPermissions.AddRange(allows);
                AllowedPermissions = _allowPermissions.ToArray();
            }
        }

        /// <summary>
        /// Sets the specified allows - designated to load the Acl from persistent store.
        /// No additional checks. For setting the permission from standard code, use Add(Allow)
        /// </summary>
        /// <param name="denies">The allowed permission set.</param>
        public void Load(IEnumerable<Deny<TIdentity>> denies)
        {
            lock (_denyPermissions)
            {
                _denyPermissions.Clear();
                _denyPermissions.AddRange(denies);
                DeniedPermissions = _denyPermissions.ToArray();
            }
        }

        /// <summary>
        /// Adds the specified permission.
        /// </summary>
        /// <param name="permission">The permission.</param>
        public virtual void Add(Allow<TIdentity> permission)
        {
            lock (_allowPermissions)
            {
                InternalRevoke(permission.Trustee, permission.PermissionMask);

                var existingAdd = _allowPermissions.FirstOrDefault(p => p.IsInherited && EqualityComparer<TIdentity>.Default.Equals(p.Trustee, permission.Trustee));

                // if there is already definition for trustee, use it, otherwise add the new one
                if (existingAdd == null)
                {
                    _allowPermissions.Add(permission);
                }
                else
                {
                    existingAdd.PermissionMask |= permission.PermissionMask;
                }

                RemoveEmptyEntriesAndUpdateOuputs();
            }
        }

        /// <summary>
        /// Adds the specified permission.
        /// </summary>
        /// <param name="permission">The permission.</param>
        public virtual void Add(AllowNonInherited<TIdentity> permission)
        {
            lock (_allowPermissions)
            {
                InternalRevoke(permission.Trustee, permission.PermissionMask);

                var existingAdd = _allowPermissions.FirstOrDefault(p => !p.IsInherited && EqualityComparer<TIdentity>.Default.Equals(p.Trustee, permission.Trustee));

                // if there is already definition for trustee, use it, otherwise add the new one
                if (existingAdd == null)
                {
                    _allowPermissions.Add(permission);
                }
                else
                {
                    existingAdd.PermissionMask |= permission.PermissionMask;
                }

                RemoveEmptyEntriesAndUpdateOuputs();
            }
        }

        /// <summary>
        /// Adds the specified permission.
        /// </summary>
        /// <param name="permission">The permission.</param>
        public virtual void Add(Deny<TIdentity> permission)
        {
            lock (_denyPermissions)
            {
                InternalRevoke(permission.Trustee, permission.PermissionMask);

                var existingAdd = _denyPermissions.FirstOrDefault(p => p.IsInherited && EqualityComparer<TIdentity>.Default.Equals(p.Trustee, permission.Trustee));

                // if there is already definition for trustee, use it, otherwise add the new one
                if (existingAdd == null)
                {
                    _denyPermissions.Add(permission);
                }
                else
                {
                    existingAdd.PermissionMask |= permission.PermissionMask;
                }

                RemoveEmptyEntriesAndUpdateOuputs();
            }
        }

        /// <summary>
        /// Adds the specified permission.
        /// </summary>
        /// <param name="permission">The permission.</param>
        public virtual void Add(DenyNonInherited<TIdentity> permission)
        {
            lock (_denyPermissions)
            {
                InternalRevoke(permission.Trustee, permission.PermissionMask);

                var existingAdd = _denyPermissions.FirstOrDefault(p => !p.IsInherited && EqualityComparer<TIdentity>.Default.Equals(p.Trustee, permission.Trustee));

                // if there is already definition for trustee, use it, otherwise add the new one
                if (existingAdd == null)
                {
                    _denyPermissions.Add(permission);
                }
                else
                {
                    existingAdd.PermissionMask |= permission.PermissionMask;
                }

                RemoveEmptyEntriesAndUpdateOuputs();
            }
        }

        /// <summary>
        /// Revokes the permission for specified trustee.
        /// </summary>
        /// <param name="trusteeId">The trustee id.</param>
        /// <param name="permission">The permission.</param>
        public virtual void Revoke(TIdentity trusteeId, Right permission)
        {
            InternalRevoke(trusteeId, permission);
        }

        /// <summary>
        /// Revokes the permission without notification.
        /// </summary>
        /// <param name="trusteeId">The trustee id.</param>
        /// <param name="permission">The permission.</param>
        private void InternalRevoke(TIdentity trusteeId, Right permission)
        {
            lock (_allowPermissions)
            lock (_denyPermissions)
            {
                // revoke deny permissions
                var denials = _denyPermissions.Where(p => EqualityComparer<TIdentity>.Default.Equals(p.Trustee, trusteeId));
                foreach (var denial in denials)
                    denial.PermissionMask &= ~permission;

                // revoke allow permissions
                var allows = _allowPermissions.Where(p => EqualityComparer<TIdentity>.Default.Equals(p.Trustee, trusteeId));
                foreach (var allow in allows)
                    allow.PermissionMask &= ~permission;

                RemoveEmptyEntriesAndUpdateOuputs();
            }
        }

        /// <summary>
        /// Removes the empty entries.
        /// </summary>
        private void RemoveEmptyEntriesAndUpdateOuputs()
        {
            lock (_allowPermissions)
            lock (_denyPermissions)
            {
                // reduce chaoss by removing empty entries
                _denyPermissions.RemoveAll(p => p.PermissionMask == Right.None);
                _allowPermissions.RemoveAll(p => p.PermissionMask == Right.None);

                DeniedPermissions = _denyPermissions.ToArray();
                AllowedPermissions = _allowPermissions.ToArray();
            }
        }
    }
}