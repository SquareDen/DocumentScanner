namespace DocumentScannerAPI.Models
{
    public class DocumentRequest
    {
        public IFormFile ContractPhoto { get; set; }
        public string UserQuestion { get; set; }
    }
}
