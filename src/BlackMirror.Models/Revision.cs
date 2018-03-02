namespace BlackMirror.Models
{
    using BlackMirror.Interfaces;
    public class Revision : IRevision
    {
        public string Author { get; set; }

        public string Id { get; set; }

        public string Message { get; set; }
    }
}