namespace SharpArch.Wcf
{
    using JetBrains.Annotations;

    /// <summary>
    ///     When implemented by your WCF contracts, they are then interchangeable with WCF client proxies.
    ///     This makes it simpler to use dependency injection and to mock the WCF services without
    ///     having to worry about if it's a WCF client when you go to close/abort it.
    /// </summary>
    [PublicAPI]
    public interface ICloseableAndAbortable
    {
        /// <summary>
        /// Aborts connection.
        /// </summary>
        void Abort();

        /// <summary>
        /// Closes connection.
        /// </summary>
        void Close();
    }
}