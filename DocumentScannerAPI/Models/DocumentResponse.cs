namespace DocumentScannerAPI.Models
{
    public class DocumentResponse
    {
        public string Question { get; set; }
        public string ExtractedText { get; set; }
        public string Answer { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}
