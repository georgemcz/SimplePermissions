using System.Collections.Generic;
using System.Linq;

namespace Simple.Permissions
{
    /// <summary>
    /// Derive from this class, when you are developing simple membership provider, where is no need for groups
    /// </summary>
    /// <typeparam name="TIdentity">The type of the identity.</typeparam>
    /// <seealso cref="IMembershipProvider{TIdentity}" />
    public abstract class SimplifiedMembershipProvider<TIdentity> : IMembershipProvider<TIdentity>
    {
        /// <summary>
        /// Gets the membership for defined object.
        /// </summary>
        /// <param name="protectedObjectId">The protected object id.</param>
        /// <returns>
        /// A membership for defined object or <c>null</c> when the object is unknown.
        /// </returns>
        public IObjectIdentity<TIdentity>[][] GetMembership(TIdentity protectedObjectId)
        {
            return GetParents(protectedObjectId)?.Select(p => new[] { p }).ToArray();
        }

        /// <summary>
        /// Gets the parents.
        /// </summary>
        /// <param name="objectId">The object identifier.</param>
        /// <returns>
        /// A membership for defined object or <c>null</c> when the object is unknown.
        /// </returns>
        protected abstract IEnumerable<IObjectIdentity<TIdentity>> GetParents(TIdentity objectId);
    }
}