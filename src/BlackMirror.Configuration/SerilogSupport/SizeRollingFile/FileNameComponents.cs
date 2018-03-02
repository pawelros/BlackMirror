namespace BlackMirror.Configuration.SerilogSupport.SizeRollingFile
{
    public class FileNameComponents
    {
        internal readonly string Name;
        internal readonly uint Sequence;
        internal readonly string Extension;
        internal readonly string FullName;
        private const string FullNameFormat = "{0}-{1}.{2}";
        private const string FullNameNoExtension = "{0}-{1}";

        public FileNameComponents(string name, uint sequence, string extension)
        {
            this.Name = name;
            this.Sequence = sequence;
            this.Extension = extension;
            this.FullName = string.IsNullOrWhiteSpace(extension)
                ? string.Format(FullNameNoExtension, name, sequence)
                : string.Format(FullNameFormat, name, sequence, extension);
        }
    }
}
