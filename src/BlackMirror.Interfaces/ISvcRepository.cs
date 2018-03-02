namespace BlackMirror.Interfaces
{
    using BlackMirror.Models;

    public interface ISvcRepository : IModel
    {
        SvcRepositoryType Type { get; }
        string Name { get; }
        string Uri { get; }
        string DefaultCommitMessagePrefix { get; }
        IUser CheckoutUser { get; }
        IUser PushUser { get; }
        ICredentials MappedCheckoutCredentials { get; }
    }
}