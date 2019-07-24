namespace Simple.Permissions
{
    /// <summary>
    /// Object that has identity, it can support permission calculations on that.
    /// </summary>
    /// <typeparam name="T">Identity type</typeparam>
    public interface IObjectIdentity<out T>
    {
        /// <summary>
        /// Gets the unique identifier for the object identity.
        /// </summary>
        /// <value>
        /// The object identity.
        /// </value>
        T Id { get; }
    }
}