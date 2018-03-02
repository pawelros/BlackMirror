namespace BlackMirror.Svc.Svn
{
    using SharpSvn;

    public class SvnRevision : ISvnRevision
    {
        public string Author { get; set; }

        public string Id { get; set; }

        public string Message { get; set; }

        public SvnLogEventArgs LogObject { get; set; }
    }
}