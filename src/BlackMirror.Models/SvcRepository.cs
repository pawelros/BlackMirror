namespace BlackMirror.Models
{
    using BlackMirror.Interfaces;
    public class SvcRepository : ISvcRepository
    {
        public string Id { get; set; }

        public SvcRepositoryType Type { get; set; }

        public string Name { get; set; }

        public string Uri { get; set; }

        public string DefaultCommitMessagePrefix{ get; set; }

        public IUser CheckoutUser { get; set; }

        public IUser PushUser { get; set; }

        public ICredentials MappedCheckoutCredentials { get; set; }
    }
}