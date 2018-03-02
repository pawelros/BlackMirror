namespace BlackMirror.Svc.Svn
{
    using BlackMirror.Interfaces;
    using SharpSvn;

    public interface ISvnRevision : IRevision
    {
        SvnLogEventArgs LogObject { get; }
    }
}