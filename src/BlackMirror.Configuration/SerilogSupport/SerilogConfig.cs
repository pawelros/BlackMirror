namespace BlackMirror.Configuration.SerilogSupport
{
    using System.Xml.Serialization;

    [XmlRoot("serilog")]
    public class SerilogConfig
    {
        [XmlElement("RollingFile.pathFormat")]
        public string LogFilePath { get; set; }

        [XmlElement("RollingFile.serilogPathFormat")]
        public string SerilogLogFilePath { get; set; }

        [XmlElement("RollingFile.retainedFileCountLimit")]
        public int RetainedFileCountLimit { get; set; }

        [XmlElement("RollingFile.fileSizeLimitBytes")]
        public long FileSizeLimitBytes { get; set; }
    }
}
 
