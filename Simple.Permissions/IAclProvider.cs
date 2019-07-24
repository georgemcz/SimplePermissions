using System.Collections.Generic;

namespace Simple.Permissions
{
    /// <summary>
    /// Provides a current ACLs.
    /// </summary>
    public interface IAclProvider<TIdentity>
    {
        /// <summary>
        /// Gets the acl for defined object is.
        /// </summary>
        /// <param name="objectId">The object id.</param>
        /// <returns>An ACL for defined secured object.</returns>
        Acl<TIdentity> GetById(TIdentity objectId);
    }
}