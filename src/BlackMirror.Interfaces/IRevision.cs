namespace BlackMirror.Interfaces
{
    public interface IRevision
    {
        string Author { get; }
        string Id { get; }
        string Message { get; set; }
    }
}