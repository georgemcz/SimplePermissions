namespace Simple.Permissions
{
    /// <summary>
    /// Provides a membership for secured objects.
    /// </summary>
    public interface IMembershipProvider<TIdentity>
    {
        /// <summary>
        /// Gets the membership for defined object.
        /// </summary>
        /// <param name="protectedObjectId">The protected object id.</param>
        /// <returns>A membership for defined object or <c>null</c> when the object is unknown.</returns>
        IObjectIdentity<TIdentity>[][] GetMembership(TIdentity protectedObjectId);
    }
}