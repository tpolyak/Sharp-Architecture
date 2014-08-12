namespace SharpArch.Wcf
{
    /// <summary>
    ///     When implemented by your WCF contracts, they are then interchangable with WCF client proxies.
    ///     This makes it simpler to use dependency injection and to mock the WCF services without
    ///     having to worry about if it's a WCF client when you go to close/abort it.
    /// </summary>
    public interface ICloseableAndAbortable
    {
        void Abort();

        void Close();
    }
}