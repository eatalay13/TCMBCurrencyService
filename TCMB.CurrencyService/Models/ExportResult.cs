namespace TCMB.CurrencyService.Models
{
    public class ExportResult
    {
        public byte[] Data { get; set; }
        public string MimeType { get; set; }
        public string FileExtension { get; set; }
    }
}
