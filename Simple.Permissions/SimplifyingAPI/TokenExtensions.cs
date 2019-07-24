namespace Simple.Permissions
{
    /// <summary>
    /// Simple extensions for token interface to avoid polluting it with unnecessary methods.
    /// </summary>
    public static class TokenExtensions
    {
        /// <summary>
        /// Checks the permission.
        /// </summary>
        /// <typeparam name="TIdentity">The type of the identity.</typeparam>
        /// <param name="token">The token.</param>
        /// <param name="on">The object identifier.</param>
        /// <param name="to">The required access.</param>
        /// <returns></returns>
        public static bool HasRight<TIdentity>(
            this IToken<TIdentity> token,
            TIdentity on,
            Right to)
        {
            var granted = token.GetPermissions(on);
            return (granted & to) == to;
        }

        public static bool HasRight<TIdentity>(
            this IToken<TIdentity> token,
            IObjectIdentity<TIdentity> on,
            Right to)
        {
            var granted = token.GetPermissions(on.Id);
            return (granted & to) == to;
        }
    }
}